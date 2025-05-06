import React from "react";
import { useDataTable } from "@/hooks/use-data-table";
import { DataTable } from "@/components/data-table/data-table";
import { DataTableToolbar } from "@/components/data-table/data-table-toolbar";
import { loadPaginatedHitboxesSearchParams } from "@/loaders/hitboxes-search-params";
import { hitboxTableColumns } from "@/features/hitboxes/components/table/columns";
import { HitboxesTableToolbarActions } from "@/features/hitboxes/components/table/toolbar-actions";
import { useQuery } from "@tanstack/react-query";
import { getApiHitboxesOptions } from "@/api/exvs/@tanstack/react-query.gen";

// // intended to be used in this table only, to be cast from HitboxGroupDto
// // the original HitboxDto does not have the context of unitId associated to it, the info can only be found in HitboxGroupDto
// // without the unitIds field it'll be hard to implement filter
// export interface HitboxTableVm extends HitboxDto {
//   unitIds: number[];
// }

interface HitboxesTableProps {
  unitId?: number | undefined;
}

const HitboxesTable = ({ unitId }: HitboxesTableProps) => {
  const { page, perPage, hashes, unitIds } = loadPaginatedHitboxesSearchParams(
    location.search,
  );

  const paginatedQuery = useQuery({
    ...getApiHitboxesOptions({
      query: {
        Page: page,
        PerPage: perPage,
        Hashes: hashes?.map((x) => parseInt(x, 16)) ?? undefined,
        UnitIds: unitId ? [unitId] : (unitIds ?? undefined),
      },
    }),
  });

  const paginatedData = paginatedQuery?.data;

  // const seriesUnitsQuery = useQuery({
  //   ...getApiSeriesUnitsOptions({
  //     query: {
  //       ListAll: true,
  //     },
  //   }),
  //   select: (data) =>
  //     data?.items
  //       .filter((vm) => vm.units)
  //       .flatMap((vm) => vm.units!)
  //       ?.map((type) => ({
  //         label: type.nameEnglish ?? "-",
  //         value: type.unitId!.toString(),
  //       })) ?? [],
  // });
  //
  // let unitColumnDef = hitboxTableColumns.find((x) => x.id === "unitId");
  // if (unitColumnDef?.meta) unitColumnDef.meta.options = seriesUnitsQuery.data;
  // if (unitColumnDef && unitId) unitColumnDef.enableColumnFilter = false;

  const { table } = useDataTable({
    data: paginatedData?.items ?? [],
    columns: hitboxTableColumns,
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
        <HitboxesTableToolbarActions />
      </DataTableToolbar>
    </DataTable>
  );
};

export default HitboxesTable;
