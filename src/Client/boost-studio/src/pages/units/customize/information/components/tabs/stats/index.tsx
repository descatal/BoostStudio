import React, { useCallback, useEffect, useState } from "react"
import {
  GetApiStats200Response,
  GetApiUnitStats200ResponseItemsInner,
  UnitAmmoSlotDto,
} from "@/api/exvs"
import { GetApiStatsById200Response } from "@/api/exvs/models/GetApiStatsById200Response"
import { fetchAmmoOptions } from "@/api/wrapper/ammo-api"
import {
  fetchStats,
  fetchUnitStatsByUnitId,
  updateStats,
} from "@/api/wrapper/stats-api"

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

import AmmoSlots from "./components/ammo-slots"
import { statGroupColumns } from "./components/data-table/stat-group-data-table-columns"
import { StatGroupDataTableToolbar } from "./components/data-table/stat-group-data-table-toolbar"

const Stats = ({ unitId }: { unitId: number }) => {
  const [statsResponse, setStatsResponse] = useState<GetApiStats200Response>()
  const [stats, setStats] = useState<GetApiStatsById200Response[]>([])

  const [unitStatsResponse, setUnitStatsResponse] =
    useState<GetApiUnitStats200ResponseItemsInner>()
  const [ammoSlots, setAmmoSlots] = useState<UnitAmmoSlotDto[]>([])

  const [ammoOptions, setAmmoOptions] = useState<number[]>([])

  const getUnitStats = useCallback(async () => {
    const response = await fetchUnitStatsByUnitId({
      unitId: unitId,
    })
    setUnitStatsResponse(response)
  }, [unitId])

  const getAmmoOptionsData = useCallback(async () => {
    const ammoOptions = await fetchAmmoOptions([unitId])
    setAmmoOptions(ammoOptions)
  }, [])

  const getData = useCallback(async () => {
    const pagination = table.getState().pagination
    const apiUnitStats200Response = await fetchStats({
      unitIds: [unitId],
      page: pagination.pageIndex + 1,
      perPage: pagination.pageSize,
    })
    setStatsResponse(apiUnitStats200Response)
  }, [unitId])

  const saveData = useCallback(async () => {
    const modifiedRows = table.options.meta?.modifiedRows ?? []
    const modifiedEntities = modifiedRows.map((rowIndex, index) => {
      return table.getRow(rowIndex.toString()).original
    })

    for (const entity of modifiedEntities) {
      if (entity.id == null) continue
      await updateStats(entity.id, { ...entity, id: entity.id! })
    }

    toast({
      title: "Saved!",
      description: "Save operation successful.",
    })
    await getData()
  }, [])

  useEffect(() => {
    let statGroups = statsResponse?.items ?? []
    statGroups = statGroups.sort((a, b) => (a.order ?? 0) - (b.order ?? 0))
    setStats(statGroups)
  }, [statsResponse])

  useEffect(() => {
    setAmmoSlots(unitStatsResponse?.ammoSlots ?? [])
  }, [unitStatsResponse])

  useEffect(() => {
    getUnitStats().catch((e) => console.error(e))
    getData().catch((e) => console.error(e))
    getAmmoOptionsData().catch((e) => console.error(e))
  }, [unitId])

  const { table } = useDataTable({
    data: stats,
    setData: setStats,
    columns: statGroupColumns,
    pageCount: statsResponse?.totalPages ?? 0,
    fetchData: getData,
    saveData: saveData,
  })

  return (
    <>
      <Card className="col-span-full">
        <CardHeader>
          <CardTitle>
            <div className="flex items-center justify-between space-y-2">
              <h2 className="text-2xl font-bold tracking-tight">Ammo Slots</h2>
              {/*<div className="flex items-center space-x-2">*/}
              {/*  <Button>Edit</Button>*/}
              {/*</div>*/}
            </div>
          </CardTitle>
          <CardDescription>
            The ammo assigned to each ammo slot when spawning this unit.
          </CardDescription>
        </CardHeader>
        <CardContent>
          <AmmoSlots
            unitId={unitId}
            ammoSlots={ammoSlots ?? []}
            ammoOptions={ammoOptions}
          />
        </CardContent>
      </Card>
      <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-7">
        <Card className="col-span-full">
          <CardHeader>
            <CardTitle>Stat Groups</CardTitle>
            <CardDescription>
              Stat groups associated with this unit.
            </CardDescription>
          </CardHeader>
          <CardContent>
            <div className="space-y-4">
              <DataTable table={table}>
                <DataTableToolbar table={table} filterFields={[]}>
                  <StatGroupDataTableToolbar table={table} />
                </DataTableToolbar>
              </DataTable>
            </div>
          </CardContent>
        </Card>
      </div>
    </>
  )
}

export default Stats
