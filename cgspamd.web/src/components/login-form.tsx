import { cn } from "@/lib/utils"
import { Button } from "@/components/ui/button"
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"
import {
  Field,
  FieldGroup,
  FieldLabel,
} from "@/components/ui/field"
import { Input } from "@/components/ui/input"
import * as React from "react";
import {type RefObject, useEffect, useRef } from "react";
import {observer} from "mobx-react";
import { rootStore } from "@/store/root-store.ts";

export const LoginForm = observer(({className,...props}: React.ComponentProps<"div">)=>{
  const inputRef:RefObject<HTMLInputElement| null>  = useRef(null);
  useEffect(() => {
    inputRef.current?.focus();
  },[])
  return (
    <div className={cn("flex flex-col gap-6", className)} {...props}>
      <Card>
        <CardHeader>
          <CardTitle>Вход в настройки cgspamd</CardTitle>
          <CardDescription>
            Введите имя пользователя для входа
          </CardDescription>
        </CardHeader>
        <CardContent>
          <form onSubmit={rootStore.handleLogin}>
            <FieldGroup>
              <Field>
                <FieldLabel htmlFor="username">Имя пользователя</FieldLabel>
                <Input
                  value={rootStore.login}
                  ref={inputRef}
                  id="username"
                  type="text"
                  required
                  onChange={rootStore.handleLoginChange}
                />
              </Field>
              <Field>
                <div className="flex items-center">
                  <FieldLabel htmlFor="password">Пароль</FieldLabel>
                </div>
                <Input
                  value = {rootStore.password}
                  id="password"
                  type="password"
                  required
                  onChange={rootStore.handlePasswordChange}
                />
              </Field>
              <Field>
                <Button>Вход</Button>
              </Field>
            </FieldGroup>
          </form>
        </CardContent>
      </Card>
    </div>
  )
});
