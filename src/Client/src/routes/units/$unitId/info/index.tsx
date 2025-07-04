import { createFileRoute, Navigate } from "@tanstack/react-router";

export const Route = createFileRoute("/units/$unitId/info/")({
  component: RouteComponent,
});

function RouteComponent() {
  const { unitId }: { unitId: number } = Route.useParams();
  return (
    <Navigate
      to={`/units/$unitId/info/stats`}
      params={{ unitId: unitId.toString() }}
      replace
    />
  );
}
