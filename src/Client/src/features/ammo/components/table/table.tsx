import { useDataTable } from "@/hooks/use-data-table";
import { DataTable } from "@/components/data-table/data-table";
import { DataTableToolbar } from "@/components/data-table/data-table-toolbar";
import { loadPaginatedAmmoSearchParams } from "@/loaders/ammo-search-params";
import { ammoTableColumns } from "@/features/ammo/components/table/columns";
import { AmmoTableToolbarActions } from "@/features/ammo/components/table/toolbar-actions";
import { useQuery } from "@tanstack/react-query";
import {
  getApiAmmoOptions,
  getApiSeriesUnitsOptions,
} from "@/api/exvs/@tanstack/react-query.gen";

type AmmoTableProps = {
  unitId?: number | undefined;
};

const AmmoTable = ({ unitId }: AmmoTableProps) => {
  const { page, perPage, hashes, unitIds } = loadPaginatedAmmoSearchParams(
    location.search,
  );

  const paginatedQuery = useQuery({
    ...getApiAmmoOptions({
      query: {
        Page: page,
        PerPage: perPage,
        Hash: hashes?.map((x) => parseInt(x, 16)) ?? undefined,
        UnitIds: unitId ? [unitId] : (unitIds ?? undefined),
      },
    }),
  });

  const paginatedData = paginatedQuery?.data;

  const seriesUnitsQuery = useQuery({
    ...getApiSeriesUnitsOptions({
      query: {
        ListAll: true,
      },
    }),
    select: (data) =>
      data?.items
        .filter((vm) => vm.units)
        .flatMap((vm) => vm.units!)
        ?.map((type) => ({
          label: type.nameEnglish ?? "-",
          value: type.unitId!.toString(),
        })) ?? [],
  });

  let unitColumnDef = ammoTableColumns.find((x) => x.id === "unitId");
  if (unitColumnDef?.meta) unitColumnDef.meta.options = seriesUnitsQuery.data;
  if (unitColumnDef && unitId) unitColumnDef.enableColumnFilter = false;

  const { table } = useDataTable({
    data: paginatedData?.items ?? [],
    columns: ammoTableColumns,
    initialState: {
      columnPinning: { right: ["actions"], left: ["hash"] },
      pagination: {
        pageIndex: 1,
        pageSize: 5,
      },
    },
    pageCount: paginatedData?.totalPages ?? 0,
    shallow: false,
  });

  return (
    <DataTable table={table}>
      <DataTableToolbar table={table}>
        <AmmoTableToolbarActions />
      </DataTableToolbar>
    </DataTable>
  );
};

export default AmmoTable;
