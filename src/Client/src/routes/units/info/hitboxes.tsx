import { createFileRoute } from "@tanstack/react-router";
import HitboxesList from "@/features/hitboxes/components/hitboxes-list";

export const Route = createFileRoute("/units/info/hitboxes")({
  component: RouteComponent,
});

function RouteComponent() {
  return <HitboxesList />;
}
