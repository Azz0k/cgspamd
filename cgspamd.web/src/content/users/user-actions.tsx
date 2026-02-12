import {observer} from "mobx-react";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger
} from "@/components/ui/dropdown-menu.tsx";
import {Button} from "@/components/ui/button.tsx";
import {MoreHorizontal} from "lucide-react";
import {DeleteConfirmationDialog} from "@/components/delete-confirmation-dialog.tsx";
import React from "react";
import {userState} from "@/content/users/user-state.ts";

type UserActionsProps = {
  id:number;
}
export const UserActions = observer(({id}:UserActionsProps)=>{
  const [open, SetOpen] = React.useState(false);
  return (
    <DropdownMenu open={open} onOpenChange={SetOpen}>
      <DropdownMenuTrigger asChild>
        <Button variant="ghost" className="h-8 w-8 p-0">
          <span className="sr-only">Open menu</span>
          <MoreHorizontal className="h-4 w-4" />
        </Button>
      </DropdownMenuTrigger>
      <DropdownMenuContent align="end">
        <DropdownMenuItem>Добавить пользователя</DropdownMenuItem>
        <DropdownMenuItem>Редактировать пользователя</DropdownMenuItem>
        <DeleteConfirmationDialog
          onCancel={()=>SetOpen(false)}
          onConfirm={()=>userState.handleDeleteUser(id)}
          error={userState.error}
          menuItemText="Удалить пользователя"
          loading={userState.loading}
        />
        <DropdownMenuItem>Сменить пароль пользователю</DropdownMenuItem>
      </DropdownMenuContent>
    </DropdownMenu>
  )
});