import * as React from "react";
import { useEffect } from "react";
import MultipleSelector, { Option } from "@/components/ui/multiple-selector";
import { cn } from "@/lib/utils";
import { toast } from "sonner";
import { useQuery } from "@tanstack/react-query";
import { getApiSeriesUnitsOptions } from "@/api/exvs/@tanstack/react-query.gen";

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
  const seriesUnitsQuery = useQuery({
    ...getApiSeriesUnitsOptions({
      query: {
        ListAll: true,
      },
    }),
    select: (data): Option[] => {
      return data.items?.flatMap(
        (series) =>
          series.units?.map((unit) => ({
            group: series.nameEnglish ?? "",
            label: unit.nameEnglish ?? "",
            value: unit.unitId!.toString(),
            fixed: defaultValues?.some((x) => x === unit.unitId!),
          })) ?? [],
      );
    },
  });

  const seriesUnitsOptions = seriesUnitsQuery.data ?? [];

  useEffect(() => {
    if (defaultValues?.length <= 0 || !props.onChange) return;

    // if default values comes in, but the props.value isn't in sync, make sure the current state is matched
    const defaultOptions = seriesUnitsOptions.filter((x) =>
      defaultValues?.some((p) => p.toString() === x.value),
    );

    props.onChange(defaultOptions);
  }, [defaultValues, seriesUnitsOptions]);

  return (
    <>
      {seriesUnitsOptions.length > 0 && (
        <div className={cn(className ?? "w-full px-10")}>
          <MultipleSelector
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
            {...props}
          />
        </div>
      )}
    </>
  );
}
