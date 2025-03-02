import { createFileRoute, Navigate } from "@tanstack/react-router";
import { UnitCustomizableInfoSections } from "@/lib/constants";

export const Route = createFileRoute("/units/$unitId/")({
  component: RouteComponent,
});

function RouteComponent() {
  const { unitId }: { unitId: number } = Route.useParams();
  return (
    <Navigate
      to={`/units/$unitId/info`}
      params={{ unitId: unitId.toString() }}
    />
  );
}
