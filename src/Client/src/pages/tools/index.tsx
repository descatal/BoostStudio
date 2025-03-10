﻿import * as React from "react"
import { IconBrowserCheck, IconUser } from "@tabler/icons-react"
import { Link, Outlet, useLocation } from "react-router-dom"

import { cn } from "@/lib/utils"

const sidebarNavItems = [
  {
    title: "Assets",
    icon: <IconUser size={18} />,
    href: "/tools/assets",
  },
  {
    title: "Psarc",
    icon: <IconBrowserCheck size={18} />,
    href: "/tools/psarc",
  },
  {
    title: "Scripts",
    icon: <IconBrowserCheck size={18} />,
    href: "/tools/scripts",
  },
]

export default function Tools() {
  const { pathname } = useLocation()

  return (
    <div className="flex min-h-screen w-full flex-col">
      <main className="flex min-h-[calc(100vh_-_theme(spacing.16))] flex-1 flex-col gap-4 bg-muted/40 p-4 md:gap-8 md:p-10">
        <div className="mx-auto grid w-full max-w-6xl gap-2">
          <h1 className="text-3xl font-semibold">Tools</h1>
        </div>
        <div className="mx-auto grid w-full max-w-6xl items-start gap-6 md:grid-cols-[180px_1fr] lg:grid-cols-[250px_1fr]">
          <nav className="grid gap-4 text-sm text-muted-foreground">
            {sidebarNavItems.map((item) => (
              <Link
                key={item.href}
                to={item.href}
                className={cn(
                  pathname === item.href ? "font-semibold text-primary" : ""
                )}
              >
                {item.title}
              </Link>
            ))}
          </nav>
          <Outlet />
        </div>
      </main>
    </div>
  )
}
