import { CellContext, ColumnDef } from "@tanstack/react-table";
import { AmmoDto, HitboxDto } from "@/api/exvs";
import { AmmoDto as ZodAmmoDto } from "@/api/exvs/zod";
import AmmoTableRowActions from "@/features/ammo/components/ammo-table/row-actions";
import { HashInput } from "@/components/custom/hash-input";
import React from "react";

const customTableRows: ColumnDef<AmmoDto>[] = [
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
];

const allCustomTableRowKeys = customTableRows.map(
  // @ts-ignore
  (x) => x.accessorKey, // for some reason ts can't get this type
);

export const ammoTableColumns: ColumnDef<AmmoDto>[] = [
  ...customTableRows,
  // map everything else
  ...Object.keys(ZodAmmoDto.shape)
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
      return <AmmoTableRowActions data={row.original} />;
    },
  },
];
