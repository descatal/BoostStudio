import { createFileRoute } from "@tanstack/react-router";
import StatsList from "@/features/stats/components/stats-list";

export const Route = createFileRoute("/units/$unitId/info/stats")({
  component: RouteComponent,
});

function RouteComponent() {
  const { unitId }: { unitId: number } = Route.useParams();
  return <StatsList unitId={Number(unitId)} />;
}
