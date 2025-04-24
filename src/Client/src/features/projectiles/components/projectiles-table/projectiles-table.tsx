import React from "react";
import { useSeriesUnits } from "@/features/series/api/get-series";
import { DataTableFilterField } from "@/types/index2";
import { ProjectileDto } from "@/api/exvs";
import { useDataTable } from "@/hooks/use-react-table-3";
import { DataTable } from "@/components/data-table-2/data-table";
import { DataTableToolbar } from "@/components/data-table-2/data-table-toolbar";
import { loadPaginatedProjectilesSearchParams } from "@/loaders/projectiles-search-params";
import { useApiProjectiles } from "@/features/projectiles/api/get-projectiles";
import { projectileTableColumns } from "@/features/projectiles/components/projectiles-table/columns";
import { ProjectilesTableToolbarActions } from "@/features/projectiles/components/projectiles-table/toolbar-actions";

interface ProjectilesTableProps {
  unitId?: number | undefined;
}

const ProjectilesTable = ({ unitId }: ProjectilesTableProps) => {
  const { page, perPage, hashes, unitIds } =
    loadPaginatedProjectilesSearchParams(location.search);

  const paginatedData = useApiProjectiles({
    page: page,
    perPage: perPage,
    hashes: hashes?.map((x) => parseInt(x, 16)) ?? undefined,
    unitIds: unitId ? [unitId] : (unitIds ?? undefined),
  })?.data;

  const seriesUnits =
    useSeriesUnits()
      ?.data?.items.filter((vm) => vm.units)
      .flatMap((vm) => vm.units!) ?? [];

  const baseFilterFields: DataTableFilterField<ProjectileDto>[] = [
    {
      id: "hash",
      label: "Hash",
      placeholder: "Filter by Hash (Hex)",
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
    data: paginatedData?.items ?? [],
    columns: projectileTableColumns,
    filterFields: filterFields,
    initialState: {
      columnPinning: { right: ["actions"], left: ["hash"] },
    },
    pageCount: paginatedData?.totalPages ?? 0,
    shallow: false,
  });

  return (
    <DataTable table={table}>
      <DataTableToolbar table={table} filterFields={filterFields}>
        <ProjectilesTableToolbarActions />
      </DataTableToolbar>
    </DataTable>
  );
};

export default ProjectilesTable;
