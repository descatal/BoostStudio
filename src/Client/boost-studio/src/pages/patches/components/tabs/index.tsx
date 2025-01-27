import React from "react"
import {
  AssetFileType,
  PatchFileVersion,
  type PatchFileSummaryVm,
} from "@/api/exvs"
import { useTblById } from "@/features/patches/api/get-tbl"
import { useTblPatchFiles } from "@/features/patches/api/get-tbl-patches"
import ExportTblDialog from "@/features/patches/components/export-tbl-dialog"
import ResizePatchDialog from "@/features/patches/components/resize-patch-dialog"
import { useSeriesUnits } from "@/features/series/api/get-series"
import { loadPaginatedPatchesSearchParams } from "@/loaders/patches-search-params"
import { PatchFilesTableToolbarActions } from "@/pages/patches/components/tabs/components/data-table/patch-files-table-toolbar-actions"
import { PatchFileTabs, PatchIdNameMap } from "@/pages/patches/libs/store"

import { DataTableFilterField } from "@/types/index2"
import { toSentenceCase } from "@/lib/utils"
import { useDataTable } from "@/hooks/use-react-table-3"
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"
import { DataTable } from "@/components/data-table-2/data-table"
import { DataTableSkeleton } from "@/components/data-table-2/data-table-skeleton"
import { DataTableToolbar } from "@/components/data-table-2/data-table-toolbar"

import { patchFileColumns } from "./components/data-table/patch-file-data-table-columns"

const PatchInformation = ({
  patchId,
}: {
  patchId?: PatchFileTabs | undefined
}) => {
  const { page, perPage, assetFileHashes, fileTypes } =
    loadPaginatedPatchesSearchParams(location.search)

  const patchFiles = useTblPatchFiles({
    page: page,
    perPage: perPage,
    versions: !patchId || patchId === "All" ? undefined : [patchId],
    assetFileHashes: assetFileHashes ? assetFileHashes : undefined,
    assetFileTypes: fileTypes ? fileTypes : undefined,
  })?.data

  const tblInfo =
    !patchId || patchId === "All" ? undefined : useTblById(patchId)?.data

  const seriesUnits =
    useSeriesUnits()
      ?.data?.items.filter((vm) => vm.units)
      .flatMap((vm) => vm.units!) ?? []

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
  ]

  const { table } = useDataTable({
    data: patchFiles?.items ?? [],
    columns: patchFileColumns,
    filterFields: filterFields,
    initialState: {
      columnPinning: { right: ["actions"] },
    },
    pageCount: patchFiles?.totalPages ?? 0,
    shallow: false,
  })

  return (
    <>
      <Card>
        <CardHeader className="flex-row justify-between px-7">
          <div>
            <CardTitle>{patchId ? PatchIdNameMap[patchId] : "All"}</CardTitle>
            <CardDescription className={"pt-2"}>
              Cumulative asset index:
              {tblInfo?.cumulativeAssetIndex ?? "-"}
            </CardDescription>
          </div>
          <div className={"space-x-2"}>
            <ResizePatchDialog patchId={patchId} />
            <ExportTblDialog patchId={patchId} />
          </div>
        </CardHeader>
        <CardContent>
          <div className="space-y-4">
            <React.Suspense
              fallback={
                <DataTableSkeleton
                  columnCount={6}
                  searchableColumnCount={1}
                  filterableColumnCount={2}
                  cellWidths={[
                    "10rem",
                    "40rem",
                    "12rem",
                    "12rem",
                    "8rem",
                    "8rem",
                  ]}
                  shrinkZero
                />
              }
            >
              <DataTable table={table}>
                <DataTableToolbar table={table} filterFields={filterFields}>
                  <PatchFilesTableToolbarActions table={table} />
                </DataTableToolbar>
              </DataTable>
            </React.Suspense>
          </div>
        </CardContent>
      </Card>
    </>
  )
}

export default PatchInformation
