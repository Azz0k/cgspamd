import { AppSidebar } from "@/components/app-sidebar"
import { SiteHeader } from "@/components/site-header"
import {
  SidebarInset,
  SidebarProvider,
} from "@/components/ui/sidebar"
import {observer} from "mobx-react";
import {RouterProvider} from "@tanstack/react-router";
import {router} from "@/routes/router.tsx";

export const DefaultPage = observer(()=>{
  return (
    <div className="[--header-height:calc(--spacing(14))]">
      <SidebarProvider className="flex flex-col">
        <SiteHeader />
        <div className="flex flex-1">
          <AppSidebar />
          <SidebarInset>
            <RouterProvider router={router} />
          </SidebarInset>
        </div>
      </SidebarProvider>
    </div>
  )
});