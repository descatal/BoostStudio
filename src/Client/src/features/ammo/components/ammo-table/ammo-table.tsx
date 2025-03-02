import React from "react";
import { useSeriesUnits } from "@/features/series/api/get-series";
import { DataTableFilterField } from "@/types/index2";
import { useDataTable } from "@/hooks/use-react-table-3";
import { DataTable } from "@/components/data-table-2/data-table";
import { DataTableToolbar } from "@/components/data-table-2/data-table-toolbar";
import { loadPaginatedAmmoSearchParams } from "@/loaders/ammo-search-params";
import { useApiAmmo } from "@/features/ammo/api/get-ammo";
import { ammoTableColumns } from "@/features/ammo/components/ammo-table/columns";
import { AmmoDto } from "@/api/exvs";
import { AmmoTableToolbarActions } from "@/features/ammo/components/ammo-table/toolbar-actions";

type AmmoTableProps = {
  unitId?: number | undefined;
};

const AmmoTable = ({ unitId }: AmmoTableProps) => {
  const { page, perPage, hashes, unitIds } = loadPaginatedAmmoSearchParams(
    location.search,
  );

  const paginatedData = useApiAmmo({
    page: page,
    perPage: perPage,
    hash: hashes?.map((x) => parseInt(x, 16)) ?? undefined,
    unitIds: unitId ? [unitId] : (unitIds ?? undefined),
  })?.data;

  const seriesUnits =
    useSeriesUnits()
      ?.data?.items.filter((vm) => vm.units)
      .flatMap((vm) => vm.units!) ?? [];

  const baseFilterFields: DataTableFilterField<AmmoDto>[] = [
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
    columns: ammoTableColumns,
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
        <AmmoTableToolbarActions />
      </DataTableToolbar>
    </DataTable>
  );
};

export default AmmoTable;
