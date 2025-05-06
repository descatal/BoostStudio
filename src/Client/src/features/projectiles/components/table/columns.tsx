import { ColumnDef } from "@tanstack/react-table";
import React from "react";
import { HashInput } from "@/components/custom/hash-input";
import { ProjectileDto } from "@/api/exvs";
import ProjectilesTableRowActions from "@/features/projectiles/components/table/row-actions";
import { Link } from "@tanstack/react-router";
import { zProjectileDto } from "@/api/exvs/zod.gen";

const customTableRows: ColumnDef<ProjectileDto>[] = [
  {
    id: "hash",
    accessorKey: "hash",
    cell: ({ row }) => (
      <HashInput
        className={"border-none"}
        initialValue={row.original.hash}
        readonly={true}
        initialMode={"hex"}
      />
    ),
    meta: {
      label: "Hash",
      variant: "text",
      placeholder: "Search by Hash (Hex)",
    },
    enableColumnFilter: true,
  },
  {
    id: "unitId",
    accessorKey: "unitId",
    meta: {
      label: "Unit",
      variant: "multiSelect",
    },
    enableColumnFilter: true,
  },
  {
    id: "hitboxHash",
    accessorKey: "hitboxHash",
    cell: ({ row }) => {
      return (
        <>
          <Link
            to={"/units/info/hitboxes"}
            search={{
              hash: row.original.hitboxHash?.toString(16).toUpperCase(),
            }}
          >
            <HashInput
              className={"border-none"}
              initialValue={row.original.hitboxHash}
              readonly={true}
              initialMode={"hex"}
            />
          </Link>
        </>
      );
    },
  },
];

const allCustomTableRowKeys = customTableRows.map(
  // @ts-ignore
  (x) => x.accessorKey, // for some reason ts can't get this type
);

export const projectileTableColumns: ColumnDef<ProjectileDto>[] = [
  ...customTableRows,
  // map everything else
  ...Object.keys(zProjectileDto.shape)
    .filter((key) => !allCustomTableRowKeys.includes(key))
    .map((x) => ({
      accessorKey: x,
      header: x,
    })),
  {
    id: "actions",
    size: 40,
    cell: ({ row }) => {
      if (!row.original.hash) return;
      return <ProjectilesTableRowActions data={row.original} />;
    },
  },
];
