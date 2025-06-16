import { useDataTable } from "@/hooks/use-data-table";
import { statsGroupTableColumns } from "@/features/stats/components/stats-group-table/columns";
import { loadPaginatedStatsGroupSearchParams } from "@/loaders/stats-group-search-params";
import { useQuery } from "@tanstack/react-query";
import {
  getApiSeriesUnitsOptions,
  getApiStatsOptions,
} from "@/api/exvs/@tanstack/react-query.gen";
import { DataTable } from "@/components/data-table/data-table";
import { DataTableToolbar } from "@/components/data-table/data-table-toolbar";
import { StatsGroupTableToolbarActions } from "@/features/stats/components/stats-group-table/toolbar-actions";
import { hitboxTableColumns } from "@/features/hitboxes/components/table/columns";

type StatsGroupTableProps = {
  unitId?: number | undefined;
};

const StatsGroupTable = ({ unitId }: StatsGroupTableProps) => {
  const { page, perPage, ids, unitIds } = loadPaginatedStatsGroupSearchParams(
    location.search,
  );

  const paginatedUnitStatsQuery = useQuery({
    ...getApiStatsOptions({
      query: {
        Page: page,
        PerPage: perPage,
        Ids: ids ?? undefined,
        UnitIds: unitId ? [unitId] : (unitIds ?? undefined),
      },
    }),
  });

  const paginatedUnitStats = paginatedUnitStatsQuery?.data;

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
    data: paginatedUnitStats?.items ?? [],
    columns: statsGroupTableColumns,
    initialState: {
      columnPinning: { right: ["actions"], left: ["id"] },
      pagination: {
        pageIndex: 1,
        pageSize: 5,
      },
    },
    pageCount: paginatedUnitStats?.totalPages ?? 0,
    shallow: false,
  });

  return (
    <DataTable table={table}>
      <DataTableToolbar table={table}>
        <StatsGroupTableToolbarActions />
      </DataTableToolbar>
    </DataTable>
  );
};

export default StatsGroupTable;
