import {observer} from "mobx-react";
import {LoginForm} from "@/components/login-form.tsx";

export const LoginPage = observer(()=>{
  return (
    <div className="flex min-h-svh w-full items-center justify-center p-6 md:p-10">
      <div className="w-full max-w-sm">
        <LoginForm />
      </div>
    </div>
  );
});