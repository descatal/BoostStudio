import { createFileRoute } from "@tanstack/react-router";
import StatsList from "@/features/stats/components/stats-list";

export const Route = createFileRoute("/units/info/stats")({
  component: RouteComponent,
});

function RouteComponent() {
  return <StatsList />;
}
