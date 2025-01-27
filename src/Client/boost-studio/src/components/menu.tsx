"use client"

import {useCallback} from "react"

import {
  Menubar,
  MenubarContent,
  MenubarItem,
  MenubarMenu,
  MenubarSeparator,
  MenubarShortcut,
  MenubarSub,
  MenubarTrigger,
} from "@/components/ui/menubar"

import {AboutDialog} from "./about-dialog"
import {MenuModeToggle} from "./menu-mode-toggle"
import {Dialog, DialogTrigger} from "./ui/dialog"

export function Menu() {
  const closeWindow = useCallback(async () => {
    const {appWindow} = await import("@tauri-apps/plugin-window")

    appWindow.close()
  }, [])

  return (
    <Menubar className="rounded-none border-b border-none pl-2 lg:pl-3">
      <MenubarMenu>
        <div className="inline-flex h-fit w-fit items-center text-cyan-500">
          {/*<Sailboat className="h-5 w-5" />*/}
        </div>
      </MenubarMenu>

      <MenubarMenu>
        <MenubarTrigger className="font-bold">App</MenubarTrigger>
        <Dialog modal={false}>
          <MenubarContent>
            <DialogTrigger asChild>
              <MenubarItem>About App</MenubarItem>
            </DialogTrigger>
            <MenubarSeparator/>
            <MenubarShortcut/>
            <MenubarItem onClick={closeWindow}>
              Quit Boost Studio
            </MenubarItem>
          </MenubarContent>
          <AboutDialog/>
        </Dialog>
      </MenubarMenu>
      <MenubarMenu>
        <MenubarTrigger>View</MenubarTrigger>
        <MenubarContent>
          <MenubarSub>
            <MenubarItem>
              New Window
            </MenubarItem>
          </MenubarSub>
        </MenubarContent>
      </MenubarMenu>
      <MenuModeToggle/>
    </Menubar>
  )
}
