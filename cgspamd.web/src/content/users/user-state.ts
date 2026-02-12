import {makeAutoObservable} from "mobx";
import type {User} from "@/content/users/user-columns.tsx";
import {deleteUser, loadAllUsers} from "@/services/users.api.ts";
import {rootStore} from "@/store/root-store.ts";

type Users = User[];
class UserState {
  constructor(){
    makeAutoObservable(this);
  }
  usersData: Users = [];
  loading: boolean = false;
  error: string | null = null;

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