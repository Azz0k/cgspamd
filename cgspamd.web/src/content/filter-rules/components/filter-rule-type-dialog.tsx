import {observer} from "mobx-react";
import {
  Select,
  SelectContent,
  SelectGroup,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import {filterRuleTypes} from "@/content/filter-rules/components/filter-rule-types.ts";
import {filterRuleStore} from "@/content/filter-rules/filter-rule-store.ts";

export const FilterRuleTypeDialog = observer(() => {
  const values = filterRuleTypes.map(element=>
    <SelectItem value={element}>{element}</SelectItem>
  )
  return (
    <Select
      value={filterRuleStore.filterRulesMutationStore.TypeValue}
      onValueChange={filterRuleStore.filterRulesMutationStore.handleDraftChangeType}
    >
      <SelectTrigger className="w-full">
        <SelectValue placeholder="Выберите тип" />
      </SelectTrigger>
      <SelectContent>
        <SelectGroup>
          {values}
        </SelectGroup>
      </SelectContent>
    </Select>
  );
});