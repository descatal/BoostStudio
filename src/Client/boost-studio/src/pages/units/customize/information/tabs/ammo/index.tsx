import React, {useCallback, useEffect, useState} from 'react';
import {AmmoDto, GetApiAmmo200Response,} from "@/api/exvs";
import {DataTable} from "@/components/data-table/data-table";
import {ammoColumns} from "@/pages/units/customize/information/tabs/ammo/ammo-data-table-columns";
import {AmmoDataTableToolbar} from "@/pages/units/customize/information/tabs/ammo/ammo-data-table-toolbar";
import {Card, CardContent, CardDescription, CardHeader, CardTitle} from "@/components/ui/card";
import {toast} from '@/components/ui/use-toast';
import {fetchAmmo, updateAmmo} from '@/api/wrapper/ammo-api';
import {useDataTable} from "@/hooks/use-react-table-2";
import {DataTableToolbar} from '@/components/data-table/data-table-toolbar';
import {DataTableFilterField} from '@/types';

const Ammo = ({unitId}: { unitId: number }) => {
  const [response, setResponse] = useState<GetApiAmmo200Response>();
  const [ammo, setAmmo] = useState<AmmoDto[]>([]);

  const getData = useCallback(async () => {
    const pagination = table.getState().pagination
    const hash = table.getState().columnFilters.find((column) => column.id === 'hash')?.value as string;
    const hashes = hash?.split(';').map(x => Number(x))    
    const ammo200Response = await fetchAmmo({
      unitIds: [unitId], 
      page: pagination.pageIndex + 1, 
      perPage: pagination.pageSize,
      hash: hashes
    })
    setResponse(ammo200Response)
  }, []);

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
  }, []);

  useEffect(() => {
    setAmmo(response?.items ?? [])
  }, [response])

  useEffect(() => {
    getData().catch(console.error)
  }, []);

  const filterFields: DataTableFilterField<AmmoDto>[] = [{
    label: "Title",
    value: "hash",
    placeholder: "Search by hash"
  }]

  const {table} = useDataTable({
    data: ammo,
    setData: setAmmo,
    columns: ammoColumns,
    pageCount: response?.totalPages ?? 0,
    filterFields: filterFields,
    fetchData: getData,
    saveData: saveData
  });

  return (
    <div>
      {response
        ?
        <Card>
          <CardHeader className="px-7">
            <CardTitle>Ammo</CardTitle>
            <CardDescription>All ammo info associated with this unit.</CardDescription>
          </CardHeader>
          <CardContent>
            <div className="space-y-4">
              <DataTable table={table}>
                <DataTableToolbar table={table} filterFields={filterFields}>
                  <AmmoDataTableToolbar table={table} />
                </DataTableToolbar>
              </DataTable>
            </div>
          </CardContent>
        </Card>
        : <></>
      }
    </div>
  );
};

export default Ammo;
