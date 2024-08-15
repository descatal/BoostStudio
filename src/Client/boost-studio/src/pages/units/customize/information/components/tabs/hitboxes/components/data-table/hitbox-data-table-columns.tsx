"use client"

import {ColumnDef, RowData} from "@tanstack/react-table"
import {GetApiHitboxesByHash200Response, type GetApiProjectilesByHash200Response} from "@/api/exvs";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuTrigger
} from "@/components/ui/dropdown-menu";
import {Button} from "@/components/ui/button";
import {MoreHorizontal} from "lucide-react";
import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
  AlertDialogTrigger
} from "@/components/ui/alert-dialog";
import {createAmmo, deleteAmmo} from "@/api/wrapper/ammo-api";
import {createHitbox, deleteHitbox} from "@/api/wrapper/hitbox-api";
import {toast} from "@/components/ui/use-toast";
import { Input } from "@/components/ui/input";

export const hitboxColumns: ColumnDef<GetApiHitboxesByHash200Response>[] =
  [
    {
      accessorKey: "hash",
      header: "Hash",
      meta: {
        isKey: true,
      }
    },
    {
      accessorKey: "hitboxType",
      header: "Hitbox Type",
    },
    {
      accessorKey: "damage",
      header: "Damage",
    },
    {
      accessorKey: "unk8",
      header: "Unk8",
    },
    {
      accessorKey: "downValueThreshold",
      header: "Down Value Threshold",
    },
    {
      accessorKey: "yorukeValueThreshold",
      header: "Yoruke Value Threshold",
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
      accessorKey: "damageCorrection",
      header: "Damage Correction",
    },
    {
      accessorKey: "specialEffect",
      header: "Special Effect",
    },
    {
      accessorKey: "hitEffect",
      header: "Hit Effect",
    },
    {
      accessorKey: "flyDirection1",
      header: "Fly Direction 1",
    },
    {
      accessorKey: "flyDirection2",
      header: "Fly Direction 2",
    },
    {
      accessorKey: "flyDirection3",
      header: "Fly Direction 3",
    },
    {
      accessorKey: "enemyCameraShakeMultiplier",
      header: "Enemy Camera Shake Multiplier",
    },
    {
      accessorKey: "playerCameraShakeMultiplier",
      header: "Player Camera Shake Multiplier",
    },
    {
      accessorKey: "unk56",
      header: "Unk56",
    },
    {
      accessorKey: "knockUpAngle",
      header: "Knock Up Angle",
    },
    {
      accessorKey: "knockUpRange",
      header: "Knock Up Range",
    },
    {
      accessorKey: "unk68",
      header: "Unk68",
    },
    {
      accessorKey: "multipleHitIntervalFrame",
      header: "Multiple Hit Interval Frame",
    },
    {
      accessorKey: "multipleHitCount",
      header: "Multiple Hit Count",
    },
    {
      accessorKey: "enemyStunDuration",
      header: "Enemy Stun Duration",
    },
    {
      accessorKey: "playerStunDuration",
      header: "Player Stun Duration",
    },
    {
      accessorKey: "hitVisualEffect",
      header: "Hit Visual Effect",
    },
    {
      accessorKey: "hitVisualEffectSizeMultiplier",
      header: "Hit Visual Effect Size Multiplier",
    },
    {
      accessorKey: "hitSoundEffectHash",
      header: "Hit Sound Effect Hash",
    },
    {
      accessorKey: "unk100",
      header: "Unk100",
    },
    {
      accessorKey: "friendlyDamageFlag",
      header: "Friendly Damage Flag",
    },
    {
      accessorKey: "unk108",
      header: "Unk108",
    },
    {
      accessorKey: "hitboxGroupHash",
      header: "Hitbox Group Hash",
    },
    {
      id: "actions",
      meta: {
        isAction: true,
      },
      cell: ({row, table}) => {
        const data = row.original

        return (
          <>
            <AlertDialog>
              <DropdownMenu>
                <DropdownMenuTrigger asChild>
                  <Button variant="ghost" className="h-8 w-8 p-0">
                    <span className="sr-only">Open menu</span>
                    <MoreHorizontal className="h-4 w-4"/>
                  </Button>
                </DropdownMenuTrigger>
                <DropdownMenuContent align="end">
                  <DropdownMenuLabel>Actions</DropdownMenuLabel>
                  <DropdownMenuItem
                    onClick={async () => {
                      await createHitbox({
                        postApiHitboxesRequest: {...data}
                      })
                      await table.options.meta?.fetchData()
                      toast({
                        title: "Hitbox Duplicated!"
                      })
                    }}
                  >
                    Duplicate
                  </DropdownMenuItem>
                  {data.hash ?
                    <AlertDialogTrigger asChild>
                      <DropdownMenuItem>
                        Delete
                      </DropdownMenuItem>
                    </AlertDialogTrigger> : <></>}
                </DropdownMenuContent>
              </DropdownMenu>
              <AlertDialogContent>
                <AlertDialogHeader>
                  <AlertDialogTitle>Are you sure?</AlertDialogTitle>
                  <AlertDialogDescription>
                    This action cannot be undone. This will permanently delete this entry from the database.
                  </AlertDialogDescription>
                </AlertDialogHeader>
                <AlertDialogFooter>
                  <AlertDialogCancel>Cancel</AlertDialogCancel>
                  <AlertDialogAction
                    onClick={async () => {
                      await deleteHitbox({
                        hash:data.hash!
                      })
                      await table.options.meta?.fetchData()
                      toast({
                        title: "Hitbox Deleted!"
                      })
                    }}>
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