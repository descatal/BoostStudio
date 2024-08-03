import { cn } from "@/lib/utils"
import React from "react";

export function MainNav({
  className,
  ...props
}: React.HTMLAttributes<HTMLElement>) {
  return (
    <nav
      className={cn("flex items-center space-x-4 lg:space-x-6", className)}
      {...props}
    >
      <a
        href="/units/{id}/info/{tab}"
        className="text-sm font-medium transition-colors hover:text-primary"
      >
        Information
      </a>
      <a
        href="/units/{id}/scripts"
        className="text-sm font-medium text-muted-foreground transition-colors hover:text-primary"
      >
        Script
      </a>
      <a
        href="/units/{id}/models"
        className="text-sm font-medium text-muted-foreground transition-colors hover:text-primary"
      >
        Model & Animation
      </a>
      <a
        href="/units/{id}/misc"
        className="text-sm font-medium text-muted-foreground transition-colors hover:text-primary"
      >
        Misc
      </a>
    </nav>
  )
}
