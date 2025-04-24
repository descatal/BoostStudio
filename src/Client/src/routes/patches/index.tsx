import { createFileRoute, Navigate } from "@tanstack/react-router";
import { PatchFileTabs } from "@/features/patches/libs/constants";

export const Route = createFileRoute("/patches/")({
  component: RouteComponent,
});

function RouteComponent() {
  const { patchId }: { patchId: PatchFileTabs } = Route.useParams();
  return (
    <Navigate
      to="/patches/$patchId"
      params={{ patchId: patchId ?? PatchFileTabs.Patch6 }}
      replace
    />
  );
}
