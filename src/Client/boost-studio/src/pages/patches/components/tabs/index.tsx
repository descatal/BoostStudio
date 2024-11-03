import React, { useCallback, useEffect, useState } from "react"
import {
  AssetFileType,
  PaginatedListOfPatchFileSummaryVm,
  UnitDto,
  type PaginatedListOfPatchFileSummaryVmItemsInner,
} from "@/api/exvs"
import { GetApiTblById200Response } from "@/api/exvs/models/GetApiTblById200Response"
import {
  exportTbl,
  fetchPatchFileSummaries,
  fetchTblById,
  resizePatchFiles,
} from "@/api/wrapper/tbl-api"
import { PatchFilesTableToolbarActions } from "@/pages/patches/components/tabs/components/data-table/patch-files-table-toolbar-actions"
import { PatchFileTabs, PatchIdNameMap } from "@/pages/patches/libs/store"
import { useSettingsStore } from "@/pages/settings/libs/store"
import { DataTableFilterField } from "@/types"

import { useDataTable } from "@/hooks/use-react-table-2"
import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
  AlertDialogTrigger,
} from "@/components/ui/alert-dialog"
import { Button } from "@/components/ui/button"
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"
import { toast } from "@/components/ui/use-toast"
import { DataTable } from "@/components/data-table/data-table"
import { DataTableToolbar } from "@/components/data-table/data-table-toolbar"

import { patchFileColumns } from "./components/data-table/patch-file-data-table-columns"

const PatchInformation = ({
  patchId,
}: {
  patchId?: PatchFileTabs | undefined
}) => {
  const [hideNoAssetEntries, setHideNoAssetEntries] = useState(false)
  const [hideNoPathEntries, setHideNoPathEntries] = useState(false)

  const [tblResponse, setTblResponse] = useState<
    GetApiTblById200Response | undefined
  >()
  const [patchFilesResponse, setPatchFilesResponse] =
    useState<PaginatedListOfPatchFileSummaryVm>()
  const [patchFiles, setPatchFiles] = useState<
    PaginatedListOfPatchFileSummaryVmItemsInner[]
  >([])

  const stagingDirectory = useSettingsStore((state) => state.stagingDirectory)

  const getData = useCallback(async () => {
    const pagination = table.getState().pagination

    setTblResponse(
      patchId && patchId != "All"
        ? await fetchTblById({
            id: patchId,
          })
        : undefined
    )

    const search = table
      .getState()
      .columnFilters.find((column) => column.id === "fileHash")?.value as string

    const fileTypes = table
      .getState()
      .columnFilters.filter((column) => column.id === "fileType")
      .map((filter) => filter.value as AssetFileType)

    const units = table
      .getState()
      .columnFilters.find((column) => column.id === "unit")?.value as
      | UnitDto[]
      | undefined

    const unitIds =
      units && units.length > 0 ? units.map((u) => u.unitId!) : undefined

    const patchFilesResponse = await fetchPatchFileSummaries({
      versions: patchId && patchId != "All" ? [patchId] : undefined,
      page: pagination.pageIndex + 1,
      perPage: pagination.pageSize,
      unitIds: unitIds,
      assetFileTypes: fileTypes.length > 0 ? fileTypes : undefined,
    })
    setPatchFilesResponse(patchFilesResponse)
  }, [patchId])

  useEffect(() => {
    setPatchFiles(patchFilesResponse?.items ?? [])
  }, [patchFilesResponse])

  useEffect(() => {
    getData().catch((e) => console.error(e))
  }, [patchId])

  const filterFields: DataTableFilterField<PaginatedListOfPatchFileSummaryVmItemsInner>[] =
    [
      {
        type: "input",
        label: "File Hash",
        // @ts-ignore
        value: "fileHash",
        placeholder: "Filter hash...",
      },
      {
        type: "unit",
        label: "Units",
        // @ts-ignore
        value: "unit",
      },
      {
        type: "select",
        label: "File Type",
        // @ts-ignore
        value: "fileType",
        options: Object.values(AssetFileType).map((type) => ({
          label: type,
          value: type,
          withCount: true,
        })),
      },
    ]

  const { table } = useDataTable({
    data: patchFiles,
    setData: setPatchFiles,
    columns: patchFileColumns,
    pageCount: patchFilesResponse?.totalPages ?? 0,
    filterFields: filterFields,
    fetchData: getData,
  })

  return (
    <>
      <Card>
        <CardHeader className="flex-row justify-between px-7">
          <div>
            <CardTitle>{patchId ? PatchIdNameMap[patchId] : "All"}</CardTitle>
            <CardDescription className={"pt-2"}>
              Cumulative asset index:
              {tblResponse?.cumulativeAssetIndex ?? "-"}
            </CardDescription>
          </div>
          <div className={"space-x-2"}>
            <AlertDialog>
              <AlertDialogTrigger asChild>
                <Button>Resize</Button>
              </AlertDialogTrigger>
              <AlertDialogContent>
                <AlertDialogHeader>
                  <AlertDialogTitle>Are you sure?</AlertDialogTitle>
                  <AlertDialogDescription>
                    This will update the size info for all the patch file
                    entries
                    {patchId && ` in ${PatchIdNameMap[patchId]}`}
                  </AlertDialogDescription>
                </AlertDialogHeader>
                <AlertDialogFooter>
                  <AlertDialogCancel>Cancel</AlertDialogCancel>
                  <AlertDialogAction
                    onClick={async () => {
                      await resizePatchFiles({
                        resizePatchFileCommand: {
                          versions:
                            !patchId || patchId === "All"
                              ? undefined
                              : [patchId],
                        },
                      })

                      toast({
                        title: "Resize success!",
                      })
                    }}
                  >
                    Confirm
                  </AlertDialogAction>
                </AlertDialogFooter>
              </AlertDialogContent>
            </AlertDialog>
            <AlertDialog>
              <AlertDialogTrigger asChild>
                <Button>Export</Button>
              </AlertDialogTrigger>
              <AlertDialogContent>
                <AlertDialogHeader>
                  <AlertDialogTitle>Are you sure?</AlertDialogTitle>
                  <AlertDialogDescription>
                    This will replace the tbl information at{" "}
                    {patchId && PatchIdNameMap[patchId]}/PATCH.TBL
                  </AlertDialogDescription>
                </AlertDialogHeader>
                <AlertDialogFooter>
                  <AlertDialogCancel>Cancel</AlertDialogCancel>
                  <AlertDialogAction
                    onClick={async () => {
                      if (!patchId || patchId === "All") return

                      await exportTbl({
                        exportTblCommand: {
                          versions: [patchId],
                          replaceStaging: true,
                        },
                      })

                      toast({
                        title: "Export success!",
                      })
                    }}
                  >
                    Confirm
                  </AlertDialogAction>
                </AlertDialogFooter>
              </AlertDialogContent>
            </AlertDialog>
          </div>
        </CardHeader>
        <CardContent>
          <div className="space-y-4">
            <DataTable table={table}>
              <DataTableToolbar table={table} filterFields={filterFields}>
                <PatchFilesTableToolbarActions table={table} />
              </DataTableToolbar>
            </DataTable>
          </div>
        </CardContent>
      </Card>
    </>
  )
}

export default PatchInformation
