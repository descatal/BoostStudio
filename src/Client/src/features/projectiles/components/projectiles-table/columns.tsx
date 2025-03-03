import { ColumnDef } from "@tanstack/react-table";
import React from "react";
import { HashInput } from "@/components/custom/hash-input";
import { ProjectileDto } from "@/api/exvs";
import { ProjectileDto as ZodProjectileDto } from "@/api/exvs/zod";
import ProjectilesTableRowActions from "@/features/projectiles/components/projectiles-table/row-actions";
import { Link } from "@tanstack/react-router";

const customTableRows: ColumnDef<ProjectileDto>[] = [
  {
    accessorKey: "hash",
    header: "hash",
    cell: ({ row }) => (
      <HashInput
        className={"border-none"}
        initialValue={row.original.hash}
        readonly={true}
        initialMode={"hex"}
      />
    ),
  },
  {
    accessorKey: "hitboxHash",
    header: "hitboxHash",
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
  ...Object.keys(ZodProjectileDto.shape)
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
