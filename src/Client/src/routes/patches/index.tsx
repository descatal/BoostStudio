import { createFileRoute, Navigate } from "@tanstack/react-router";
import { PatchFileTabs } from "@/pages/patches/libs/store";
import { PatchFileVersion } from "@/api/exvs";

export const Route = createFileRoute("/patches/")({
  component: RouteComponent,
});

function RouteComponent() {
  const { patchId }: { patchId: PatchFileTabs } = Route.useParams();
  return (
    <Navigate
      to="/patches/$patchId"
      params={{ patchId: patchId ?? PatchFileVersion.Patch6 }}
    />
  );
}
