import './App.css'
import {configure} from "mobx";
import {observer} from "mobx-react";
import {rootStore} from "@/store/root-store.ts";
import {LoginPage} from "@/pages/LoginPage.tsx";
import {DefaultPage} from "@/pages/DefaultPage.tsx";
import {ThemeProvider} from "@/components/theme-provider.tsx";

configure({
  enforceActions: 'never',
});
export  const  App = observer(()=> {
  return (
    <ThemeProvider theme={rootStore.theme}>
    <section className="h-screen w-screen">
      {rootStore.isLoggedIn ?(
        <DefaultPage/>
        ):(
      <LoginPage/>
      )}
    </section>
    </ThemeProvider>
  )
});


