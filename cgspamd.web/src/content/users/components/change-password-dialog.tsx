import { Field, FieldGroup } from "@/components/ui/field";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import {observer} from "mobx-react";
import {userStore} from "@/content/users/user-store.ts";

export const ChangePasswordDialog = observer(() => {
  return (
    <FieldGroup>
      <Field>
        <Label htmlFor="password-1">Пароль</Label>
        <Input
          id="password-1"
          type="password"
          value={userStore.userMutationStore.passwordChangeValues.password1}
          onChange={userStore.userMutationStore.handlePassword1ChangeValue}
          />
      </Field>
      <Field>
        <Label htmlFor="password-2">Подтверждение пароля</Label>
        <Input
          id="password-2"
          type="password"
          value={userStore.userMutationStore.passwordChangeValues.password2}
          onChange={userStore.userMutationStore.handlePassword2ChangeValue}
        />
      </Field>
    </FieldGroup>
  );
})