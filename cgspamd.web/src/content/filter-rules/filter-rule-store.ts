import {makeAutoObservable} from "mobx";
import type {FilterRule} from "@/content/filter-rules/filter-rules-columns.tsx";
import {rootStore} from "@/store/root-store.ts";
import {addRule, loadAllRules} from "@/services/fillter-rules.api.ts";
import {type DraftFilterRule, filterRuleMutationStore} from "@/content/filter-rules/filter-rule-mutation-store.ts";

type FilterRules = FilterRule[];
class FilterRuleStore {
  constructor() {
    makeAutoObservable(this);
  }

  filterRulesData: FilterRules = [];
  loading: boolean = false;
  error: string | null = null;
  filterRulesMutationStore = filterRuleMutationStore;

  handleCancelAction = () =>{
    this.filterRulesMutationStore.clear();
    this.error = null;
  }
  handleAddRule = async () => {
    this.error = null;
    if (!this.filterRulesMutationStore.validate()){
      this.error = this.filterRulesMutationStore.error;
      return false;
    }
    this.loading = true;
    const result = await this.AddFilterRule(this.filterRulesMutationStore.draft);
    if (result){
      this.error = null;
      this.filterRulesMutationStore.clear();
    }
    this.loading = false;
    return result;
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
          this.error = null;
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
  async AddFilterRule(rule:DraftFilterRule){
    this.error = null;
    try {
      const body = JSON.stringify(rule);
      const res = await addRule(body) as FilterRule;
      this.filterRulesData = [...this.filterRulesData, res];
      return true
      }
    catch(error:unknown) {
      switch (error){
        case 403:
        case 401:
          this.filterRulesData = [];
          this.error = null;
          rootStore.handleLogout();
          break;
        case 409:
          this.error = "Такое правило уже существует";
          break;
        default:
          this.error = 'Неизвестная ошибка';
          break;
      }
      return false;
    }
  }
}

export const filterRuleStore = new FilterRuleStore();