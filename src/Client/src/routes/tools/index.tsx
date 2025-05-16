import { createFileRoute, Navigate } from "@tanstack/react-router";

export const Route = createFileRoute("/tools/")({
  component: RouteComponent,
});

function RouteComponent() {
  return <Navigate to={`/tools/fhm`} replace />;
}
