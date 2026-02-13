import {makeAutoObservable} from "mobx";
import type {FilterRule} from "@/content/filter-rules/filter-rules-columns.tsx";
import {rootStore} from "@/store/root-store.ts";
import {loadAllRules} from "@/services/fillter-rules.api.ts";

type FilterRules = FilterRule[];
class FilterRuleStore {
  constructor() {
    makeAutoObservable(this);
  }

  filterRulesData: FilterRules = [];
  loading: boolean = false;
  error: string | null = null;

  handleCancelAction = () =>{
    //this.userMutationStore.clear();
    this.error = null;
  }
  handleAddRule = async () => {
  return true;
  }
  handleUpdateRule = async (id:number) => {
    return true;
  }
  handleDeleteRule = async (id:number) => {
    return true;
  }
  async LoadAllRules(){
    this.loading = true;
    try{
      this.filterRulesData = await loadAllRules() as FilterRules;
    }
    catch(error:unknown){
      switch (error){
        case 403:
        case 401:
          this.filterRulesData = [];
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

export const filterRuleStore = new FilterRuleStore();