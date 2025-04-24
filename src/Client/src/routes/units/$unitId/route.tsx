import { createFileRoute, Outlet } from "@tanstack/react-router";
import React, { useEffect } from "react";
import { UnitCustomizableSections } from "@/lib/constants";
import { useAppContext } from "@/providers/app-store-provider";

export const Route = createFileRoute("/units/$unitId")({
  component: RouteComponent,
});

function RouteComponent() {
  const { unitId }: { unitId: number } = Route.useParams();

  const setTopbarLinks = useAppContext((state) => state.setTopbarLinks);
  const links = Object.entries(UnitCustomizableSections).map(
    ([label, path]) => ({
      label: label,
      path: `/units/${unitId}/${path}`,
    }),
  );

  useEffect(() => {
    setTopbarLinks(links);
  }, []);

  return <Outlet />;
}
