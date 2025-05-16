import { createFileRoute, Outlet } from "@tanstack/react-router";
import { useAppContext } from "@/providers/app-store-provider.ts";
import { useEffect } from "react";

export const Route = createFileRoute("/settings")({
  component: RouteComponent,
});

function RouteComponent() {
  const setTopbarLinks = useAppContext((state) => state.setTopbarLinks);

  useEffect(() => {
    setTopbarLinks([]);
  }, []);

  return <Outlet />;
}
