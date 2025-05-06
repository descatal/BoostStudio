import { ColumnDef } from "@tanstack/react-table";
import { AmmoDto } from "@/api/exvs";
import AmmoTableRowActions from "@/features/ammo/components/table/row-actions";
import { HashInput } from "@/components/custom/hash-input";
import React from "react";
import { zAmmoDto } from "@/api/exvs/zod.gen";

const customTableRows: ColumnDef<AmmoDto>[] = [
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
];

const allCustomTableRowKeys = customTableRows.map(
  // @ts-ignore
  (x) => x.accessorKey, // for some reason ts can't get this type
);

export const ammoTableColumns: ColumnDef<AmmoDto>[] = [
  ...customTableRows,
  // map everything else
  ...Object.keys(zAmmoDto.shape)
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
