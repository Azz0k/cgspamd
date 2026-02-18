import {observer} from "mobx-react";
import {Field, FieldGroup} from "@/components/ui/field.tsx";
import {Label} from "@/components/ui/label.tsx";
import {Input} from "@/components/ui/input.tsx";
import type {FilterRule} from "@/content/filter-rules/filter-rules-columns.tsx";
import {FilterRuleTypeDialog} from "@/content/filter-rules/components/filter-rule-type-dialog.tsx";
import {filterRuleStore} from "@/content/filter-rules/filter-rule-store.ts";
import {useEffect} from "react";
import {defaultDraftFilterRule} from "@/content/filter-rules/filter-rule-mutation-store.ts";

type FilterRuleEditDialogProps = {
  filterRule?: FilterRule;
}
export const FilterRuleEditDialog = observer(({filterRule}:FilterRuleEditDialogProps)=>{
  useEffect(()=>{
    filterRuleStore.filterRulesMutationStore.createNewDraft({
      value: filterRule?.value ?? defaultDraftFilterRule.value,
      comment: filterRule?.comment ?? defaultDraftFilterRule.comment,
      type: filterRule?.type ?? defaultDraftFilterRule.type
    });
  },[])
  return (
    <FieldGroup>
      <Field>
        <Label htmlFor="value">Значение</Label>
        <Input
          id="value"
          type="text"
          value={filterRuleStore.filterRulesMutationStore.draft.value}
          onChange={filterRuleStore.filterRulesMutationStore.handleDraftChangeValue}
        />
      </Field>
      <Field>
        <Label htmlFor="comment">Комментарий</Label>
        <Input
          id="comment"
          type="text"
          value={filterRuleStore.filterRulesMutationStore.draft.comment}
          onChange={filterRuleStore.filterRulesMutationStore.handleDraftChangeComment}
        />
      </Field>
      {!filterRule && <FilterRuleTypeDialog/>}
    </FieldGroup>
  );
});