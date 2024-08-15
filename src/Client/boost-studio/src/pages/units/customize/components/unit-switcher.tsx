"use client"

import * as React from "react"
import { useCallback, useEffect, useState } from "react"
import { fetchUnits } from "@/api/wrapper/units-api"
import { useCustomizeInformationUnitStore } from "@/pages/units/customize/information/libs/store"
import { CaretSortIcon, CheckIcon } from "@radix-ui/react-icons"
import { Link } from "react-router-dom"

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
import { Dialog } from "@/components/ui/dialog"
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "@/components/ui/popover"

type UnitGroup = {
  label: string
  units: {
    label: string
    value: number
  }[]
}

type Unit = UnitGroup[][number]["units"][number]

type PopoverTriggerProps = React.ComponentPropsWithoutRef<typeof PopoverTrigger>

interface TeamSwitcherProps extends PopoverTriggerProps {
  unitId: number
}

export default function UnitSwitcher({ className, unitId }: TeamSwitcherProps) {
  const selectedTab = useCustomizeInformationUnitStore(
    (state) => state.selectedTab
  )
  const [unitGroups, setUnitGroups] = useState<UnitGroup[]>([])

  const [open, setOpen] = React.useState(false)
  const [showNewUnitDialog, setShowNewUnitDialog] = React.useState(false)
  const [selectedUnit, setSelectedUnit] = React.useState<Unit>({
    label: "",
    value: 0,
  })

  const getData = useCallback(async () => {
    let units = await fetchUnits({})
    units = units.sort((a, b) => (a.unitId ?? 0) - (b.unitId ?? 0))
    const mappedUnits = units.map((item) => {
      return {
        label: item.name ?? "",
        value: item.unitId ?? 0,
      }
    })
    const group: UnitGroup[] = [
      {
        label: "All",
        units: mappedUnits,
      },
    ]
    setUnitGroups(group)
  }, [])

  useEffect(() => {
    getData().catch(console.error)
  }, [])

  useEffect(() => {
    if (unitGroups.length <= 0) return
    const selectedUnit = unitGroups[0]?.units?.find(
      (unit) => unit.value === unitId
    )
    setSelectedUnit(
      selectedUnit?.value
        ? selectedUnit
        : {
            label: "",
            value: 0,
          }
    )
  }, [unitGroups])

  return (
    <Dialog open={showNewUnitDialog} onOpenChange={setShowNewUnitDialog}>
      <Popover open={open} onOpenChange={setOpen}>
        <PopoverTrigger asChild>
          <Button
            variant="outline"
            role="combobox"
            aria-expanded={open}
            aria-label="Select a unit"
            className={cn("w-[350px] justify-between", className)}
          >
            <Avatar className="mr-2 h-5 w-5">
              <AvatarImage
                src={`https://avatar.vercel.sh/${selectedUnit.value}.png`}
                alt={selectedUnit.label}
              />
              <AvatarFallback>SC</AvatarFallback>
            </Avatar>
            {selectedUnit.label}
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
                    <Link
                      key={unit.value}
                      to={`/units/${unit.value}/customize/info/${selectedTab}`}
                    >
                      <CommandItem
                        onSelect={() => {
                          setSelectedUnit(unit)
                          setOpen(false)
                        }}
                        className="text-sm"
                      >
                        <Avatar className="mr-2 h-5 w-5">
                          <AvatarImage
                            src={`https://avatar.vercel.sh/${unit.value}.png`}
                            alt={unit.label}
                            className="grayscale"
                          />
                          <AvatarFallback>SC</AvatarFallback>
                        </Avatar>
                        {unit.label}
                        <CheckIcon
                          className={cn(
                            "ml-auto h-4 w-4",
                            selectedUnit.value === unit.value
                              ? "opacity-100"
                              : "opacity-0"
                          )}
                        />
                      </CommandItem>
                    </Link>
                  ))}
                </CommandGroup>
              ))}
            </CommandList>
            <CommandSeparator />
            {/*<CommandList>*/}
            {/*  <CommandGroup>*/}
            {/*    <DialogTrigger asChild>*/}
            {/*      <CommandItem*/}
            {/*        onSelect={() => {*/}
            {/*          setOpen(false)*/}
            {/*          setShowNewTeamDialog(true)*/}
            {/*        }}*/}
            {/*      >*/}
            {/*        <PlusCircledIcon className="mr-2 h-5 w-5" />*/}
            {/*        Create Team*/}
            {/*      </CommandItem>*/}
            {/*    </DialogTrigger>*/}
            {/*  </CommandGroup>*/}
            {/*</CommandList>*/}
          </Command>
        </PopoverContent>
      </Popover>
      {/*<DialogContent>*/}
      {/*  <DialogHeader>*/}
      {/*    <DialogTitle>Create team</DialogTitle>*/}
      {/*    <DialogDescription>*/}
      {/*      Add a new team to manage products and customers.*/}
      {/*    </DialogDescription>*/}
      {/*  </DialogHeader>*/}
      {/*  <div>*/}
      {/*    <div className="space-y-4 py-2 pb-4">*/}
      {/*      <div className="space-y-2">*/}
      {/*        <Label htmlFor="name">Team name</Label>*/}
      {/*        <Input id="name" placeholder="Acme Inc."/>*/}
      {/*      </div>*/}
      {/*      <div className="space-y-2">*/}
      {/*        <Label htmlFor="plan">Subscription plan</Label>*/}
      {/*        <Select>*/}
      {/*          <SelectTrigger>*/}
      {/*            <SelectValue placeholder="Select a plan"/>*/}
      {/*          </SelectTrigger>*/}
      {/*          <SelectContent>*/}
      {/*            <SelectItem value="free">*/}
      {/*              <span className="font-medium">Free</span> -{" "}*/}
      {/*              <span className="text-muted-foreground">*/}
      {/*                Trial for two weeks*/}
      {/*              </span>*/}
      {/*            </SelectItem>*/}
      {/*            <SelectItem value="pro">*/}
      {/*              <span className="font-medium">Pro</span> -{" "}*/}
      {/*              <span className="text-muted-foreground">*/}
      {/*                $9/month per user*/}
      {/*              </span>*/}
      {/*            </SelectItem>*/}
      {/*          </SelectContent>*/}
      {/*        </Select>*/}
      {/*      </div>*/}
      {/*    </div>*/}
      {/*  </div>*/}
      {/*  <DialogFooter>*/}
      {/*    <Button variant="outline" onClick={() => setShowNewUnitDialog(false)}>*/}
      {/*      Cancel*/}
      {/*    </Button>*/}
      {/*    <Button type="submit">Continue</Button>*/}
      {/*  </DialogFooter>*/}
      {/*</DialogContent>*/}
    </Dialog>
  )
}
