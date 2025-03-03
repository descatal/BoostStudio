import { createFileRoute, Navigate } from "@tanstack/react-router";

export const Route = createFileRoute("/units/info/")({
  component: RouteComponent,
});

function RouteComponent() {
  return <Navigate to={`/units/info/stats`} />;
}
