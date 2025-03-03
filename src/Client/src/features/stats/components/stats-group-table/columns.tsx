"use client";

import { ProjectileDto, StatDto } from "@/api/exvs";
import { ColumnDef } from "@tanstack/react-table";
import StatsGroupTableRowActions from "@/features/stats/components/stats-group-table/row-actions";
import { StatDto as ZodStatDto } from "@/api/exvs/zod";
import { HashInput } from "@/components/custom/hash-input";
import { Link, useParams } from "@tanstack/react-router";
import React from "react";

const customTableRows: ColumnDef<ProjectileDto>[] = [];

const allCustomTableRowKeys = customTableRows.map(
  // @ts-ignore
  (x) => x.accessorKey, // for some reason ts can't get this type
);

export const statsGroupTableColumns: ColumnDef<StatDto>[] = [
  ...customTableRows,
  ...Object.keys(ZodStatDto.shape)
    .filter((key) => !allCustomTableRowKeys.includes(key))
    .map((x) => ({
      accessorKey: x,
      header: x,
    })),
  {
    id: "actions",
    size: 40,
    cell: ({ row }) => {
      if (!row.original.id) return;
      return <StatsGroupTableRowActions data={row.original} />;
    },
  },
];
