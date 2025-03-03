import { ColumnDef } from "@tanstack/react-table";
import React from "react";
// import { HitboxTableVm } from "@/features/hitboxes/components/hitboxes-table/hitboxes-table";
import { HashInput } from "@/components/custom/hash-input";
import HitboxesTableRowActions from "@/features/hitboxes/components/hitboxes-table/row-actions";
import { HitboxDto } from "@/api/exvs";
import { HitboxDto as ZodHitboxDto } from "@/api/exvs/zod";

const customTableRows: ColumnDef<HitboxDto>[] = [
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

export const hitboxTableColumns: ColumnDef<HitboxDto>[] = [
  ...customTableRows,
  // map everything else
  ...Object.keys(ZodHitboxDto.shape)
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
      return <HitboxesTableRowActions data={row.original} />;
    },
  },
];
