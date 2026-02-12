import {userColumns} from "@/content/users/user-columns.tsx";
import {DataTable} from "@/components/data-table.tsx";
import {observer} from "mobx-react";
import {userState} from "@/content/users/user-state.ts";
import {rootStore} from "@/store/root-store.ts";
import {useEffect} from "react";
import {reaction} from "mobx";

export const UsersContent = observer(() => {
  useEffect(()=>{
    return   reaction(
      ()=>rootStore.isLoggedIn,
      ()=>{
        if (rootStore.isLoggedIn){
          userState.LoadAllUsers().then();
        }
      },
      { fireImmediately: true }
    );
  },[]);
  return (
    <section className="container mx-auto py-10">
      <DataTable columns={userColumns} data={userState.usersData}/>
    </section>
  );
});