import * as React from "react";
import MultipleSelector, { Option } from "@/components/ui/multiple-selector";
import { cn } from "@/lib/utils";
import { toast } from "sonner";
import { useQuery } from "@tanstack/react-query";
import { getApiSeriesUnitsOptions } from "@/api/exvs/@tanstack/react-query.gen";
import { useEffect } from "react";

interface UnitsSelectorProps
  extends Omit<
    React.ComponentPropsWithoutRef<typeof MultipleSelector>,
    "onChange" | "value"
  > {
  fixedValues?: number[];
  values?: number[];
  onChange?: (value: number[]) => void;
  multiple?: boolean;
}

export default function UnitsSelector({
  fixedValues,
  values,
  onChange,
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
            fixed: fixedValues?.some((x) => x === unit.unitId!),
          })) ?? [],
      );
    },
  });

  const seriesUnitsOptions = seriesUnitsQuery.data ?? [];

  const [options, setOptions] = React.useState<Option[]>(
    seriesUnitsOptions?.filter((option) => {
      const optionValue = Number(option.value);
      return (
        values?.includes(optionValue) || fixedValues?.includes(optionValue)
      );
    }),
  );

  useEffect(() => {
    // Convert Option[] back to number[] and call onChange
    const numberValues = options.map((option) => Number(option.value));
    onChange?.(numberValues);
  }, [options]);

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
            value={options}
            onChange={setOptions}
            {...props}
          />
        </div>
      )}
    </>
  );
}
