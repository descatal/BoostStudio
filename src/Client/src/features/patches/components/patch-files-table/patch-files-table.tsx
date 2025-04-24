import React from "react";
import { DataTable } from "@/components/data-table-2/data-table";
import { DataTableToolbar } from "@/components/data-table-2/data-table-toolbar";
import { PatchFilesListToolbarActions } from "@/features/patches/components/patch-files-table/toolbar-actions";
import { loadPaginatedPatchesSearchParams } from "@/loaders/patches-search-params";
import { useTblPatchFiles } from "@/features/patches/api/get-tbl-patches";
import { useSeriesUnits } from "@/features/series/api/get-series";
import { DataTableFilterField } from "@/types/index2";
import {
  AssetFileType,
  PatchFileSummaryVm,
  PatchFileVersion,
} from "@/api/exvs";
import { toSentenceCase } from "@/lib/utils";
import { useDataTable } from "@/hooks/use-react-table-3";
import { patchFilesListColumns } from "@/features/patches/components/patch-files-table/columns";

type PatchFilesTableProps = {
  patchId?: PatchFileVersion | undefined;
};

const PatchFilesTable = ({ patchId }: PatchFilesTableProps) => {
  const { page, perPage, assetFileHashes, fileTypes } =
    loadPaginatedPatchesSearchParams(location.search);

  const patchFiles = useTblPatchFiles({
    page: page,
    perPage: perPage,
    versions: !patchId ? undefined : [patchId],
    assetFileHashes: assetFileHashes ? assetFileHashes : undefined,
    assetFileTypes: fileTypes ? fileTypes : undefined,
  })?.data;

  const seriesUnits =
    useSeriesUnits()
      ?.data?.items.filter((vm) => vm.units)
      .flatMap((vm) => vm.units!) ?? [];

  const filterFields: DataTableFilterField<PatchFileSummaryVm>[] = [
    {
      id: "assetFileHash",
      label: "File Hash",
      placeholder: "Filter by Asset File Hash",
    },
    {
      // @ts-ignore
      id: "fileType",
      label: "File Type",
      options: Object.keys(AssetFileType).map((type) => ({
        label: toSentenceCase(type),
        value: type,
      })),
    },
    {
      // @ts-ignore
      id: "units",
      label: "Units",
      options: seriesUnits.map((type) => ({
        label: type.nameEnglish ?? "-",
        value: type.unitId!.toString(),
      })),
    },
  ];

  const { table } = useDataTable({
    data: patchFiles?.items ?? [],
    columns: patchFilesListColumns,
    filterFields: filterFields,
    initialState: {
      columnPinning: { right: ["actions"] },
    },
    pageCount: patchFiles?.totalPages ?? 0,
    shallow: false,
  });

  return (
    <DataTable table={table}>
      <DataTableToolbar table={table} filterFields={filterFields}>
        <PatchFilesListToolbarActions table={table} />
      </DataTableToolbar>
    </DataTable>
  );
};

export default PatchFilesTable;
