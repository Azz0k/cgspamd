import {observer} from "mobx-react";
import React from "react";
import {filterRuleStore} from "@/content/filter-rules/filter-rule-store.ts";
import {DropdownMenu, DropdownMenuContent, DropdownMenuTrigger} from "@/components/ui/dropdown-menu.tsx";
import {Button} from "@/components/ui/button.tsx";
import {MoreHorizontal} from "lucide-react";
import {ActionConfirmationDialog} from "@/components/action-confirmation-dialog.tsx";
import {FilterRuleEditDialog} from "@/content/filter-rules/components/filter-rule-edit-dialog.tsx";
import {FilterRuleImportDialog} from "@/content/filter-rules/components/filter-rule-import-dialog.tsx";

export const FilterRuleGlobalActions = observer(() => {
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
          onConfirm={filterRuleStore.handleAddRule}
          error={filterRuleStore.error}
          menuItemText="Добавить правило"
          loading={filterRuleStore.loading}
          description="Введите данные нового правила"
          title="Новое правило"
        >
          <FilterRuleEditDialog/>
        </ActionConfirmationDialog>
        <ActionConfirmationDialog
          onCancel={cancelAction}
          onConfirm={filterRuleStore.handleImportRules}
          error={filterRuleStore.error}
          menuItemText="Импорт правил"
          loading={filterRuleStore.loading}
          description="Введите данные построчно"
          title="Импорт"
        >
          <FilterRuleImportDialog/>
        </ActionConfirmationDialog>
      </DropdownMenuContent>
    </DropdownMenu>
  );
});