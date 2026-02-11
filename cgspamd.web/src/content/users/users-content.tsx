import {userColumns} from "@/content/users/user-columns.tsx";
import {DataTable} from "@/content/data-table.tsx";
import {observer} from "mobx-react";
import {userState} from "@/content/users/UserState.ts";
import {rootStore} from "@/store/RootStore.ts";
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
      <DataTable columns={userColumns} data={userState.usersData} />
    </section>
  );
});