import React, { useCallback, useEffect, useState } from "react";
import {
  fetchProjectiles,
  updateProjectile,
} from "@/api/wrapper/projectile-api";
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

import { projectileColumns } from "./components/data-table/projectile-data-table-columns";
import { ProjectileDataTableToolbar } from "./components/data-table/projectile-data-table-toolbar";
import {
  type PaginatedListOfProjectileDto,
  type ProjectileDto,
} from "@/api/exvs";

const Projectiles = ({ unitId }: { unitId: number }) => {
  const [response, setResponse] = useState<PaginatedListOfProjectileDto>();
  const [projectiles, setProjectiles] = useState<ProjectileDto[]>([]);

  const getData = useCallback(async () => {
    const pagination = table.getState().pagination;
    const hash = table
      .getState()
      .columnFilters.find((column) => column.id === "hash")?.value as string;
    const hitboxHash = table
      .getState()
      .columnFilters.find((column) => column.id === "hitboxHash")
      ?.value as string;

    const hashes = hash?.split(";").map((x) => Number(x));
    const projectileDto = await fetchProjectiles({
      unitIds: [unitId],
      page: pagination.pageIndex + 1,
      perPage: pagination.pageSize,
      hashes: hashes,
      search: hitboxHash,
    });
    setResponse(projectileDto);
  }, [unitId]);

  const saveData = useCallback(async () => {
    const modifiedRows = table.options.meta?.modifiedRows ?? [];
    const modifiedEntities = modifiedRows.map((rowIndex, index) => {
      return table.getRow(rowIndex.toString()).original;
    });

    for (const entity of modifiedEntities) {
      if (entity.hash === undefined) continue;
      if (entity.hitboxHash === 0) entity.hitboxHash = undefined;
      await updateProjectile({
        hash: entity.hash!,
        updateProjectileByIdCommand: {
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
    setProjectiles(response?.items ?? []);
  }, [response]);

  useEffect(() => {
    getData().catch(console.error);
  }, [unitId]);

  const filterFields: DataTableFilterField<ProjectileDto>[] = [
    {
      type: "input",
      label: "Hash",
      value: "hash",
      placeholder: "Search by hash",
    },
    {
      type: "input",
      label: "Hitbox Hash",
      value: "hitboxHash",
      placeholder: "Search by hitbox hash",
    },
  ];

  const { table } = useDataTable({
    data: projectiles,
    setData: setProjectiles,
    columns: projectileColumns,
    pageCount: response?.totalPages ?? 0,
    filterFields: filterFields,
    fetchData: getData,
    saveData: saveData,
    enableEditingMode: true,
  });

  return (
    <div>
      {response && (
        <Card>
          <CardHeader className="px-7">
            <CardTitle>Projectiles</CardTitle>
            <CardDescription>
              All projectile info associated with this unit.
            </CardDescription>
          </CardHeader>
          <CardContent>
            <div className="space-y-4">
              <DataTable table={table}>
                <DataTableToolbar table={table} filterFields={filterFields}>
                  <ProjectileDataTableToolbar table={table} />
                </DataTableToolbar>
              </DataTable>
            </div>
          </CardContent>
        </Card>
      )}
    </div>
  );
};

export default Projectiles;
