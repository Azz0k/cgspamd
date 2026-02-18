import {makeAutoObservable} from "mobx";
import type {FilterRule} from "@/content/filter-rules/filter-rules-columns.tsx";
import {rootStore} from "@/store/root-store.ts";
import {addRule, deleteRule, loadAllRules, updateRule} from "@/services/fillter-rules.api.ts";
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

  handleLogout = ()=>{
    this.filterRulesData = [];
    this.error = null;
    rootStore.handleLogout();
  }
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
    const result = await this.AddFilterRule(this.filterRulesMutationStore.draft);
    if (result){
      this.error = null;
      this.filterRulesMutationStore.clear();
    }
    return result;
  }
  handleUpdateRule = async (id:number) => {
    if(!this.filterRulesMutationStore.validate()){
      this.error = this.filterRulesMutationStore.error;
      return false;
    }
    const result = await this.UpdateFilterRule(id, this.filterRulesMutationStore.draft);
    if (result){
      this.filterRulesMutationStore.clear();
    }
    return result;
  }
  handleDeleteRule = async (id:number) => {
    const result = await this.DeleteRule(id);
    if (result){
      this.filterRulesData = this.filterRulesData.filter(rule => rule.id !== id);
      this.filterRulesMutationStore.clear();
    }
    return result;
  }
  async LoadAllRules(){
    this.loading = true;
    try{
      this.filterRulesData = await loadAllRules() as FilterRules;
    }
    catch(error:unknown){
      this.handleApiError(error);
      }
    finally{
      this.loading = false;
    }
  }
  async AddFilterRule(rule:DraftFilterRule){
    this.error = null;
    this.loading = true;
    const body = JSON.stringify(rule);
    try {
      const res = await addRule(body) as FilterRule;
      this.filterRulesData = [...this.filterRulesData, res];
      return true;
      }
    catch(error:unknown) {
      this.handleApiError(error);
      return false;
    }
    finally{
      this.loading = false;
    }
  }
  async UpdateFilterRule(id:number, rule:DraftFilterRule){
    this.error = null;
    this.loading = true;
    const body = JSON.stringify({...rule, id:id});
    try {
      const res = await updateRule(body) as FilterRule;
      this.filterRulesData = this.filterRulesData.map(rule=>{
        if (rule.id === res.id){
          return res;
        }
        return rule;
      })
      return true;
    }
    catch(error:unknown) {
      this.handleApiError(error);
      return false;
    }
    finally{
      this.loading = false;
    }
  }
  async DeleteRule(ruleId:number){
    this.error = null;
    this.loading = true;
    try{

      await deleteRule(ruleId);
    }
    catch (error:unknown) {
      this.handleApiError(error);
      return false;
    }
    finally{
      this.loading = false;
    }
    return true;
  }
  handleApiError = (error:unknown) => {
    switch (error){
      case 403:
      case 401:
        this.handleLogout();
        break;
      case 404:
        this.error = "Такого правила не существует. Обновите страницу";
        break;
      case 409:
        this.error = "Такое правило уже существует";
        break;
      default:
        this.error = 'Неизвестная ошибка';
        break;
    }
  }
}

export const filterRuleStore = new FilterRuleStore();