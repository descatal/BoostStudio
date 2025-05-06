import React, { useCallback, useEffect, useState } from "react";
import { GetApiHitboxes200Response } from "@/api/exvs/models/GetApiHitboxes200Response";
import { GetApiHitboxesByHash200Response } from "@/api/exvs/models/GetApiHitboxesByHash200Response";
import { fetchHitboxes, updateHitbox } from "@/api/wrapper/hitbox-api";
import { DataTableFilterField } from "@/types";

import { useDataTable } from "@/hooks/use-react-table-2";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { toast } from "@/hooks/use-toast";
import { DataTable } from "@/components/data-table/data-table";
import { DataTableToolbar } from "@/components/data-table/data-table-toolbar";

import { hitboxColumns } from "./components/data-table/hitbox-data-table-columns";
import { HitboxDataTableToolbar } from "./components/data-table/hitbox-data-table-toolbar";

const Hitboxes = ({ unitId }: { unitId: number }) => {
  const [response, setResponse] = useState<GetApiHitboxes200Response>();
  const [hitboxes, setHitboxes] = useState<GetApiHitboxesByHash200Response[]>(
    [],
  );

  const getData = useCallback(async () => {
    const pagination = table.getState().pagination;
    const hash = table
      .getState()
      .columnFilters.find((column) => column.id === "hash")?.value as string;
    const hashes = hash?.split(";").map((x) => Number(x));
    const getApiHitboxes200Response = await fetchHitboxes({
      unitIds: [unitId],
      page: pagination.pageIndex + 1,
      perPage: pagination.pageSize,
      hashes: hashes,
    });
    setResponse(getApiHitboxes200Response);
  }, [unitId]);

  const saveData = useCallback(async () => {
    const modifiedRows = table.options.meta?.modifiedRows ?? [];
    const modifiedEntities = modifiedRows.map((rowIndex, index) => {
      return table.getRow(rowIndex.toString()).original;
    });

    for (const entity of modifiedEntities) {
      if (!entity.hash) continue;
      await updateHitbox({
        hash: entity.hash!,
        updateHitboxCommand: {
          ...entity,
          hash: entity.hash!,
        },
      });
    }

    toast({
      title: "Saved!",
      description: "Save operation successful.",
    });
    await getData();
  }, []);

  useEffect(() => {
    setHitboxes(response?.items ?? []);
  }, [response]);

  useEffect(() => {
    getData().catch(console.error);
  }, [unitId]);

  const filterFields: DataTableFilterField<GetApiHitboxesByHash200Response>[] =
    [
      {
        type: "input",
        label: "Title",
        value: "hash",
        placeholder: "Search by hash",
      },
    ];

  const { table } = useDataTable({
    data: hitboxes,
    setData: setHitboxes,
    columns: hitboxColumns,
    pageCount: response?.totalPages ?? 0,
    filterFields: filterFields,
    fetchData: getData,
    saveData: saveData,
    enableEditingMode: true,
  });

  return (
    <div>
      {response ? (
        <Card>
          <CardHeader className="px-7">
            <CardTitle>Hitboxes</CardTitle>
            <CardDescription>
              All hitbox info associated with this unit.
            </CardDescription>
          </CardHeader>
          <CardContent>
            <div className="space-y-4">
              <DataTable table={table}>
                <DataTableToolbar table={table} filterFields={filterFields}>
                  <HitboxDataTableToolbar table={table} />
                </DataTableToolbar>
              </DataTable>
            </div>
          </CardContent>
        </Card>
      ) : (
        <></>
      )}
    </div>
  );
};

export default Hitboxes;
