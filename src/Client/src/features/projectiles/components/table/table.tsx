import { useDataTable } from "@/hooks/use-data-table";
import { DataTable } from "@/components/data-table/data-table";
import { DataTableToolbar } from "@/components/data-table/data-table-toolbar";
import { loadPaginatedProjectilesSearchParams } from "@/loaders/projectiles-search-params";
import { projectileTableColumns } from "@/features/projectiles/components/table/columns";
import { ProjectilesTableToolbarActions } from "@/features/projectiles/components/table/toolbar-actions";
import { useQuery } from "@tanstack/react-query";
import {
  getApiProjectilesOptions,
  getApiSeriesUnitsOptions,
} from "@/api/exvs/@tanstack/react-query.gen";
import { hitboxTableColumns } from "@/features/hitboxes/components/table/columns";

interface ProjectilesTableProps {
  unitId?: number | undefined;
}

const ProjectilesTable = ({ unitId }: ProjectilesTableProps) => {
  const { page, perPage, hashes, unitIds } =
    loadPaginatedProjectilesSearchParams(location.search);

  const paginatedQuery = useQuery({
    ...getApiProjectilesOptions({
      query: {
        Page: page,
        PerPage: perPage,
        Hashes: hashes?.map((x) => parseInt(x, 16)) ?? undefined,
        UnitIds: unitId ? [unitId] : (unitIds ?? undefined),
      },
    }),
  });

  const paginatedData = paginatedQuery?.data;

  const seriesUnitsQuery = useQuery({
    ...getApiSeriesUnitsOptions({
      query: {
        ListAll: true,
      },
    }),
    select: (data) =>
      data?.items
        .filter((vm) => vm.units)
        .flatMap((vm) => vm.units!)
        ?.map((type) => ({
          label: type.nameEnglish ?? "-",
          value: type.unitId!.toString(),
        })) ?? [],
  });

  let unitColumnDef = hitboxTableColumns.find((x) => x.id === "unitId");
  if (unitColumnDef?.meta) unitColumnDef.meta.options = seriesUnitsQuery.data;
  if (unitColumnDef && unitId) unitColumnDef.enableColumnFilter = false;

  const { table } = useDataTable({
    data: paginatedData?.items ?? [],
    columns: projectileTableColumns,
    initialState: {
      columnPinning: { right: ["actions"], left: ["hash"] },
      pagination: {
        pageIndex: 1,
        pageSize: 5,
      },
    },
    pageCount: paginatedData?.totalPages ?? 0,
    shallow: false,
  });

  return (
    <DataTable table={table}>
      <DataTableToolbar table={table}>
        <ProjectilesTableToolbarActions />
      </DataTableToolbar>
    </DataTable>
  );
};

export default ProjectilesTable;
