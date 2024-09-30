"use client"

import * as React from "react"
import { useCallback, useEffect, useState } from "react"
import { UnitDto } from "@/api/exvs"
import { fetchUnits } from "@/api/wrapper/units-api"
import { CaretSortIcon, CheckIcon } from "@radix-ui/react-icons"
import { FaPlus } from "react-icons/fa"
import { GoPlus } from "react-icons/go"

import { cn } from "@/lib/utils"
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar"
import { Button } from "@/components/ui/button"
import {
  Command,
  CommandEmpty,
  CommandGroup,
  CommandInput,
  CommandItem,
  CommandList,
  CommandSeparator,
} from "@/components/ui/command"
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "@/components/ui/popover"

type UnitGroup = {
  label: string
  units: UnitDto[]
}

type PopoverTriggerProps = React.ComponentPropsWithoutRef<typeof PopoverTrigger>

interface UnitSwitcherProps extends PopoverTriggerProps {
  selectedUnits: UnitDto[] | undefined
  setSelectedUnits: (selectedUnit: UnitDto[] | undefined) => void
  multipleSelect?: false
}

export default function UnitSwitcher({
  className,
  selectedUnits,
  setSelectedUnits,
  multipleSelect,
}: UnitSwitcherProps) {
  const [unitGroups, setUnitGroups] = useState<UnitGroup[]>([])

  const [open, setOpen] = React.useState(false)

  const getData = useCallback(async () => {
    let units = await fetchUnits({})
    units = units.sort((a, b) => (a.unitId ?? 0) - (b.unitId ?? 0))
    // const mappedUnits = units.map((item) => {
    //   return {
    //     label: item.name ?? "",
    //     value: item.unitId ?? 0,
    //   }
    // })
    const group: UnitGroup[] = [
      {
        label: "All",
        units: units,
      },
    ]
    setUnitGroups(group)
  }, [])

  useEffect(() => {
    getData().catch(console.error)
  }, [])

  useEffect(() => {
    if (unitGroups.length <= 0) return
    const units = unitGroups[0]?.units?.filter((unit) =>
      selectedUnits?.some((x) => x.unitId === unit.unitId)
    )
    setSelectedUnits(units)
  }, [unitGroups])

  return (
    <Popover open={open} onOpenChange={setOpen}>
      <PopoverTrigger asChild>
        <Button
          variant="outline"
          role="combobox"
          aria-expanded={open}
          aria-label="Select a unit"
          placeholder={"Select a unit"}
          className={cn("w-full justify-between", className)}
        >
          <Avatar className="mr-2 h-5 w-5">
            <AvatarImage
              src={`/avatars/01.png`}
              alt={selectedUnits ? (selectedUnits[0]?.name ?? "") : ""}
            />
            <AvatarFallback>SC</AvatarFallback>
          </Avatar>
          {selectedUnits ? (
            selectedUnits.length <= 1 ? (
              (selectedUnits[0]?.name ?? "Select a unit")
            ) : (
              <div className={"flex flex-row items-center"}>
                {selectedUnits.length >= 2 && (
                  <Avatar className="mr-2 h-5 w-5">
                    <AvatarImage
                      src={`/avatars/01.png`}
                      alt={selectedUnits[1]?.name ?? ""}
                    />
                    <AvatarFallback>SC</AvatarFallback>
                  </Avatar>
                )}
                {selectedUnits.length >= 3 && (
                  <>
                    <Avatar className="mr-2 h-5 w-5 bg-red-400">
                      <AvatarImage
                        src={`/avatars/01.png`}
                        alt={selectedUnits[2]?.name ?? ""}
                      />
                      <AvatarFallback>SC</AvatarFallback>
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
      <PopoverContent className="w-[350px] p-0">
        <Command>
          <CommandList>
            <CommandInput placeholder="Search unit..." />
            <CommandEmpty>No unit found.</CommandEmpty>
            {unitGroups.map((group) => (
              <CommandGroup key={group.label} heading={group.label}>
                {group.units.map((unit) => (
                  <CommandItem
                    key={unit.unitId}
                    onSelect={() => {
                      if (!selectedUnits) selectedUnits = []

                      if (multipleSelect) {
                        // add the newly selected unit to the list
                        // if it is previously selected, remove it
                        const ifExist = selectedUnits.some(
                          (selectedUnit) => selectedUnit.unitId === unit.unitId
                        )

                        if (ifExist) {
                          // deselect the unit
                          const filteredUnits = selectedUnits.filter(
                            (selectedUnit) =>
                              selectedUnit.unitId !== unit.unitId
                          )
                          setSelectedUnits(filteredUnits)
                        } else {
                          setSelectedUnits([...selectedUnits, unit])
                        }
                      } else {
                        setSelectedUnits([unit])
                        setOpen(false)
                      }
                    }}
                    className="text-sm"
                  >
                    <Avatar className="mr-2 h-5 w-5">
                      <AvatarImage
                        src={`/avatars/01.png`}
                        alt={unit.name}
                        className="grayscale"
                      />
                      <AvatarFallback>SC</AvatarFallback>
                    </Avatar>
                    {unit.name}
                    <CheckIcon
                      className={cn(
                        "ml-auto h-4 w-4",
                        selectedUnits?.some(
                          (selectedUnit) => selectedUnit.unitId === unit.unitId
                        )
                          ? "opacity-100"
                          : "opacity-0"
                      )}
                    />
                  </CommandItem>
                ))}
              </CommandGroup>
            ))}
          </CommandList>
          <CommandSeparator />
        </Command>
      </PopoverContent>
    </Popover>
  )
}
