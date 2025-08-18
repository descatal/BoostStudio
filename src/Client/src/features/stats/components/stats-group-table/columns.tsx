import { ProjectileDto, StatDto } from "@/api/exvs";
import { ColumnDef } from "@tanstack/react-table";
import StatsGroupTableRowActions from "@/features/stats/components/stats-group-table/row-actions";
import { zStatDto } from "@/api/exvs/zod.gen";

const customTableRows: ColumnDef<ProjectileDto>[] = [
  // {
  //   id: "unitId",
  //   accessorKey: "unitId",
  //   meta: {
  //     label: "Unit",
  //     variant: "multiSelect",
  //   },
  //   enableColumnFilter: true,
  // },
];

const allCustomTableRowKeys = customTableRows.map(
  // @ts-ignore
  (x) => x.accessorKey, // for some reason ts can't get this type
);

export const statsGroupTableColumns: ColumnDef<StatDto>[] = [
  ...customTableRows,
  ...Object.keys(zStatDto.shape)
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
