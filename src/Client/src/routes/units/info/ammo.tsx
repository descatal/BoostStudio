import { createFileRoute } from "@tanstack/react-router";
import AmmoList from "@/features/ammo/components/ammo-list";

export const Route = createFileRoute("/units/info/ammo")({
  component: RouteComponent,
});

function RouteComponent() {
  return <AmmoList />;
}
