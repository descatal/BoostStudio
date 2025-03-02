import React from "react"
import Cookies from "js-cookie"
import { Outlet } from "react-router-dom"

import useIsCollapsed from "@/hooks/use-is-collapsed"

import Sidebar from "./layout/sidebar"

export default function AppShell() {
  const defaultOpen = Cookies.get("sidebar:state") !== "false"

  const [isCollapsed, setIsCollapsed] = useIsCollapsed()
  return (
    <div className="relative h-screen overflow-hidden bg-background">
      <Sidebar isCollapsed={isCollapsed} setIsCollapsed={setIsCollapsed} />
      <main
        id="content"
        className={`
          overflow-x-hidden pb-20 pt-16 transition-[margin] md:pt-0
          ${isCollapsed ? "md:ml-14" : "md:ml-64"} h-full`}
      >
        {/*<SidebarTrigger />*/}
        <Outlet />
      </main>
    </div>
  )
}
