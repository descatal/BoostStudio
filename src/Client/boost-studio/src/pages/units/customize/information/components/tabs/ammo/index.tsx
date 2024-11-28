import React, { useCallback, useEffect, useState } from "react"
import { AmmoDto } from "@/api/exvs"
import { GetApiAmmo200Response } from "@/api/exvs/models/GetApiAmmo200Response"
import { fetchAmmo, updateAmmo } from "@/api/wrapper/ammo-api"
import { DataTableFilterField } from "@/types"

import { useDataTable } from "@/hooks/use-react-table-2"
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

import { ammoColumns } from "./components/data-table/ammo-data-table-columns"
import { AmmoDataTableToolbar } from "./components/data-table/ammo-data-table-toolbar"

const Ammo = ({ unitId }: { unitId: number }) => {
  const [response, setResponse] = useState<GetApiAmmo200Response>()
  const [ammo, setAmmo] = useState<AmmoDto[]>([])

  const getData = useCallback(async () => {
    const pagination = table.getState().pagination
    const hash = table
      .getState()
      .columnFilters.find((column) => column.id === "hash")?.value as string
    const hashes = hash?.split(";").map((x) => Number(x))
    const ammo200Response = await fetchAmmo({
      unitIds: [unitId],
      page: pagination.pageIndex + 1,
      perPage: pagination.pageSize,
      hash: hashes,
    })
    setResponse(ammo200Response)
  }, [unitId])

  const saveData = useCallback(async () => {
    const modifiedRows = table.options.meta?.modifiedRows ?? []
    const modifiedEntities = modifiedRows.map((rowIndex, index) => {
      return table.getRow(rowIndex.toString()).original
    })

    for (const entity of modifiedEntities) {
      await updateAmmo(entity)
    }

    toast({
      title: "Saved!",
      description: "Save operation successful.",
    })
    await getData()
  }, [])

  useEffect(() => {
    setAmmo(response?.items ?? [])
  }, [response])

  useEffect(() => {
    getData().catch(console.error)
  }, [unitId])

  // const filterFields: DataTableFilterField<AmmoDto>[] = [
  //   {
  //     label: "Title",
  //     value: "hash",
  //     placeholder: "Search by hash",
  //   },
  // ]

  const { table } = useDataTable({
    data: ammo,
    setData: setAmmo,
    columns: ammoColumns,
    pageCount: response?.totalPages ?? 0,
    // filterFields: filterFields,
    fetchData: getData,
    saveData: saveData,
    enableEditingMode: true,
  })

  return (
    <div>
      {response ? (
        <Card>
          <CardHeader className="px-7">
            <CardTitle>Ammo</CardTitle>
            <CardDescription>
              All ammo info associated with this unit.
            </CardDescription>
          </CardHeader>
          <CardContent>
            <div className="space-y-4">
              <DataTable table={table}>
                <DataTableToolbar table={table}>
                  <AmmoDataTableToolbar table={table} />
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

export default Ammo
