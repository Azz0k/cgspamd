import {makeAutoObservable} from "mobx";
import type {User} from "@/content/users/user-columns.tsx";
import {loadAllUsers} from "@/services/users.api.ts";
import {rootStore} from "@/store/RootStore.ts";

type Users = User[];
class UserState {
  constructor(){
    makeAutoObservable(this);
  }
  usersData: Users = [];
  loading: boolean = false;

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
}

export const userState = new UserState();