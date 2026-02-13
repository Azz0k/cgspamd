import {makeAutoObservable} from "mobx";
import type {ChangeEvent} from "react";

export type DraftUser = {
  userName: string;
  fullName: string,
  enabled: boolean,
  isAdmin: boolean,
};
export const defaultDraftUser = {
  userName: "",
  fullName: "",
  isAdmin: false,
  enabled: true
};

class UserMutationStore{
  constructor() {
    makeAutoObservable(this);
  }
  error: string | null = null;
  passwordChangeValues = {
    password1: "",
    password2: ""
  }
  draft:DraftUser = defaultDraftUser;
  createNewDraft =(user:DraftUser)=>{
    this.draft = user;
  }
  clear = () =>{
    this.error = null;
    this.draft = defaultDraftUser;
    this.passwordChangeValues.password1 = "";
    this.passwordChangeValues.password2 = "";
  }
  validatePassword = () =>{
    if (this.passwordChangeValues.password1 !== this.passwordChangeValues.password2) {
      this.error = "Пароли не совпадают";
      return false;
    }
    if (this.passwordChangeValues.password1.length<8) {
      this.error = "Пароль слишком короткий";
      return false;
    }
    this.error = null;
    return true;
  }
  handlePassword1ChangeValue = (event: ChangeEvent<HTMLInputElement>) => {
    this.passwordChangeValues.password1 = event.target.value;
  }
  handlePassword2ChangeValue = (event: ChangeEvent<HTMLInputElement>) => {
    this.passwordChangeValues.password2 = event.target.value;
  }
  validateDraft = ()=>{
    this.draft.fullName = this.draft.fullName.trim();
    this.draft.userName = this.draft.userName.trim();
    if (this.draft.fullName.length===0) {
      this.error = "ФИО не должно быть пустым";
      return false;
    }
    if (this.draft.userName.length===0) {
      this.error = "Логин не должен быть пустым";
      return false;
    }
    return true;
  }
  handleDraftUserNameChangeValue = (event: ChangeEvent<HTMLInputElement>) => {
    this.draft.userName = event.target.value;
  }
  handleDraftFullNameChangeValue = (event: ChangeEvent<HTMLInputElement>) => {
    this.draft.fullName = event.target.value;
  }
  handleDraftIsAdminCheckedChange = (value:boolean) => {
    this.draft.isAdmin = value;
  }
  handleDraftEnabledCheckedChange = (value:boolean) => {
    this.draft.enabled = value;
  }
}

export const userMutationStore = new UserMutationStore();