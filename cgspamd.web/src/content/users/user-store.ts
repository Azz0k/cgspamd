import {makeAutoObservable} from "mobx";
import type {User} from "@/content/users/user-columns.tsx";
import {addUser, deleteUser, loadAllUsers, updateUser} from "@/services/users.api.ts";
import {rootStore} from "@/store/root-store.ts";
import {type DraftUser, userMutationStore} from "@/content/users/user-mutation-store.ts";

type NewUser = {
  userName: string;
  fullName: string,
  password: string;
  enabled: boolean,
  isAdmin: boolean,
}

type Users = User[];
class UserStore {
  constructor(){
    makeAutoObservable(this);
  }
  usersData: Users = [];
  loading: boolean = false;
  error: string | null = null;
  userMutationStore = userMutationStore;


  handleCancelAction = () =>{
    this.userMutationStore.clear();
    this.error = null;
  }
  handleAddUser = async () => {
    this.error = null;
    if (!this.userMutationStore.validateDraft()){
      this.error = this.userMutationStore.error;
      return false;
    }
    if (!this.userMutationStore.validatePassword()) {
      this.error = this.userMutationStore.error;
      return false;
    }
    this.loading = true;
    const result = await this.AddUser({...this.userMutationStore.draft, password:this.userMutationStore.passwordChangeValues.password1});
    if (result){
      this.error = null;
      this.userMutationStore.clear();
    }
    this.loading = false;
    return result;
  }
  handleUpdateUser = async (id:number) => {
    this.error = null;
    if (!this.userMutationStore.validateDraft()){
      this.error = this.userMutationStore.error;
      return false;
    }
    this.loading = true;
    const result = await this.UpdateUser(id, this.userMutationStore.draft);
    if (result){
      this.error = null;
      this.usersData = this.usersData.map(u=>{
        if (u.id === id){
          return {...this.userMutationStore.draft, id}
        }
        return u;
      })
      this.userMutationStore.clear();
    }
    this.loading = false;
    return result;
  }
  handlePasswordChange = async (user:User) =>{
    this.error = null;
    if (!this.userMutationStore.validatePassword()) {
      this.error = this.userMutationStore.error;
      return false;
    }
    this.loading = true;
    const newUser: NewUser = {
      userName: user.userName,
      fullName: user.fullName,
      password: this.userMutationStore.passwordChangeValues.password1,
      enabled:user.enabled,
      isAdmin: user.isAdmin,
    }
    const result = await this.UpdateUser(user.id, newUser);
    if (result) {
      this.error = null;
      this.userMutationStore.clear();
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
  async AddUser(newUser:NewUser) {
    this.error = null;
    try {
      const body = JSON.stringify(newUser);
      const res: string = await addUser(body);
      const id = parseInt(res);
      if (!Number.isNaN(id)) {
        if (!this.usersData.find(value => value.id === id)) {
          const user:User = {...newUser, id:id};
          this.usersData = [...this.usersData, user]
          return true;
        }
      }
      this.error = "Пользователь с таким id уже существует";
      return false;
    }
    catch(error:unknown){
      console.log(error);
      switch (error){
        case 403:
        case 401:
          this.usersData = [];
          rootStore.handleLogout();
          break;
        default:
          this.error = 'Неизвестная ошибка';
          break;
      }
      return false;
    }
  }
  async UpdateUser (id:number,user:NewUser|DraftUser ) {
    this.error = null;
    try{
      const body = JSON.stringify({...user, id});
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
          this.error = 'Пользователь с таким логином уже существует';
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

export const userStore = new UserStore();