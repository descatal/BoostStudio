"use client";

import * as React from "react";
import { BASE_PATH, UnitSummaryVm } from "@/api/exvs";
import { useApiSeriesUnits } from "@/features/series/api/get-series";
import { CaretSortIcon } from "@radix-ui/react-icons";
import { GoPlus } from "react-icons/go";

import { cn } from "@/lib/utils";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import { Button } from "@/components/ui/button";
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "@/components/ui/popover";
import VirtualizedCommand from "@/components/virtualized-command";

// type UnitGroup = {
//   id: number
//   label: string
//   units: UnitSummaryVm[]
// }

type PopoverTriggerProps = React.ComponentPropsWithoutRef<
  typeof PopoverTrigger
>;

interface UnitSwitcherProps extends PopoverTriggerProps {
  selectedUnits: UnitSummaryVm[] | undefined;
  setSelectedUnits: (selectedUnits: UnitSummaryVm[] | undefined) => void;
  multipleSelect?: boolean | undefined;
}

export default function SeriesUnitsSelector({
  className,
  selectedUnits,
  setSelectedUnits,
  multipleSelect = false,
  ...props
}: UnitSwitcherProps) {
  const [open, setOpen] = React.useState(false);

  const seriesUnitsQuery = useApiSeriesUnits();
  const seriesUnits =
    seriesUnitsQuery.data?.items
      .filter((x) => x.units)
      .flatMap((x) => x.units!) ?? [];

  return (
    <Popover open={open} onOpenChange={setOpen}>
      <PopoverTrigger {...props} asChild>
        <Button
          variant="outline"
          role="combobox"
          aria-expanded={open}
          aria-label="Select a unit"
          // placeholder={"Select a unit"}
          className={cn("w-full justify-between", className)}
        >
          <Avatar className="mr-2 h-5 w-5">
            <AvatarImage
              src={
                selectedUnits
                  ? `${BASE_PATH}/assets/${selectedUnits[0]?.unitId ?? "default"}.png`
                  : ""
              }
              alt={selectedUnits ? (selectedUnits[0]?.nameEnglish ?? "") : ""}
            />
            <AvatarFallback>
              {selectedUnits
                ? (selectedUnits[0]?.nameEnglish?.charAt(0) ?? "G")
                : "G"}
            </AvatarFallback>
          </Avatar>
          {selectedUnits ? (
            selectedUnits.length <= 1 ? (
              (selectedUnits[0]?.nameEnglish ?? "Select a unit")
            ) : (
              <div className={"flex flex-row items-center"}>
                {selectedUnits.length >= 2 && (
                  <Avatar className="mr-2 h-5 w-5">
                    <AvatarImage
                      src={`${BASE_PATH}/assets/${selectedUnits[1]?.unitId}.png`}
                      alt={selectedUnits[1]?.nameEnglish ?? ""}
                    />
                    <AvatarFallback>
                      {selectedUnits
                        ? (selectedUnits[1]?.nameEnglish?.charAt(0) ?? "G")
                        : "G"}
                    </AvatarFallback>
                  </Avatar>
                )}
                {selectedUnits.length >= 3 && (
                  <>
                    <Avatar className="mr-2 h-5 w-5 bg-red-400">
                      <AvatarImage
                        src={`${BASE_PATH}/assets/${selectedUnits[2]?.unitId}.png`}
                        alt={selectedUnits[2]?.nameEnglish ?? ""}
                      />
                      <AvatarFallback>
                        {selectedUnits
                          ? (selectedUnits[2]?.nameEnglish?.charAt(0) ?? "G")
                          : "G"}
                      </AvatarFallback>
                    </Avatar>
                    {selectedUnits.length > 3 && <GoPlus />}
                  </>
                )}
              </div>
            )
          ) : (
            "Select a unit"
          )}
          <CaretSortIcon className="ml-auto h-4 w-4 shrink-0 opacity-50" />
        </Button>
      </PopoverTrigger>
      <PopoverContent className="p-0">
        <VirtualizedCommand
          height={"450px"}
          options={
            seriesUnits?.map((option) => ({
              value: option.unitId!.toString(),
              label: option.nameEnglish ?? "",
              imageSrc: `${BASE_PATH}/assets/${option.unitId ?? "default"}.png`,
            })) ?? []
          }
          placeholder={"Search units..."}
          selectedOptions={
            selectedUnits
              ?.filter((option) => option.unitId)
              .map((option) => option.unitId!.toString()) ?? []
          }
          onSelectOptions={(options) => {
            const units = seriesUnits?.filter((x) =>
              options.some((option) => option === x.unitId?.toString()),
            );
            setSelectedUnits(units);

            if (!multipleSelect) setOpen(false);
          }}
          multipleSelect={multipleSelect}
        />
        {/*<Command*/}
        {/*  shouldFilter={false}*/}
        {/*  onKeyDown={(event) => {*/}
        {/*    if (event.key === "ArrowDown" || event.key === "ArrowUp") {*/}
        {/*      event.preventDefault()*/}
        {/*    }*/}
        {/*  }}*/}
        {/*>*/}
        {/*  <CommandList>*/}
        {/*    <CommandInput placeholder="Search unit..." />*/}
        {/*    <CommandEmpty>No unit found.</CommandEmpty>*/}
        {/*    {unitGroups.map((group) => (*/}
        {/*      <CommandGroup*/}
        {/*        ref={parentRef}*/}
        {/*        key={group.label}*/}
        {/*        heading={group.label}*/}
        {/*        style={{*/}
        {/*          height: "400px",*/}
        {/*          width: "100%",*/}
        {/*          overflow: "auto",*/}
        {/*        }}*/}
        {/*      >*/}
        {/*        <div*/}
        {/*          style={{*/}
        {/*            height: `${virtualizer.getTotalSize()}px`,*/}
        {/*            width: "100%",*/}
        {/*            position: "relative",*/}
        {/*          }}*/}
        {/*        >*/}
        {/*          {virtualOptions.map((virtualOption) => (*/}
        {/*            <CommandItem*/}
        {/*              style={{*/}
        {/*                position: "absolute",*/}
        {/*                top: 0,*/}
        {/*                left: 0,*/}
        {/*                width: "100%",*/}
        {/*                height: `${virtualOption.size}px`,*/}
        {/*                transform: `translateY(${virtualOption.start}px)`,*/}
        {/*              }}*/}
        {/*              key={group.units[virtualOption.index].unitId}*/}
        {/*              onSelect={() => {*/}
        {/*                const unit = group.units[virtualOption.index]*/}

        {/*                if (!selectedUnits) selectedUnits = []*/}

        {/*                if (multipleSelect) {*/}
        {/*                  // add the newly selected unit to the list*/}
        {/*                  // if it is previously selected, remove it*/}
        {/*                  const ifExist = selectedUnits.some(*/}
        {/*                    (selectedUnit) =>*/}
        {/*                      selectedUnit.unitId === unit.unitId*/}
        {/*                  )*/}

        {/*                  if (ifExist) {*/}
        {/*                    // deselect the unit*/}
        {/*                    const filteredUnits = selectedUnits.filter(*/}
        {/*                      (selectedUnit) =>*/}
        {/*                        selectedUnit.unitId !== unit.unitId*/}
        {/*                    )*/}
        {/*                    setSelectedUnits(filteredUnits)*/}
        {/*                  } else {*/}
        {/*                    setSelectedUnits([...selectedUnits, unit])*/}
        {/*                  }*/}
        {/*                } else {*/}
        {/*                  setSelectedUnits([unit])*/}
        {/*                  setOpen(false)*/}
        {/*                }*/}
        {/*              }}*/}
        {/*            >*/}
        {/*              <Avatar className="mr-2 h-5 w-5">*/}
        {/*                <AvatarImage*/}
        {/*                  src={`https://localhost:5001/assets/${group.units[virtualOption.index].unitId}.png`}*/}
        {/*                  alt={group.units[virtualOption.index].name}*/}
        {/*                  className="grayscale"*/}
        {/*                />*/}
        {/*                <AvatarFallback>G</AvatarFallback>*/}
        {/*              </Avatar>*/}
        {/*              {group.units[virtualOption.index].name}*/}
        {/*              <CheckIcon*/}
        {/*                className={cn(*/}
        {/*                  "ml-auto h-4 w-4",*/}
        {/*                  selectedUnits?.some(*/}
        {/*                    (selectedUnit) =>*/}
        {/*                      selectedUnit.unitId ===*/}
        {/*                      group.units[virtualOption.index].unitId*/}
        {/*                  )*/}
        {/*                    ? "opacity-100"*/}
        {/*                    : "opacity-0"*/}
        {/*                )}*/}
        {/*              />*/}
        {/*            </CommandItem>*/}
        {/*          ))}*/}
        {/*        </div>*/}
        {/*      </CommandGroup>*/}
        {/*    ))}*/}
        {/*  </CommandList>*/}
        {/*  <CommandSeparator />*/}
        {/*</Command>*/}
      </PopoverContent>
    </Popover>
  );
}
