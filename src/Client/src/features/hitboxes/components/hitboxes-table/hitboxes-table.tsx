import React from "react";
import { useSeriesUnits } from "@/features/series/api/get-series";
import { DataTableFilterField } from "@/types/index2";
import { HitboxDto } from "@/api/exvs";
import { useDataTable } from "@/hooks/use-react-table-3";
import { DataTable } from "@/components/data-table-2/data-table";
import { DataTableToolbar } from "@/components/data-table-2/data-table-toolbar";
import { loadPaginatedHitboxesSearchParams } from "@/loaders/hitboxes-search-params";
import { hitboxTableColumns } from "@/features/hitboxes/components/hitboxes-table/columns";
import { HitboxesTableToolbarActions } from "@/features/hitboxes/components/hitboxes-table/toolbar-actions";
import { useHitboxes } from "@/features/hitboxes/api/get-hitboxes";

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

  const paginatedData = useHitboxes({
    page: page,
    perPage: perPage,
    hashes: hashes?.map((x) => parseInt(x, 16)) ?? undefined,
    unitIds: unitId ? [unitId] : (unitIds ?? undefined),
  })?.data;

  // const mappedData: HitboxTableVm[] =
  //   paginatedData?.items.flatMap(
  //     (hitboxGroupDto) =>
  //       hitboxGroupDto.hitboxes?.map((hitboxDto) => ({
  //         ...hitboxDto,
  //         unitIds: hitboxGroupDto.unitIds ?? [],
  //       })) ?? [],
  //   ) ?? [];

  const seriesUnits =
    useSeriesUnits()
      ?.data?.items.filter((vm) => vm.units)
      .flatMap((vm) => vm.units!) ?? [];

  const baseFilterFields: DataTableFilterField<HitboxDto>[] = [
    {
      id: "hash",
      label: "Hash",
      placeholder: "Filter by Hash (Hex)",
    },
    {
      // @ts-ignore
      id: "unitIds",
      label: "Units",
      options: seriesUnits.map((type) => ({
        label: type.nameEnglish ?? "-",
        value: type.unitId!.toString(),
      })),
    },
  ];

  const filterFields = baseFilterFields.filter((field) => {
    // @ts-ignore
    if (field.id === "unitIds" && unitId) return false;
    return true;
  });

  const { table } = useDataTable({
    data: paginatedData?.items ?? [],
    columns: hitboxTableColumns,
    filterFields: filterFields,
    initialState: {
      columnPinning: { right: ["actions"], left: ["hash"] },
    },
    pageCount: paginatedData?.totalPages ?? 0,
    shallow: false,
  });

  return (
    <DataTable table={table}>
      <DataTableToolbar table={table} filterFields={filterFields}>
        <HitboxesTableToolbarActions />
      </DataTableToolbar>
    </DataTable>
  );
};

export default HitboxesTable;
