import * as React from "react"
import { NavMain } from "@/components/nav-main"
import { NavUser } from "@/components/nav-user"
import {
  Sidebar,
  SidebarContent,
  SidebarFooter,
} from "@/components/ui/sidebar"
import {rootStore} from "@/store/root-store.ts";
import {observer} from "mobx-react";

export const AppSidebar = observer(({ ...props }: React.ComponentProps<typeof Sidebar>)=> {
  return (
    <Sidebar
      className="top-(--header-height) h-[calc(100svh-var(--header-height))]!"
      {...props}
    >
      <SidebarContent>
        <NavMain items={rootStore.mainMenu} />
      </SidebarContent>
      <SidebarFooter>
        <NavUser/>
      </SidebarFooter>
    </Sidebar>
  )
});
