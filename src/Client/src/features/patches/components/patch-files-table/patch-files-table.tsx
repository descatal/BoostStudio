import React from "react";
import { DataTable } from "@/components/data-table/data-table";
import { DataTableToolbar } from "@/components/data-table/data-table-toolbar";
import { PatchFilesListToolbarActions } from "@/features/patches/components/patch-files-table/toolbar-actions";
import { loadPaginatedPatchesSearchParams } from "@/loaders/patches-search-params";
import { PatchFileVersion } from "@/api/exvs";
import { useDataTable } from "@/hooks/use-data-table";
import { patchFilesListColumns } from "@/features/patches/components/patch-files-table/columns";
import { useQuery } from "@tanstack/react-query";
import {
  getApiPatchFilesSummaryOptions,
  getApiSeriesUnitsOptions,
} from "@/api/exvs/@tanstack/react-query.gen";

type PatchFilesTableProps = {
  patchId?: PatchFileVersion | undefined;
};

const PatchFilesTable = ({ patchId }: PatchFilesTableProps) => {
  const { page, perPage, assetFileHashes, fileTypes, unitIds } =
    loadPaginatedPatchesSearchParams(location.search);

  const paginatedQuery = useQuery({
    ...getApiPatchFilesSummaryOptions({
      query: {
        Page: page,
        PerPage: perPage,
        Versions: !patchId ? undefined : [patchId],
        AssetFileHashes:
          assetFileHashes?.map((x) => parseInt(x, 16)) ?? undefined,
        AssetFileTypes: fileTypes ? fileTypes : undefined,
        UnitIds: unitIds ? unitIds : undefined,
      },
    }),
  });

  const paginatedData = paginatedQuery.data;

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

  let unitColumnDef = patchFilesListColumns.find((x) => x.id === "unitId");
  if (unitColumnDef?.meta) unitColumnDef.meta.options = seriesUnitsQuery.data;

  const { table } = useDataTable({
    data: paginatedData?.items ?? [],
    columns: patchFilesListColumns,
    initialState: {
      columnPinning: { right: ["actions"] },
    },
    pageCount: paginatedData?.totalPages ?? 0,
    shallow: false,
  });

  return (
    <DataTable table={table}>
      <DataTableToolbar table={table}>
        <PatchFilesListToolbarActions table={table} />
      </DataTableToolbar>
    </DataTable>
  );
};

export default PatchFilesTable;
