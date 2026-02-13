import {observer} from "mobx-react";
import {Field, FieldGroup} from "@/components/ui/field.tsx";
import {Label} from "@/components/ui/label.tsx";
import {Input} from "@/components/ui/input.tsx";
import {userStore} from "@/content/users/user-store.ts";
import type {FilterRule} from "@/content/filter-rules/filter-rules-columns.tsx";
import {FilterRuleTypeDialog} from "@/content/filter-rules/components/filter-rule-type-dialog.tsx";

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
          value={userStore.userMutationStore.draft.userName}
          onChange={userStore.userMutationStore.handleDraftUserNameChangeValue}
        />
      </Field>
      <Field>
        <Label htmlFor="comment">Комментарий</Label>
        <Input
          id="comment"
          type="text"
          value={userStore.userMutationStore.draft.fullName}
          onChange={userStore.userMutationStore.handleDraftFullNameChangeValue}
        />
      </Field>
      {!filterRule && <FilterRuleTypeDialog/>}
    </FieldGroup>
  );
});