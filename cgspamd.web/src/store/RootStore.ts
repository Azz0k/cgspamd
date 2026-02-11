import {makeAutoObservable} from "mobx";
import {Authenticate} from "../services/Authenticate.api.ts";
import * as React from "react";
import type { JWTPayloadConverted} from "@/interfaces/JWT-payload.ts";
import {convertJWTPayload} from "@/lib/utils.ts";
import { router } from "@/routes/router.tsx";
import {mainMenu} from "@/store/main-menu.ts";

const themeKey = "vite-ui-theme";

class RootStore {
  constructor() {
    makeAutoObservable(this);
    this.token = localStorage.getItem("token");
    if (this.token) {
      this.CurrentUser = convertJWTPayload(this.token);
    }
    const theme = localStorage.getItem(themeKey);
    if (theme) {
      if (theme === "light") {
        this.themeSwitchValue = false;
      }
    }
    router.subscribe('onResolved', (evt)=>{
      this.pathName = evt.toLocation.pathname;
    });
  }
  pathName!: string;
  CurrentUser:JWTPayloadConverted | null = null;
  themeSwitchValue = true;
  login:string = "";
  password:string = "";
  token:string |null = null;
  get theme() {
    return this.themeSwitchValue? "dark" : "light";
  }
  get mainMenu() {
    return rootStore.CurrentUser?.IsAdmin?mainMenu.navAdmin:mainMenu.navMain;
  }
  get mainMenuTitle() {
    return this.mainMenu.find(e=>e.url===this.pathName)?.title;
  }
  handleCheckTheme = (value:boolean) => {
    this.themeSwitchValue = value;
    localStorage.setItem(themeKey, this.theme);
  }
  handleLoginChange = (e:React.ChangeEvent<HTMLInputElement>) =>{
    this.login = e.target.value;
  }
  handlePasswordChange = (e:React.ChangeEvent<HTMLInputElement>) =>{
    this.password = e.target.value;
  }
  handleLogin = (e:React.SubmitEvent<HTMLFormElement>) =>{
    e.preventDefault();
    this.Authenticate().then();
  }
  get isLoggedIn(): boolean {
    return this.token !== null;
  }

  handleLogout = ()=>  {
    localStorage.removeItem("token");
    this.token = null;
    this.CurrentUser = null;
  }
  async Authenticate(): Promise<void> {
    try{
      const result= await Authenticate(JSON.stringify({
        Login: this.login,
        Password: this.password,
      }));
      const token=result.token;
      if(token){
        this.token = token;
        localStorage.setItem("token", token);
        this.login = "";
        this.password = "";
        this.CurrentUser = convertJWTPayload(token);
      }
    }
    catch(error:unknown){
      switch (error){
        default:
          console.log(error);
          break;
      }
    }
  }
}

export const rootStore= new RootStore();