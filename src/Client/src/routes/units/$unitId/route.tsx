import { createFileRoute, Link, Outlet } from "@tanstack/react-router";
import { Search } from "@/components/search";
import { ThemeSwitch } from "@/components/theme-switch";
import { Header } from "@/components/layout/header";
import React from "react";
import { UnitCustomizableSections } from "@/lib/constants";

export const Route = createFileRoute("/units/$unitId")({
  component: RouteComponent,
});

function RouteComponent() {
  const { unitId }: { unitId: number } = Route.useParams();

  return (
    <>
      {/* ===== Top Heading ===== */}
      <Header>
        {Object.entries(UnitCustomizableSections).map(([label, path]) => (
          <Link
            key={path}
            className={`text-muted-foreground text-sm font-medium transition-colors hover:text-primary`}
            to={`/units/$unitId/${path}`}
            activeProps={{ className: "text-primary" }}
            params={{
              unitId: unitId.toString(),
            }}
          >
            {label}
          </Link>
        ))}
        <div className="ml-auto flex items-center space-x-4">
          <Search />
          <ThemeSwitch />
        </div>
      </Header>
      <Outlet />
    </>
  );
}
