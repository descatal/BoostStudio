import { createFileRoute, Navigate } from "@tanstack/react-router";
import UnitsSelector from "@/features/units/components/units-selector";

export const Route = createFileRoute("/tools/")({
  component: RouteComponent,
});

function RouteComponent() {
  return <Navigate to={`/tools/fhm`} replace />;
}
