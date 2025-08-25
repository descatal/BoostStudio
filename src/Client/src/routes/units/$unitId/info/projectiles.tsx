import { createFileRoute } from "@tanstack/react-router";
import ProjectilesList from "@/features/projectiles/components/projectiles-list";

export const Route = createFileRoute("/units/$unitId/info/projectiles")({
  component: RouteComponent,
});

function RouteComponent() {
  const { unitId }: { unitId: number } = Route.useParams();
  return <ProjectilesList unitId={Number(unitId)} />;
}
