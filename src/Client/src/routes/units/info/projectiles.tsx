import { createFileRoute } from "@tanstack/react-router";
import ProjectilesList from "@/features/projectiles/components/projectiles-list";

export const Route = createFileRoute("/units/info/projectiles")({
  component: RouteComponent,
});

function RouteComponent() {
  return <ProjectilesList />;
}
