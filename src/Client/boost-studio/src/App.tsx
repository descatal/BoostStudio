import React, {useState} from "react"
import {invoke} from "@tauri-apps/api/tauri"

import {Menu} from "@/components/menu"

import {TailwindIndicator} from "./components/tailwind-indicator"
import {ThemeProvider} from "./components/theme-provider"
import DashboardPage from "./pages/dashboard"
import {cn} from "./lib/utils"
import { TooltipProvider } from "./components/ui/tooltip"
import Sidebar from "./components/sidebar"
import { RouterProvider } from "react-router-dom"
import router from "@/router";
import Ammo from "./pages/dashboard/tabs/ammo"
import {AlertDialog, AlertDialogTrigger} from "@/components/ui/alert-dialog";
import {Button} from "@/components/ui/button";

function App() {
  const [greetMsg, setGreetMsg] = useState("")
  const [name, setName] = useState("")

  async function greet() {
    // Learn more about Tauri commands at https://tauri.app/v1/guides/features/command
    setGreetMsg(await invoke("greet", {name}))
  }

  return (
    <ThemeProvider attribute="class" defaultTheme="system" enableSystem>
      <TooltipProvider>
        <div className="h-screen overflow-clip">
          <Menu/>
          <div
            className={cn(
              "h-screen overflow-auto border-t bg-background pb-8",
              // "scrollbar-none"
              "scrollbar scrollbar-track-transparent scrollbar-thumb-accent scrollbar-thumb-rounded-md"
            )}
          >
            <AlertDialog>
              <AlertDialogTrigger asChild>
                <Button variant={"default"} onClick={() => {}}>Save</Button>
              </AlertDialogTrigger>
            </AlertDialog>
          </div>
        </div>
      </TooltipProvider>
      <TailwindIndicator/>
    </ThemeProvider>
  )
}

export default App
