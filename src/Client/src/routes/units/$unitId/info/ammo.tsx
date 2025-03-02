import { createFileRoute } from "@tanstack/react-router";
import AmmoList from "@/features/ammo/components/ammo-list";

export const Route = createFileRoute("/units/$unitId/info/ammo")({
  component: RouteComponent,
});

function RouteComponent() {
  const { unitId }: { unitId: number } = Route.useParams();
  return <AmmoList unitId={unitId} />;
}
