"use client";

import { CellContext, ColumnDef } from "@tanstack/react-table";
import { AmmoDto } from "@/api/exvs";
import { AmmoDto as ZodAmmoDto } from "@/api/exvs/zod";
import AmmoTableRowActions from "@/features/ammo/components/ammo-table/row-actions";
import { HashInput } from "@/components/custom/hash-input";
import React from "react";

export const ammoTableColumns: ColumnDef<AmmoDto>[] = [
  ...Object.keys(ZodAmmoDto.shape).map((x) => {
    if (x === "hash") {
      return {
        accessorKey: "hash",
        header: "hash",
        cell: ({ row }: CellContext<AmmoDto, unknown>) => (
          <HashInput
            className={"border-none"}
            initialValue={row.original.hash}
            readonly={true}
            initialMode={"hex"}
          />
        ),
      };
    }

    return {
      accessorKey: x,
      header: x,
    };
  }),
  {
    id: "actions",
    size: 40,
    cell: ({ row }) => {
      if (!row.original.hash) return;
      return <AmmoTableRowActions data={row.original} />;
    },
  },
];
