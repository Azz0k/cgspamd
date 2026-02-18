import {makeAutoObservable} from "mobx";
import type {ChangeEvent} from "react";
import {filterRuleTypes} from "@/content/filter-rules/components/filter-rule-types.ts";

export type DraftFilterRule = {
  value: string;
  comment: string;
  type: number;
};

const defaultDraftFilterRule = {
  value: "",
  comment: "",
  type: 0,
};

class FilterRuleMutationStore  {
  constructor() {
    makeAutoObservable(this);
  }
  error: string | null = null;
  draft:DraftFilterRule = defaultDraftFilterRule;

  get TypeValue(){
    return filterRuleTypes[this.draft.type];
  }
  clear = (): void => {
    this.error = null;
    this.draft = defaultDraftFilterRule;
  };
  validate = (): boolean => {
    this.draft.value = this.draft.value.trim();
    if (this.draft.value === "") {
      this.error = "Значение не должно быть пустым";
      return false;
    }
    return true;
  }
  handleDraftChangeValue = (event: ChangeEvent<HTMLInputElement>)=>{
    this.draft.value = event.target.value;
  }
  handleDraftChangeComment = (event: ChangeEvent<HTMLInputElement>)=>{
    this.draft.comment = event.target.value;
  }
  handleDraftChangeType = (value:string)=>{
    filterRuleTypes.forEach((ruleType, index)=>{
      if (ruleType === value) {
        this.draft.type = index;
      }
    })
  }
}

export const filterRuleMutationStore = new FilterRuleMutationStore();