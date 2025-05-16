import { createFileRoute, Outlet } from "@tanstack/react-router";
import { useQuery } from "@tanstack/react-query";
import { getApiUnitsByUnitIdOptions } from "@/api/exvs/@tanstack/react-query.gen";

export const Route = createFileRoute("/units/$unitId")({
  component: RouteComponent,
});

function RouteComponent() {
  const { unitId }: { unitId: number } = Route.useParams();

  const query = useQuery({
    ...getApiUnitsByUnitIdOptions({
      path: {
        unitId: unitId,
      },
    }),
  });

  const data = query.data;

  return (
    <>
      {data && (
        <>
          <div className="flex items-center justify-between space-y-2">
            <h2 className="text-3xl font-bold tracking-tight">
              {data.nameEnglish}
            </h2>
          </div>
          <label className="text-sm text-muted-foreground">
            UnitId: {data.unitId}
          </label>
        </>
      )}
      <Outlet />
    </>
  );
}
