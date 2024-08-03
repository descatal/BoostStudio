import React, {useCallback, useEffect, useState} from 'react';
import {Card, CardContent, CardDescription, CardHeader, CardTitle} from "@/components/ui/card";
import {GetApiStats200Response, GetApiUnitStats200ResponseItemsInner, UnitAmmoSlotDto} from '@/api/exvs';
import {DataTable} from "@/components/data-table/data-table";
import {fetchStats, fetchUnitStats, updateStats} from "@/api/wrapper/stats-api";
import {StatGroupDataTableToolbar} from './stat-group-data-table-toolbar';
import {statGroupColumns} from "@/pages/units/customize/information/tabs/stats/stat-group-data-table-columns";
import {toast} from '@/components/ui/use-toast';
import {GetApiStatsById200Response} from "@/api/exvs/models/GetApiStatsById200Response";
import {useDataTable} from '@/hooks/use-react-table-2';
import {DataTableToolbar} from "@/components/data-table/data-table-toolbar";
import {DataTableFilterField} from "@/types";
import AmmoSlots from './ammo-slots';
import {fetchAmmoOptions} from '@/api/wrapper/ammo-api';

const Stats = ({unitId}: { unitId: number }) => {
  const [statsResponse, setStatsResponse] = useState<GetApiStats200Response>();
  const [stats, setStats] = useState<GetApiStatsById200Response[]>([]);
  
  const [unitStatsResponse, setUnitStatsResponse] = useState<GetApiUnitStats200ResponseItemsInner>();
  const [ammoSlots, setAmmoSlots] = useState<UnitAmmoSlotDto[]>([]);
  
  const [ammoOptions, setAmmoOptions] = useState<number[]>([]);

  const getUnitStats = useCallback(async () => {
    const getApiUnitStats200ResponseItemsInner = await fetchUnitStats(unitId)
    setUnitStatsResponse(getApiUnitStats200ResponseItemsInner)
  }, []);

  const getAmmoOptionsData = useCallback(async () => {
    const ammoOptions = await fetchAmmoOptions([unitId])
    setAmmoOptions(ammoOptions)
  }, []);
  
  const getData = useCallback(async () => {
    const pagination = table.getState().pagination
    const apiUnitStats200Response = await fetchStats({
      unitIds: [unitId],
      page: pagination.pageIndex + 1,
      perPage: pagination.pageSize
    })
    setStatsResponse(apiUnitStats200Response)
  }, []);

  const saveData = useCallback(async () => {
    const modifiedRows = table.options.meta?.modifiedRows ?? []
    const modifiedEntities = modifiedRows.map((rowIndex, index) => {
      return table.getRow(rowIndex.toString()).original
    })

    for (const entity of modifiedEntities) {
      if (entity.id == null) continue;
      await updateStats(entity.id, {...entity, id: entity.id!})
    }

    toast({
      title: "Saved!",
      description: "Save operation successful.",
    })
    await getData()
  }, []);

  useEffect(() => {
    let statGroups = statsResponse?.items ?? [];
    statGroups = statGroups.sort((a, b) => (a.order ?? 0) - (b.order ?? 0))
    setStats(statGroups)
  }, [statsResponse]) //

  useEffect(() => {
    setAmmoSlots(unitStatsResponse?.ammoSlots ?? [])
  }, [unitStatsResponse])

  useEffect(() => {
    getUnitStats().catch(console.error)
    getData().catch(console.error)
    getAmmoOptionsData().catch(console.error)
  }, []);

  const filterFields: DataTableFilterField<GetApiStatsById200Response>[] = [];

  const {table} = useDataTable({
    data: stats,
    setData: setStats,
    columns: statGroupColumns,
    pageCount: statsResponse?.totalPages ?? 0,
    fetchData: getData,
    saveData: saveData
  });
  
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
          <AmmoSlots unitId={unitId} ammoSlots={unitStatsResponse?.ammoSlots ?? []} ammoOptions={ammoOptions}/>
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
                <DataTableToolbar table={table} filterFields={filterFields}>
                  <StatGroupDataTableToolbar table={table}/>
                </DataTableToolbar>
              </DataTable>
            </div>
          </CardContent>
        </Card>
      </div>
    </>
  );
};

export default Stats;