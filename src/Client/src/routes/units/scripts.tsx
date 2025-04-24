import { createFileRoute } from "@tanstack/react-router";
import ScriptViewer from "@/features/scripts/components/script-viewer";

export const Route = createFileRoute("/units/scripts")({
  component: RouteComponent,
});

function RouteComponent() {
  return (
    <>
      <ScriptViewer unitId={1011} />
    </>
  );
}
