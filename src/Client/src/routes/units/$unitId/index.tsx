import { createFileRoute, Navigate } from "@tanstack/react-router";

export const Route = createFileRoute("/units/$unitId/")({
  component: RouteComponent,
});

function RouteComponent() {
  const { unitId }: { unitId: number } = Route.useParams();
  return (
    <Navigate
      to={`/units/$unitId/info`}
      params={{ unitId: unitId.toString() }}
      replace
    />
  );
}
