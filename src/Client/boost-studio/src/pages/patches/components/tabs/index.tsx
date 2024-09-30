import React, { useCallback, useEffect, useState } from "react"
import {
  AssetFileType,
  PaginatedListOfPatchFileSummaryVm,
  UnitDto,
  type PaginatedListOfPatchFileSummaryVmItemsInner,
} from "@/api/exvs"
import { GetApiTblById200Response } from "@/api/exvs/models/GetApiTblById200Response"
import { fetchPatchFileSummaries, fetchTbl } from "@/api/wrapper/tbl-api"
import { PatchFilesTableToolbarActions } from "@/pages/patches/components/tabs/components/data-table/patch-files-table-toolbar-actions"
import { PatchFileVersions, PatchIdNameMap } from "@/pages/patches/libs/store"
import { DataTableFilterField } from "@/types"

import { useDataTable } from "@/hooks/use-react-table-2"
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"
import { DataTable } from "@/components/data-table/data-table"
import { DataTableToolbar } from "@/components/data-table/data-table-toolbar"

import { patchFileColumns } from "./components/data-table/patch-file-data-table-columns"

const PatchInformation = ({ patchId }: { patchId: PatchFileVersions }) => {
  const [hideNoAssetEntries, setHideNoAssetEntries] = useState(false)
  const [hideNoPathEntries, setHideNoPathEntries] = useState(false)

  const [tblResponse, setTblTblResponse] = useState<GetApiTblById200Response>()
  const [patchFilesResponse, setPatchFilesResponse] =
    useState<PaginatedListOfPatchFileSummaryVm>()
  const [patchFiles, setPatchFiles] = useState<
    PaginatedListOfPatchFileSummaryVmItemsInner[]
  >([])

  const getData = useCallback(async () => {
    const pagination = table.getState().pagination

    const getApiHitboxes200Response = await fetchTbl({
      id: patchId,
    })
    setTblTblResponse(getApiHitboxes200Response)

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
      tblIds: [patchId],
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
    <div>
      {tblResponse ? (
        <Card>
          <CardHeader className="px-7">
            <CardTitle>{PatchIdNameMap[patchId]}</CardTitle>
            <CardDescription>File metadata (Tbl)</CardDescription>
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
      ) : (
        <></>
      )}
    </div>
  )
}

export default PatchInformation
