"use client";

import { AmmoDto } from "@/api/exvs";
import { createAmmo, deleteAmmo } from "@/api/wrapper/ammo-api";
import { ColumnDef } from "@tanstack/react-table";
import { MoreHorizontal } from "lucide-react";

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
} from "@/components/ui/alert-dialog";
import { Button } from "@/components/ui/button";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { toast } from "@/hooks/use-toast";
import { HashInput } from "@/components/custom/hash-input";

export const ammoColumns: ColumnDef<AmmoDto>[] = [
  {
    accessorKey: "hash",
    header: "Hash",
    cell: ({ row, table }) => (
      <HashInput
        className={"border-none"}
        initialValue={row.original.hash}
        readonly={true}
        initialMode={"hex"}
      />
    ),
  },
  {
    accessorKey: "ammoType",
    header: "AmmoType",
  },
  {
    accessorKey: "maxAmmo",
    header: "MaxAmmo",
  },
  {
    accessorKey: "initialAmmo",
    header: "InitialAmmo",
  },
  {
    accessorKey: "timedDurationFrame",
    header: "TimedDurationFrame",
  },
  {
    accessorKey: "unk16",
    header: "Unk16",
  },
  {
    accessorKey: "reloadType",
    header: "ReloadType",
  },
  {
    accessorKey: "cooldownDurationFrame",
    header: "CooldownDurationFrame",
  },
  {
    accessorKey: "reloadDurationFrame",
    header: "ReloadDurationFrame",
  },
  {
    accessorKey: "assaultBurstReloadDurationFrame",
    header: "AssaultBurstReloadDurationFrame",
  },
  {
    accessorKey: "blastBurstReloadDurationFrame",
    header: "BlastBurstReloadDurationFrame",
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
    accessorKey: "inactiveUnk48",
    header: "InactiveUnk48",
  },
  {
    accessorKey: "inactiveCooldownDurationFrame",
    header: "InactiveCooldownDurationFrame",
  },
  {
    accessorKey: "inactiveReloadDurationFrame",
    header: "InactiveReloadDurationFrame",
  },
  {
    accessorKey: "inactiveAssaultBurstReloadDurationFrame",
    header: "InactiveAssaultBurstReloadDurationFrame",
  },
  {
    accessorKey: "inactiveBlastBurstReloadDurationFrame",
    header: "InactiveBlastBurstReloadDurationFrame",
  },
  {
    accessorKey: "inactiveUnk68",
    header: "InactiveUnk68",
  },
  {
    accessorKey: "inactiveUnk72",
    header: "InactiveUnk72",
  },
  {
    accessorKey: "burstReplenish",
    header: "BurstReplenish",
  },
  {
    accessorKey: "unk80",
    header: "Unk80",
  },
  {
    accessorKey: "unk84",
    header: "Unk84",
  },
  {
    accessorKey: "unk88",
    header: "Unk88",
  },
  {
    accessorKey: "chargeInput",
    header: "ChargeInput",
  },
  {
    accessorKey: "chargeDurationFrame",
    header: "ChargeDurationFrame",
  },
  {
    accessorKey: "assaultBurstChargeDurationFrame",
    header: "AssaultBurstChargeDurationFrame",
  },
  {
    accessorKey: "blastBurstChargeDurationFrame",
    header: "BlastBurstChargeDurationFrame",
  },
  {
    accessorKey: "unk108",
    header: "Unk108",
  },
  {
    accessorKey: "unk112",
    header: "Unk112",
  },
  {
    accessorKey: "releaseChargeLingerDurationFrame",
    header: "ReleaseChargeLingerDurationFrame",
  },
  {
    accessorKey: "maxChargeLevel",
    header: "MaxChargeLevel",
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
    id: "actions",
    meta: {
      isAction: true,
    },
    cell: ({ row, table }) => {
      const data = row.original;

      return (
        <>
          <AlertDialog>
            <DropdownMenu>
              <DropdownMenuTrigger asChild>
                <div>
                  <Button variant="ghost" className="h-8 w-8 p-0">
                    <span className="sr-only">Open menu</span>
                    <MoreHorizontal className="h-4 w-4" />
                  </Button>
                </div>
              </DropdownMenuTrigger>
              <DropdownMenuContent align="end">
                <DropdownMenuLabel>Actions</DropdownMenuLabel>
                <DropdownMenuItem
                  onClick={async () => {
                    await createAmmo({ ...data });
                    await table.options.meta?.fetchData();
                    toast({
                      title: "Ammo Duplicated!",
                    });
                  }}
                >
                  Duplicate
                </DropdownMenuItem>
                {data.hash ? (
                  <AlertDialogTrigger asChild>
                    <DropdownMenuItem>Delete</DropdownMenuItem>
                  </AlertDialogTrigger>
                ) : (
                  <></>
                )}
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
                    await deleteAmmo(data.hash!);
                    await table.options.meta?.fetchData();
                    toast({
                      title: "Ammo Deleted!",
                    });
                  }}
                >
                  Confirm
                </AlertDialogAction>
              </AlertDialogFooter>
            </AlertDialogContent>
          </AlertDialog>
        </>
      );
    },
  },
];
