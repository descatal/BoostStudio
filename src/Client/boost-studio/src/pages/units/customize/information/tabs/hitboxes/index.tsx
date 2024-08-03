import React, {useCallback, useEffect, useState} from 'react';
import {GetApiHitboxes200Response, GetApiHitboxesByHash200Response} from "@/api/exvs";
import {DataTableFilterField} from "@/types";
import {useDataTable} from "@/hooks/use-react-table-2";
import {toast} from "@/components/ui/use-toast";
import {Card, CardContent, CardDescription, CardHeader, CardTitle} from "@/components/ui/card";
import {DataTable} from "@/components/data-table/data-table";
import {DataTableToolbar} from "@/components/data-table/data-table-toolbar";
import {HitboxDataTableToolbar} from './hitbox-data-table-toolbar';
import {fetchHitboxes, updateHitbox} from '@/api/wrapper/hitbox-api';
import {hitboxColumns} from './hitbox-data-table-columns';

const Hitboxes = ({unitId}: { unitId: number }) => {
  const [response, setResponse] = useState<GetApiHitboxes200Response>();
  const [hitboxes, setHitboxes] = useState<GetApiHitboxesByHash200Response[]>([]);

  const getData = useCallback(async () => {
    const pagination = table.getState().pagination
    const hash = table.getState().columnFilters.find((column) => column.id === 'hash')?.value as string;
    const hashes = hash?.split(';').map(x => Number(x))
    const getApiHitboxes200Response = await fetchHitboxes({
      unitIds: [unitId],
      page: pagination.pageIndex + 1,
      perPage: pagination.pageSize,
      hashes: hashes
    })
    setResponse(getApiHitboxes200Response)
  }, []);

  const saveData = useCallback(async () => {
    const modifiedRows = table.options.meta?.modifiedRows ?? []
    const modifiedEntities = modifiedRows.map((rowIndex, index) => {
      return table.getRow(rowIndex.toString()).original
    })

    for (const entity of modifiedEntities) {
      if (!entity.hash) continue
      await updateHitbox({
        hash: entity.hash!,
        postApiHitboxesByHashRequest: {
          ...entity,
          hash: entity.hash!
        }
      })
    }

    toast({
      title: "Saved!",
      description: "Save operation successful.",
    })
    await getData()
  }, []);

  useEffect(() => {
    setHitboxes(response?.items ?? [])
  }, [response])

  useEffect(() => {
    getData().catch(console.error)
  }, []);

  const filterFields: DataTableFilterField<GetApiHitboxesByHash200Response>[] = [
    {
      label: "Title",
      value: "hash",
      placeholder: "Search by hash"
    },
  ]

  const {table} = useDataTable({
    data: hitboxes,
    setData: setHitboxes,
    columns: hitboxColumns,
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
            <CardTitle>Hitboxes</CardTitle>
            <CardDescription>All hitbox info associated with this unit.</CardDescription>
          </CardHeader>
          <CardContent>
            <div className="space-y-4">
              <DataTable table={table}>
                <DataTableToolbar table={table} filterFields={filterFields}>
                  <HitboxDataTableToolbar table={table}/>
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

export default Hitboxes;