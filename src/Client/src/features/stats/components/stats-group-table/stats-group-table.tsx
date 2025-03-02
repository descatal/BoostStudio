import React from "react";
import { useSeriesUnits } from "@/features/series/api/get-series";
import { DataTableFilterField } from "@/types/index2";
import { StatDto } from "@/api/exvs";
import { useDataTable } from "@/hooks/use-react-table-3";
import { DataTable } from "@/components/data-table-2/data-table";
import { DataTableToolbar } from "@/components/data-table-2/data-table-toolbar";
import { statsGroupTableColumns } from "@/features/stats/components/stats-group-table/columns";
import { loadPaginatedStatsGroupSearchParams } from "@/loaders/stats-group-search-params";
import { useApiUnitStatsGroup } from "@/features/stats/api/get-stats";
import { StatsGroupTableToolbarActions } from "@/features/stats/components/stats-group-table/toolbar-actions";

type StatsGroupTableProps = {
  unitId?: number | undefined;
};

const StatsGroupTable = ({ unitId }: StatsGroupTableProps) => {
  const { page, perPage, ids, unitIds } = loadPaginatedStatsGroupSearchParams(
    location.search,
  );

  const paginatedUnitStats = useApiUnitStatsGroup({
    page: page,
    perPage: perPage,
    ids: ids ?? undefined,
    unitIds: unitId ? [unitId] : (unitIds ?? undefined),
  })?.data;

  const seriesUnits =
    useSeriesUnits()
      ?.data?.items.filter((vm) => vm.units)
      .flatMap((vm) => vm.units!) ?? [];

  const baseFilterFields: DataTableFilterField<StatDto>[] = [
    {
      id: "id",
      label: "Id",
      placeholder: "Filter by Id",
    },
    {
      id: "unitId",
      label: "Units",
      options: seriesUnits.map((type) => ({
        label: type.nameEnglish ?? "-",
        value: type.unitId!.toString(),
      })),
    },
  ];

  const filterFields = baseFilterFields.filter((field) => {
    if (field.id === "unitId" && unitId) return false;
    return true;
  });

  const { table } = useDataTable({
    data: paginatedUnitStats?.items ?? [],
    columns: statsGroupTableColumns,
    filterFields: filterFields,
    initialState: {
      columnPinning: { right: ["actions"], left: ["id"] },
    },
    pageCount: paginatedUnitStats?.totalPages ?? 0,
    shallow: false,
  });

  return (
    <DataTable table={table}>
      <DataTableToolbar table={table} filterFields={filterFields}>
        <StatsGroupTableToolbarActions />
      </DataTableToolbar>
    </DataTable>
  );
};

export default StatsGroupTable;
