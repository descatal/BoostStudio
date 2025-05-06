import { createFileRoute, Outlet } from "@tanstack/react-router";
import { useAppContext } from "@/providers/app-store-provider";
import { UnitCustomizableSections } from "@/lib/constants";
import React, { useEffect } from "react";

export const Route = createFileRoute("/units")({
  component: RouteComponent,
});

function RouteComponent() {
  const { unitId }: { unitId: number } = Route.useParams();

  const setTopbarLinks = useAppContext((state) => state.setTopbarLinks);

  useEffect(() => {
    const links = Object.entries(UnitCustomizableSections).map(
      ([label, path]) => ({
        label: label,
        path: unitId ? `/units/${unitId}/${path}` : `/units/${path}`,
      }),
    );

    setTopbarLinks(links);
  }, [unitId]);

  return (
    <>
      <div className="flex-col space-y-4 p-8 pt-6 h-full">
        <Outlet />
      </div>
    </>
  );
}
