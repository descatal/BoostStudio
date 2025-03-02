import React from "react";
import { useUnitsStore } from "@/pages/units/libs/store";
import { Link, useLocation, useParams } from "@tanstack/react-router";

import { cn } from "@/lib/utils";

interface MainNavProps extends React.HTMLAttributes<HTMLElement> {}

export function CustomizeUnitNav({ className, ...props }: MainNavProps) {
  const params = useParams();
  const unitId = Number(params.unitId);

  const { customizeSection } = useUnitsStore((state) => state);

  return (
    <nav
      className={cn("flex items-center space-x-4 lg:space-x-6", className)}
      {...props}
    >
      <Link
        to={`/units/${unitId}/customize/`}
        className={`${customizeSection === "info" ? "text-primary" : "text-muted-foreground"} text-sm font-medium transition-colors hover:text-primary`}
      >
        Information
      </Link>
      <Link
        to={`/units/${unitId}/customize/script`}
        className={`${customizeSection === "script" ? "text-primary" : "text-muted-foreground"} text-sm font-medium transition-colors hover:text-primary`}
      >
        Script
      </Link>
      <Link
        to={`/units/${unitId}/customize/assets`}
        className={`${customizeSection === "assets" ? "text-primary" : "text-muted-foreground"} text-sm font-medium transition-colors hover:text-primary`}
      >
        Assets
      </Link>
      {/*<a*/}
      {/*  href={`/units/${unitId}/customize/misc`}*/}
      {/*  className="text-sm font-medium text-muted-foreground transition-colors hover:text-primary"*/}
      {/*>*/}
      {/*  Misc*/}
      {/*</a>*/}
    </nav>
  );
}
