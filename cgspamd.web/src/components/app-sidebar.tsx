import * as React from "react"
import {
  MailX,
  UserCog
} from "lucide-react"

import { NavMain } from "@/components/nav-main"
import { NavUser } from "@/components/nav-user"
import {
  Sidebar,
  SidebarContent,
  SidebarFooter,
} from "@/components/ui/sidebar"
import {rootStore} from "@/store/RootStore.ts";
import {observer} from "mobx-react";

const data = {
  navAdmin: [
    {
      title: "Правила фильтрации",
      url: "/",
      icon: MailX,
      isActive: true,
    },
    {
      title: "Пользователи",
      url: "/users",
      icon: UserCog,
    }
  ],
  navMain: [
    {
      title: "Правила фильтрации",
      url: "/",
      icon: MailX,
      isActive: true,
    },
  ],
}

export const AppSidebar = observer(({ ...props }: React.ComponentProps<typeof Sidebar>)=> {
  return (
    <Sidebar
      className="top-(--header-height) h-[calc(100svh-var(--header-height))]!"
      {...props}
    >
      <SidebarContent>
        <NavMain items={rootStore.CurrentUser?.IsAdmin?data.navAdmin:data.navMain} />
      </SidebarContent>
      <SidebarFooter>
        <NavUser/>
      </SidebarFooter>
    </Sidebar>
  )
});
