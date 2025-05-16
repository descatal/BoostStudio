import { createFileRoute, Outlet } from "@tanstack/react-router";
import { ToolsSections } from "@/lib/constants";
import { useEffect } from "react";
import { useAppContext } from "@/providers/app-store-provider";

export const Route = createFileRoute("/tools")({
  component: RouteComponent,
});

function RouteComponent() {
  const setTopbarLinks = useAppContext((state) => state.setTopbarLinks);
  const links = Object.entries(ToolsSections).map(([label, path]) => ({
    label: label,
    path: `/tools/${path}`,
  }));

  useEffect(() => {
    setTopbarLinks(links);
  }, []);

  return (
    <>
      <div className="flex-col md:flex">
        <div className="flex-1 space-y-4 p-8 pt-6">
          <Outlet />
        </div>
      </div>
    </>
  );
}
