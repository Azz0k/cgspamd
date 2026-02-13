import {observer} from "mobx-react";
import type {FilterRule} from "@/content/filter-rules/filter-rules-columns.tsx";
import React from "react";
import {DropdownMenu, DropdownMenuContent, DropdownMenuTrigger} from "@/components/ui/dropdown-menu.tsx";
import {Button} from "@/components/ui/button.tsx";
import {MoreHorizontal} from "lucide-react";
import {ActionConfirmationDialog} from "@/components/action-confirmation-dialog.tsx";
import {filterRuleStore} from "@/content/filter-rules/filter-rule-store.ts";
import {FilterRuleEditDialog} from "@/content/filter-rules/components/filter-rule-edit-dialog.tsx";

type FilterRuleActionsProps = {
  filterRule: FilterRule;
}
export const FilterRuleActions = observer(({filterRule}:FilterRuleActionsProps)=> {
  const [open, SetOpen] = React.useState(false);
  const cancelAction  = ()=>{
    filterRuleStore.handleCancelAction();
    SetOpen(false);
  };
  return (
    <DropdownMenu open={open} onOpenChange={SetOpen}>
      <DropdownMenuTrigger asChild>
        <Button variant="ghost" className="h-8 w-8 p-0">
          <span className="sr-only">Open menu</span>
          <MoreHorizontal className="h-4 w-4" />
        </Button>
      </DropdownMenuTrigger>
      <DropdownMenuContent align="end">
        <ActionConfirmationDialog
          onCancel={cancelAction}
          onConfirm={()=>filterRuleStore.handleAddRule()}
          error={filterRuleStore.error}
          menuItemText="Добавить правило"
          loading={filterRuleStore.loading}
          description="Введите данные нового правила"
          title={`Новое правило`}
        >
          <FilterRuleEditDialog/>
        </ActionConfirmationDialog>
        <ActionConfirmationDialog
          onCancel={cancelAction}
          onConfirm={()=>filterRuleStore.handleUpdateRule(filterRule.id)}
          error={filterRuleStore.error}
          menuItemText="Редактировать правило"
          loading={filterRuleStore.loading}
          description="Введите новые данные правила"
          title={`Редактируем правило`}
        >
          <FilterRuleEditDialog filterRule={filterRule}/>
        </ActionConfirmationDialog>
        <ActionConfirmationDialog
          onCancel={cancelAction}
          onConfirm={()=>filterRuleStore.handleDeleteRule(filterRule.id)}
          error={filterRuleStore.error}
          menuItemText="Удалить правило"
          loading={filterRuleStore.loading}
          description="Это действие нельзя отменить."
          title="Вы уверены?"
        >
        </ActionConfirmationDialog>
      </DropdownMenuContent>
    </DropdownMenu>
  );
});