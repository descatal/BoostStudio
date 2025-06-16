import { ColumnDef } from "@tanstack/react-table";
import { HashInput } from "@/components/custom/hash-input";
import HitboxesTableRowActions from "@/features/hitboxes/components/table/row-actions";
import { HitboxDto } from "@/api/exvs";
import { zHitboxDto } from "@/api/exvs/zod.gen";

const customTableRows: ColumnDef<HitboxDto>[] = [
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

export const hitboxTableColumns: ColumnDef<HitboxDto>[] = [
  ...customTableRows,
  // map everything else
  ...Object.keys(zHitboxDto.shape)
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
