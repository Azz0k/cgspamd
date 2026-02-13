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

export const FilterRuleTypeDialog = observer(() => {
  const values = filterRuleTypes.map(element=>
    <SelectItem value={element} >{element}</SelectItem>
  )
  return (
    <Select>
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