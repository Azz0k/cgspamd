import {makeAutoObservable} from "mobx";
import type {User} from "@/content/users/user-columns.tsx";
import {deleteUser, loadAllUsers, updateUser} from "@/services/users.api.ts";
import {rootStore} from "@/store/root-store.ts";
import type {ChangeEvent} from "react";

type NewUser = {
  userName: string;
  fullName: string,
  password: string;
  enabled: boolean,
  isAdmin: boolean,
}

type Users = User[];
class UserState {
  constructor(){
    makeAutoObservable(this);
  }
  usersData: Users = [];
  loading: boolean = false;
  error: string | null = null;
  passwordChangeValues = {
    password1: "",
    password2: ""
  }
  handleCancelAction = () =>{
    this.passwordChangeValues.password1 = "";
    this.passwordChangeValues.password2 = "";
    this.error = null;
  }
  handlePassword1ChangeValue = (event: ChangeEvent<HTMLInputElement>) => {
    this.passwordChangeValues.password1 = event.target.value;
  }
  handlePassword2ChangeValue = (event: ChangeEvent<HTMLInputElement>) => {
    this.passwordChangeValues.password2 = event.target.value;
  }
  handlePasswordChange = async (user:User) =>{
    this.error = null;
    if (this.passwordChangeValues.password1 !== this.passwordChangeValues.password2) {
      this.error = "Пароли не совпадают";
      return false;
    }
    if (this.passwordChangeValues.password1.length<8) {
      this.error = "Пароль слишком короткий";
      return false;
    }
    this.loading = true;
    const newUser: NewUser = {
      userName: user.userName,
      fullName: user.fullName,
      password: this.passwordChangeValues.password1,
      enabled:user.enabled,
      isAdmin: user.isAdmin,
    }
    const result = await this.UpdateUser(user.id, newUser);
    if (result) {
      this.passwordChangeValues.password1 = "";
      this.passwordChangeValues.password2 = "";
    }
    this.loading = false;
    return result;
  }
  handleDeleteUser = async (id:number) => {
    this.loading = true;
    const result = await this.DeleteUser(id);
    if(result){
      this.usersData = this.usersData.filter(user => user.id !== id);
      this.error = null;
    }
    else {
      this.error = "Не удалось удалить пользователя. Попробуйте обновить страницу."
    }
    this.loading = false;
    return result;
  }

  async LoadAllUsers(){
    this.loading = true;
    try{
      this.usersData = await loadAllUsers() as Users;
    }
    catch(error:unknown){
      switch (error){
        case 403:
        case 401:
          this.usersData = [];
          rootStore.handleLogout();
          break;
        default:
          break;
      }
    }
    finally{
      this.loading = false;
    }
  }
  async UpdateUser(id:number, user:NewUser |null = null) {
    this.error = null;
    try{
      let body: string;
      if (user === null) {
        body = JSON.stringify(this.usersData.find(value => value.id === id));
      }
      else
        body = JSON.stringify({...user, id});
      await updateUser(body);
      return true;
    }
    catch (error:unknown) {
      switch (error){
        case 403:
        case 401:
          this.usersData = [];
          rootStore.handleLogout();
          break;
        case 400:
          this.error = 'Неверные данные';
          break;
        case 404:
          this.error = 'Не найден пользователь. Обновите страницу.';
          break;
        default:
          this.error = 'Неизвестная ошибка';
          break;
      }
      return false;
    }
  }
  async DeleteUser(id:number) {
    try {
      const code = await deleteUser(id);
      if (code === 401 || code === 403) {
        this.usersData = [];
        rootStore.handleLogout();
        return false;
      }
      return code===204;
    }
    catch  {
      return false;
    }
  }
}

export const userState = new UserState();