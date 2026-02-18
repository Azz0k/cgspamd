import {observer} from "mobx-react";
import {Field, FieldGroup} from "@/components/ui/field.tsx";
import {Label} from "@/components/ui/label.tsx";
import {Input} from "@/components/ui/input.tsx";
import type {FilterRule} from "@/content/filter-rules/filter-rules-columns.tsx";
import {FilterRuleTypeDialog} from "@/content/filter-rules/components/filter-rule-type-dialog.tsx";
import {filterRuleStore} from "@/content/filter-rules/filter-rule-store.ts";

type FilterRuleEditDialogProps = {
  filterRule?: FilterRule;
}
export const FilterRuleEditDialog = observer(({filterRule}:FilterRuleEditDialogProps)=>{
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