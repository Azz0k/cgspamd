import { Field, FieldGroup } from "@/components/ui/field";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import {observer} from "mobx-react";
import {userState} from "@/content/users/user-state.ts";

export const ChangePasswordDialog = observer(() => {
  return (
    <FieldGroup>
      <Field>
        <Label htmlFor="password-1">Пароль</Label>
        <Input
          id="password-1"
          type="password"
          value={userState.passwordChangeValues.password1}
          onChange={userState.handlePassword1ChangeValue}
          />
      </Field>
      <Field>
        <Label htmlFor="password-2">Подтверждение пароля</Label>
        <Input
          id="password-2"
          type="password"
          value={userState.passwordChangeValues.password2}
          onChange={userState.handlePassword2ChangeValue}
        />
      </Field>
    </FieldGroup>
  );
})