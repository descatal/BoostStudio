"use client";

import * as React from "react";
import { useSeriesUnits } from "@/features/series/api/get-series";
import MultipleSelector, { Option } from "@/components/ui/multiple-selector";
import { cn } from "@/lib/utils";
import { toast } from "sonner";

interface UnitsSelectorProps
  extends React.ComponentPropsWithoutRef<typeof MultipleSelector> {
  defaultValues?: number[];
  multiple?: boolean;
}

export default function UnitsSelector({
  defaultValues = [],
  multiple = false,
  className,
  ...props
}: UnitsSelectorProps) {
  const paginatedSeriesUnits = useSeriesUnits({
    listAll: true,
  });

  const seriesUnits = paginatedSeriesUnits.data?.items;

  const seriesUnitsOptions: Option[] =
    seriesUnits?.flatMap(
      (series) =>
        series.units?.map((unit) => ({
          group: series.nameEnglish ?? "",
          label: unit.nameEnglish ?? "",
          value: unit.unitId!.toString(),
          fixed: defaultValues?.some((x) => x === unit.unitId),
        })) ?? [],
    ) ?? [];

  return (
    <>
      {seriesUnitsOptions.length > 0 && (
        <div className={cn(className ?? "w-full px-10")}>
          <MultipleSelector
            {...props}
            maxSelected={multiple ? Number.MAX_SAFE_INTEGER : 1}
            onMaxSelected={() => {
              toast("Max unit reached");
            }}
            defaultOptions={seriesUnitsOptions}
            placeholder="Select units..."
            emptyIndicator={
              <p className="text-center text-lg leading-10 text-gray-600 dark:text-gray-400">
                No results found.
              </p>
            }
            groupBy="group"
          />
        </div>
      )}
    </>
  );
}
