import {observer} from "mobx-react";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger
} from "@/components/ui/dropdown-menu.tsx";
import {Button} from "@/components/ui/button.tsx";
import {MoreHorizontal} from "lucide-react";
import React from "react";
import {userState} from "@/content/users/user-state.ts";
import {ActionConfirmationDialog} from "@/components/action-confirmation-dialog.tsx";
import type {User} from "@/content/users/user-columns.tsx";
import {ChangePasswordDialog} from "@/content/users/components/change-password-dialog.tsx";

type UserActionsProps = {
  user:User;
}
export const UserActions = observer(({user}:UserActionsProps)=>{
  const [open, SetOpen] = React.useState(false);
  const cancelAction  = ()=>{
    userState.handleCancelAction();
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
        <DropdownMenuItem>Добавить пользователя</DropdownMenuItem>
        <DropdownMenuItem>Редактировать пользователя</DropdownMenuItem>
        <ActionConfirmationDialog
          onCancel={cancelAction}
          onConfirm={()=>userState.handleDeleteUser(user.id)}
          error={userState.error}
          menuItemText="Удалить пользователя"
          loading={userState.loading}
          description="Это действие нельзя отменить."
          title="Вы уверены?"
        >
        </ActionConfirmationDialog>
        <ActionConfirmationDialog
          onCancel={cancelAction}
          onConfirm={()=>userState.handlePasswordChange(user)}
          error={userState.error}
          menuItemText="Сменить пароль пользователю"
          loading={userState.loading}
          description="Введите новый пароль и подтверждение пароля"
          title={`Смена пароля для ${user.userName}`}
        >
          <ChangePasswordDialog/>
        </ActionConfirmationDialog>

      </DropdownMenuContent>
    </DropdownMenu>
  )
});