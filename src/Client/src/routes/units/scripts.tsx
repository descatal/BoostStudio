import { createFileRoute } from "@tanstack/react-router";
import ScriptCompiler from "@/features/scripts/components/script-compiler";

export const Route = createFileRoute("/units/scripts")({
  component: RouteComponent,
});

function RouteComponent() {
  return <ScriptCompiler unitId={1011} />;
}
