import { createFileRoute } from "@tanstack/react-router";
import ScriptCompiler from "@/features/scripts/components/script-compiler";

export const Route = createFileRoute("/units/$unitId/scripts")({
  component: RouteComponent,
});

function RouteComponent() {
  const { unitId }: { unitId: number } = Route.useParams();
  return <ScriptCompiler unitId={Number(unitId)}></ScriptCompiler>;
}
