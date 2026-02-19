import {observer} from "mobx-react";
import {Field, FieldGroup} from "@/components/ui/field.tsx";
import {filterRuleStore} from "@/content/filter-rules/filter-rule-store.ts";
import {FilterRuleTypeDialog} from "@/content/filter-rules/components/filter-rule-type-dialog.tsx";
import {Textarea} from "@/components/ui/textarea.tsx";
import {Label} from "@/components/ui/label.tsx";
import {
  HoverCard,
    HoverCardContent,
    HoverCardTrigger,
} from "@/components/ui/hover-card";
import { CircleQuestionMarkIcon } from "lucide-react";
import {ScrollArea} from "@/components/ui/scroll-area.tsx";

export const FilterRuleImportDialog = observer(()=>{
  return (
    <FieldGroup>
      <FilterRuleTypeDialog/>
      <Field>
        <HoverCard>
          <HoverCardTrigger>
            <div className="flex justify-between">
              <div>
                Правила сюда:
              </div>
              <div className="">
                <CircleQuestionMarkIcon/>
              </div>
            </div>
          </HoverCardTrigger>
          <HoverCardContent side="right">
            Комментарии отделяйте от правила c помощью решетки #. Если в строке нет комментария - будет добавлен комментарий по умолчанию.
          </HoverCardContent>
        </HoverCard>
        <Label htmlFor="importarea"></Label>
        <ScrollArea className="h-72 rounded-md">
          <Textarea
            className="h-72 resize-none rounded-md"
            id="importarea"
            value={filterRuleStore.filterRulesMutationStore.importArea}
            onChange={filterRuleStore.filterRulesMutationStore.handleImportAreaChange}
          />
        </ScrollArea>
      </Field>
    </FieldGroup>
  );
});