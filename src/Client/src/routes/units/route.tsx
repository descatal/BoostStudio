import { createFileRoute, Outlet } from "@tanstack/react-router";
import { useAppContext } from "@/providers/app-store-provider";
import { UnitCustomizableSections } from "@/lib/constants";
import { useEffect } from "react";

export const Route = createFileRoute("/units")({
  component: RouteComponent,
});

function RouteComponent() {
  const setTopbarLinks = useAppContext((state) => state.setTopbarLinks);
  const links = Object.entries(UnitCustomizableSections).map(
    ([label, path]) => ({
      label: label,
      path: `/units/${path}`,
    }),
  );

  useEffect(() => {
    setTopbarLinks(links);
  }, []);

  return <Outlet />;
}
