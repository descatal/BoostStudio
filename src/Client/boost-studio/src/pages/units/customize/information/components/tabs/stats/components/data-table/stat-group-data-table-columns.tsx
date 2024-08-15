"use client"

import { GetApiStatsById200Response } from "@/api/exvs/models/GetApiStatsById200Response"
import { createStats, deleteStats } from "@/api/wrapper/stats-api"
import { ColumnDef } from "@tanstack/react-table"
import { MoreHorizontal } from "lucide-react"

import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
  AlertDialogTrigger,
} from "@/components/ui/alert-dialog"
import { Button } from "@/components/ui/button"
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu"

export const statGroupColumns: ColumnDef<GetApiStatsById200Response>[] = [
  {
    accessorKey: "order",
    header: "Group",
    meta: {
      isKey: true,
    },
  },
  {
    accessorKey: "unitCost",
    header: "Unit Cost",
  },
  {
    accessorKey: "unitCost2",
    header: "Unit Cost 2",
  },
  {
    accessorKey: "maxHp",
    header: "Max Hp",
  },
  {
    accessorKey: "downValueThreshold",
    header: "Down Value Threshold",
  },
  {
    accessorKey: "yorukeValueThreshold",
    header: "YorukeValueThreshold",
  },
  {
    accessorKey: "unk20",
    header: "Unk20",
  },
  {
    accessorKey: "unk24",
    header: "Unk24",
  },
  {
    accessorKey: "unk28",
    header: "Unk28",
  },
  {
    accessorKey: "maxBoost",
    header: "Max Boost",
  },
  {
    accessorKey: "unk36",
    header: "Unk36",
  },
  {
    accessorKey: "unk40",
    header: "Unk40",
  },
  {
    accessorKey: "unk44",
    header: "Unk44",
  },
  {
    accessorKey: "gravityMultiplierAir",
    header: "Gravity Multiplier (Air)",
  },
  {
    accessorKey: "gravityMultiplierLand",
    header: "Gravity Multiplier (Land)",
  },
  {
    accessorKey: "unk56",
    header: "Unk56",
  },
  {
    accessorKey: "unk60",
    header: "Unk60",
  },
  {
    accessorKey: "unk64",
    header: "Unk64",
  },
  {
    accessorKey: "unk68",
    header: "Unk68",
  },
  {
    accessorKey: "unk72",
    header: "Unk72",
  },
  {
    accessorKey: "unk76",
    header: "Unk76",
  },
  {
    accessorKey: "unk80",
    header: "Unk80",
  },
  {
    accessorKey: "cameraZoomMultiplier",
    header: "Camera Zoom Multiplier",
  },
  {
    accessorKey: "unk88",
    header: "Unk88",
  },
  {
    accessorKey: "unk92",
    header: "Unk92",
  },
  {
    accessorKey: "unk96",
    header: "Unk96",
  },
  {
    accessorKey: "unk100",
    header: "Unk100",
  },
  {
    accessorKey: "unk104",
    header: "Unk104",
  },
  {
    accessorKey: "unk108",
    header: "Unk108",
  },
  {
    accessorKey: "sizeMultiplier",
    header: "Size Multiplier",
  },
  {
    accessorKey: "unk116",
    header: "Unk116",
  },
  {
    accessorKey: "unk120",
    header: "Unk120",
  },
  {
    accessorKey: "unk124",
    header: "Unk124",
  },
  {
    accessorKey: "unk128",
    header: "Unk128",
  },
  {
    accessorKey: "unk132",
    header: "Unk132",
  },
  {
    accessorKey: "unk136",
    header: "Unk136",
  },
  {
    accessorKey: "unk140",
    header: "Unk140",
  },
  {
    accessorKey: "unk144",
    header: "Unk144",
  },
  {
    accessorKey: "unk148",
    header: "Unk148",
  },
  {
    accessorKey: "unk152",
    header: "Unk152",
  },
  {
    accessorKey: "unk156",
    header: "Unk156",
  },
  {
    accessorKey: "unk160",
    header: "Unk160",
  },
  {
    accessorKey: "unk164",
    header: "Unk164",
  },
  {
    accessorKey: "unk168",
    header: "Unk168",
  },
  {
    accessorKey: "unk172",
    header: "Unk172",
  },
  {
    accessorKey: "unk176",
    header: "Unk176",
  },
  {
    accessorKey: "unk180",
    header: "Unk180",
  },
  {
    accessorKey: "unk184",
    header: "Unk184",
  },
  {
    accessorKey: "redLockRangeMelee",
    header: "Red Lock Range (Melee)",
  },
  {
    accessorKey: "redLockRange",
    header: "Red Lock Range (Shooting)",
  },
  {
    accessorKey: "unk196",
    header: "Unk196",
  },
  {
    accessorKey: "unk200",
    header: "Unk200",
  },
  {
    accessorKey: "unk204",
    header: "Unk204",
  },
  {
    accessorKey: "unk208",
    header: "Unk208",
  },
  {
    accessorKey: "boostReplenish",
    header: "Boost Replenish",
  },
  {
    accessorKey: "unk216",
    header: "Unk216",
  },
  {
    accessorKey: "boostInitialConsumption",
    header: "Boost Initial Consumption",
  },
  {
    accessorKey: "boostFuwaInitialConsumption",
    header: "Boost Fuwa Initial Consumption",
  },
  {
    accessorKey: "boostFlyConsumption",
    header: "Boost Fly Consumption",
  },
  {
    accessorKey: "boostGroundStepInitialConsumption",
    header: "Boost Ground Step Initial Consumption",
  },
  {
    accessorKey: "boostGroundStepConsumption",
    header: "Boost Ground Step Consumption",
  },
  {
    accessorKey: "boostAirStepInitialConsumption",
    header: "Boost Air Step Initial Consumption",
  },
  {
    accessorKey: "boostBdInitialConsumption",
    header: "Boost BD Initial Consumption",
  },
  {
    accessorKey: "boostBdConsumption",
    header: "Boost BD Consumption",
  },
  {
    accessorKey: "unk256",
    header: "Unk256",
  },
  {
    accessorKey: "unk260",
    header: "Unk260",
  },
  {
    accessorKey: "unk264",
    header: "Unk264",
  },
  {
    accessorKey: "unk268",
    header: "Unk268",
  },
  {
    accessorKey: "boostTransformInitialConsumption",
    header: "Boost Transform Initial Consumption",
  },
  {
    accessorKey: "boostTransformConsumption",
    header: "Boost Transform Consumption",
  },
  {
    accessorKey: "boostNonVernierActionConsumption",
    header: "Boost Non-Vernier Action Consumption",
  },
  {
    accessorKey: "boostPostActionConsumption",
    header: "Boost Post Action Consumption",
  },
  {
    accessorKey: "boostRainbowStepInitialConsumption",
    header: "Boost Rainbow Step Initial Consumption",
  },
  {
    accessorKey: "unk292",
    header: "Unk292",
  },
  {
    accessorKey: "unk296",
    header: "Unk296",
  },
  {
    accessorKey: "unk300",
    header: "Unk300",
  },
  {
    accessorKey: "unk304",
    header: "Unk304",
  },
  {
    accessorKey: "unk308",
    header: "Unk308",
  },
  {
    accessorKey: "unk312",
    header: "Unk312",
  },
  {
    accessorKey: "unk316",
    header: "Unk316",
  },
  {
    accessorKey: "unk320",
    header: "Unk320",
  },
  {
    accessorKey: "unk324",
    header: "Unk324",
  },
  {
    accessorKey: "unk328",
    header: "Unk328",
  },
  {
    accessorKey: "unk332",
    header: "Unk332",
  },
  {
    accessorKey: "assaultBurstRedLockMelee",
    header: "Assault Burst Red Lock (Melee)",
  },
  {
    accessorKey: "assaultBurstRedLock",
    header: "Assault Burst Red Lock (Shooting)",
  },
  {
    accessorKey: "assaultBurstDamageDealtMultiplier",
    header: "Assault Burst Damage Dealt Multiplier",
  },
  {
    accessorKey: "assaultBurstDamageTakenMultiplier",
    header: "Assault Burst Damage Taken Multiplier",
  },
  {
    accessorKey: "assaultBurstMobilityMultiplier",
    header: "Assault Burst Mobility Multiplier",
  },
  {
    accessorKey: "assaultBurstDownValueDealtMultiplier",
    header: "Assault Burst Down Value Dealt Multiplier",
  },
  {
    accessorKey: "assaultBurstBoostConsumptionMultiplier",
    header: "Assault Burst Boost Consumption Multiplier",
  },
  {
    accessorKey: "unk364",
    header: "Unk364",
  },
  {
    accessorKey: "assaultBurstDamageDealtBurstGaugeIncreaseMultiplier",
    header: "Assault Burst Damage Dealt Burst Gauge Increase Multiplier",
  },
  {
    accessorKey: "assaultBurstDamageTakenBurstGaugeIncreaseMultiplier",
    header: "Assault Burst Damage Taken Burst Gauge Increase Multiplier",
  },
  {
    accessorKey: "unk380",
    header: "Unk380",
  },
  {
    accessorKey: "unk384",
    header: "Unk384",
  },
  {
    accessorKey: "unk388",
    header: "Unk388",
  },
  {
    accessorKey: "unk392",
    header: "Unk392",
  },
  {
    accessorKey: "unk396",
    header: "Unk396",
  },
  {
    accessorKey: "blastBurstRedLockMelee",
    header: "Blast Burst Red Lock (Melee)",
  },
  {
    accessorKey: "blastBurstRedLock",
    header: "Blast Burst Red Lock (Shooting)",
  },
  {
    accessorKey: "blastBurstDamageDealtMultiplier",
    header: "Blast Burst Damage Dealt Multiplier",
  },
  {
    accessorKey: "blastBurstDamageTakenMultiplier",
    header: "Blast Burst Damage Taken Multiplier",
  },
  {
    accessorKey: "blastBurstMobilityMultiplier",
    header: "Blast Burst Mobility Multiplier",
  },
  {
    accessorKey: "blastBurstDownValueDealtMultiplier",
    header: "Blast Burst Down Value Dealt Multiplier",
  },
  {
    accessorKey: "blastBurstBoostConsumptionMultiplier",
    header: "Blast Burst Boost Consumption Multiplier",
  },
  {
    accessorKey: "unk428",
    header: "Unk428",
  },
  {
    accessorKey: "unk432",
    header: "Unk432",
  },
  {
    accessorKey: "blastBurstDamageDealtBurstGaugeIncreaseMultiplier",
    header: "Blast Burst Damage Dealt Burst Gauge Increase Multiplier",
  },
  {
    accessorKey: "blastBurstDamageTakenBurstGaugeIncreaseMultiplier",
    header: "Blast Burst Damage Taken Burst Gauge Increase Multiplier",
  },
  {
    accessorKey: "unk444",
    header: "Unk444",
  },
  {
    accessorKey: "unk448",
    header: "Unk448",
  },
  {
    accessorKey: "unk452",
    header: "Unk452",
  },
  {
    accessorKey: "unk456",
    header: "Unk456",
  },
  {
    accessorKey: "unk460",
    header: "Unk460",
  },
  {
    accessorKey: "thirdBurstRedLockMelee",
    header: "Third Burst Red Lock (Melee)",
  },
  {
    accessorKey: "thirdBurstRedLock",
    header: "Third Burst Red Lock (Shooting)",
  },
  {
    accessorKey: "thirdBurstDamageDealtMultiplier",
    header: "Third Burst Damage Dealt Multiplier",
  },
  {
    accessorKey: "thirdBurstDamageTakenMultiplier",
    header: "Third Burst Damage Taken Multiplier",
  },
  {
    accessorKey: "thirdBurstMobilityMultiplier",
    header: "Third Burst Mobility Multiplier",
  },
  {
    accessorKey: "thirdBurstDownValueDealtMultiplier",
    header: "Third Burst Down Value Dealt Multiplier",
  },
  {
    accessorKey: "thirdBurstBoostConsumptionMultiplier",
    header: "Third Burst Boost Consumption Multiplier",
  },
  {
    accessorKey: "unk492",
    header: "Unk492",
  },
  {
    accessorKey: "unk496",
    header: "Unk496",
  },
  {
    accessorKey: "thirdBurstDamageDealtBurstGaugeIncreaseMultiplier",
    header: "Third Burst Damage Dealt Burst Gauge Increase Multiplier",
  },
  {
    accessorKey: "thirdBurstDamageTakenBurstGaugeIncreaseMultiplier",
    header: "Third Burst Damage Taken Burst Gauge Increase Multiplier",
  },
  {
    accessorKey: "unk508",
    header: "Unk508",
  },
  {
    accessorKey: "unk512",
    header: "Unk512",
  },
  {
    accessorKey: "unk516",
    header: "Unk516",
  },
  {
    accessorKey: "unk520",
    header: "Unk520",
  },
  {
    accessorKey: "unk524",
    header: "Unk524",
  },
  {
    accessorKey: "fourthBurstRedLockMelee",
    header: "Fourth Burst Red Lock (Melee)",
  },
  {
    accessorKey: "fourthBurstRedLock",
    header: "Fourth Burst Red Lock (Shooting)",
  },
  {
    accessorKey: "fourthBurstDamageDealtMultiplier",
    header: "Fourth Burst Damage Dealt Multiplier",
  },
  {
    accessorKey: "fourthBurstDamageTakenMultiplier",
    header: "Fourth Burst Damage Taken Multiplier",
  },
  {
    accessorKey: "fourthBurstMobilityMultiplier",
    header: "Fourth Burst Mobility Multiplier",
  },
  {
    accessorKey: "fourthBurstDownValueDealtMultiplier",
    header: "Fourth Burst Down Value Dealt Multiplier",
  },
  {
    accessorKey: "fourthBurstBoostConsumptionMultiplier",
    header: "Fourth Burst Boost Consumption Multiplier",
  },
  {
    accessorKey: "unk572",
    header: "Unk572",
  },
  {
    accessorKey: "unk576",
    header: "Unk576",
  },
  {
    accessorKey: "fourthBurstDamageDealtBurstGaugeIncreaseMultiplier",
    header: "Fourth Burst Damage Dealt Burst Gauge Increase Multiplier",
  },
  {
    accessorKey: "fourthBurstDamageTakenBurstGaugeIncreaseMultiplier",
    header: "Fourth Burst Damage Taken Burst Gauge Increase Multiplier",
  },
  {
    accessorKey: "unk588",
    header: "Unk588",
  },
  {
    accessorKey: "unk592",
    header: "Unk592",
  },
  {
    accessorKey: "unk596",
    header: "Unk596",
  },
  {
    accessorKey: "unk600",
    header: "Unk600",
  },
  {
    accessorKey: "unk604",
    header: "Unk604",
  },
  {
    id: "actions",
    meta: {
      isAction: true,
    },
    cell: ({ row, table }) => {
      const data = row.original

      return (
        <>
          <AlertDialog>
            <DropdownMenu>
              <DropdownMenuTrigger asChild>
                <Button variant="ghost" className="h-8 w-8 p-0">
                  <span className="sr-only">Open menu</span>
                  <MoreHorizontal className="h-4 w-4" />
                </Button>
              </DropdownMenuTrigger>
              <DropdownMenuContent align="end">
                <DropdownMenuLabel>Actions</DropdownMenuLabel>
                <DropdownMenuItem
                  onClick={async () => {
                    console.log("Duplicating a new stat...", {
                      ...data,
                      id: undefined,
                    })
                    await createStats({ ...data })
                    await table.options.meta?.fetchData()
                  }}
                >
                  Duplicate
                </DropdownMenuItem>
                <AlertDialogTrigger asChild>
                  <DropdownMenuItem>Delete</DropdownMenuItem>
                </AlertDialogTrigger>
              </DropdownMenuContent>
            </DropdownMenu>
            <AlertDialogContent>
              <AlertDialogHeader>
                <AlertDialogTitle>Are you sure?</AlertDialogTitle>
                <AlertDialogDescription>
                  This action cannot be undone. This will permanently delete
                  this entry from the database.
                </AlertDialogDescription>
              </AlertDialogHeader>
              <AlertDialogFooter>
                <AlertDialogCancel>Cancel</AlertDialogCancel>
                <AlertDialogAction
                  onClick={async () => {
                    console.log("Deleting existing stat with id: ", data.id!)
                    await deleteStats(data.id!)
                    await table.options.meta?.fetchData()
                  }}
                >
                  Confirm
                </AlertDialogAction>
              </AlertDialogFooter>
            </AlertDialogContent>
          </AlertDialog>
        </>
      )
    },
  },
]