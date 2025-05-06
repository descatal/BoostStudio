import { createFileRoute } from "@tanstack/react-router";
import HitboxesList from "@/features/hitboxes/components/hitboxes-list";

export const Route = createFileRoute("/units/$unitId/info/hitboxes")({
  component: RouteComponent,
});

function RouteComponent() {
  const { unitId }: { unitId: number } = Route.useParams();
  return <HitboxesList unitId={Number(unitId)} />;
}
