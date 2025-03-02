"use client";

import { StatDto } from "@/api/exvs";
import { ColumnDef } from "@tanstack/react-table";
import StatsGroupTableRowActions from "@/features/stats/components/stats-group-table/row-actions";
import { StatDto as ZodStatDto } from "@/api/exvs/zod";

export const statsGroupTableColumns: ColumnDef<StatDto>[] = [
  ...Object.keys(ZodStatDto.shape).map((x) => {
    return {
      accessorKey: x,
      header: x,
    };
  }),
  {
    id: "actions",
    size: 40,
    cell: ({ row }) => {
      if (!row.original.id) return;
      return <StatsGroupTableRowActions data={row.original} />;
    },
  },
];
