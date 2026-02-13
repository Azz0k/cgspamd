import { Field, FieldGroup } from "@/components/ui/field";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import {observer} from "mobx-react";
import {userStore} from "@/content/users/user-store.ts";
import {Checkbox} from "@/components/ui/checkbox";
import {ChangePasswordDialog} from "@/content/users/components/change-password-dialog.tsx";
import {useEffect} from "react";
import type {User} from "@/content/users/user-columns.tsx";
import {defaultDraftUser} from "@/content/users/user-mutation-store.ts";
type EditUserDialogProps = {
  user?: User;
}
export const EditUserDialog = observer(({user}:EditUserDialogProps)=>{
  useEffect(()=>{
    userStore.userMutationStore.createNewDraft({
    userName:user?.userName ?? defaultDraftUser.userName,
    fullName:user?.fullName ?? defaultDraftUser.fullName,
    enabled:user?.enabled ?? defaultDraftUser.enabled,
    isAdmin:user?.isAdmin ?? defaultDraftUser.isAdmin
    });
  },[])
  return (
    <FieldGroup>
      <Field>
        <Label htmlFor="userName">Логин</Label>
        <Input
          id="userName"
          type="text"
          value={userStore.userMutationStore.draft.userName}
          onChange={userStore.userMutationStore.handleDraftUserNameChangeValue}
        />
      </Field>
      <Field>
        <Label htmlFor="fullName">ФИО</Label>
        <Input
          id="fullName"
          type="text"
          value={userStore.userMutationStore.draft.fullName}
          onChange={userStore.userMutationStore.handleDraftFullNameChangeValue}
        />
      </Field>
      <Field orientation="horizontal">
        <Checkbox
          id="isAdmin"
          checked={userStore.userMutationStore.draft.isAdmin}
          onCheckedChange={userStore.userMutationStore.handleDraftIsAdminCheckedChange}
        />
        <Label htmlFor="isAdmin">Администратор</Label>
      </Field>
      <Field orientation="horizontal">
        <Checkbox
          id="enabled"
          checked={userStore.userMutationStore.draft.enabled}
          onCheckedChange={userStore.userMutationStore.handleDraftEnabledCheckedChange}
        />
        <Label htmlFor="enabled">Включен</Label>
      </Field>
      {!user && <ChangePasswordDialog/>}
    </FieldGroup>
  );
});