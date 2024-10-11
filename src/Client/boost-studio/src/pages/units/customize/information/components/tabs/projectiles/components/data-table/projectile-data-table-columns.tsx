"use client"

import type { CreateProjectileCommand } from "@/api/exvs"
import { GetApiProjectilesByHash200Response } from "@/api/exvs/models/GetApiProjectilesByHash200Response"
import {
  createProjectile,
  deleteProjectile,
} from "@/api/wrapper/projectile-api"
import { ColumnDef, RowData } from "@tanstack/react-table"
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
import { toast } from "@/components/ui/use-toast"

export const projectileColumns: ColumnDef<GetApiProjectilesByHash200Response>[] =
  [
    {
      accessorKey: "hash",
      header: "Hash",
    },
    {
      accessorKey: "projectileType",
      header: "Projectile Type",
    },
    {
      accessorKey: "hitboxHash",
      header: "Hitbox Hash",
    },
    {
      accessorKey: "modelHash",
      header: "Model Hash",
    },
    {
      accessorKey: "skeletonIndex",
      header: "Skeleton Index",
    },
    {
      accessorKey: "aimType",
      header: "Aim Type",
    },
    {
      accessorKey: "translateY",
      header: "Translate Y",
    },
    {
      accessorKey: "translateZ",
      header: "Translate Z",
    },
    {
      accessorKey: "translateX",
      header: "Translate X",
    },
    {
      accessorKey: "rotateX",
      header: "Rotate X",
    },
    {
      accessorKey: "rotateZ",
      header: "Rotate Z",
    },
    {
      accessorKey: "cosmeticHash",
      header: "Cosmetic Hash",
    },
    {
      accessorKey: "unk44",
      header: "Unk44",
    },
    {
      accessorKey: "unk48",
      header: "Unk 48",
    },
    {
      accessorKey: "unk52",
      header: "Unk 52",
    },
    {
      accessorKey: "unk56",
      header: "Unk 56",
    },
    {
      accessorKey: "ammoConsumption",
      header: "Ammo Consumption",
    },
    {
      accessorKey: "durationFrame",
      header: "Duration Frame",
    },
    {
      accessorKey: "maxTravelDistance",
      header: "MaxTravel Distance",
    },
    {
      accessorKey: "initialSpeed",
      header: "Initial Speed",
    },
    {
      accessorKey: "acceleration",
      header: "Acceleration",
    },
    {
      accessorKey: "accelerationStartFrame",
      header: "Acceleration Start Frame",
    },
    {
      accessorKey: "unk84",
      header: "Unk84",
    },
    {
      accessorKey: "maxSpeed",
      header: "Max Speed",
    },
    {
      accessorKey: "reserved92",
      header: "Reserved92",
    },
    {
      accessorKey: "reserved96",
      header: "Reserved96",
    },
    {
      accessorKey: "reserved100",
      header: "Reserved100",
    },
    {
      accessorKey: "reserved104",
      header: "Reserved104",
    },
    {
      accessorKey: "reserved108",
      header: "Reserved108",
    },
    {
      accessorKey: "reserved112",
      header: "Reserved112",
    },
    {
      accessorKey: "reserved116",
      header: "Reserved116",
    },
    {
      accessorKey: "horizontalGuidance",
      header: "Horizontal Guidance",
    },
    {
      accessorKey: "horizontalGuidanceAngle",
      header: "Horizontal Guidance Angle",
    },
    {
      accessorKey: "verticalGuidance",
      header: "Vertical Guidance",
    },
    {
      accessorKey: "verticalGuidanceAngle",
      header: "Vertical Guidance Angle",
    },
    {
      accessorKey: "reserved136",
      header: "Reserved136",
    },
    {
      accessorKey: "reserved140",
      header: "Reserved140",
    },
    {
      accessorKey: "reserved144",
      header: "Reserved144",
    },
    {
      accessorKey: "reserved148",
      header: "Reserved148",
    },
    {
      accessorKey: "reserved152",
      header: "Reserved152",
    },
    {
      accessorKey: "reserved156",
      header: "Reserved156",
    },
    {
      accessorKey: "reserved160",
      header: "Reserved160",
    },
    {
      accessorKey: "reserved164",
      header: "Reserved164",
    },
    {
      accessorKey: "reserved168",
      header: "Reserved168",
    },
    {
      accessorKey: "reserved172",
      header: "Reserved172",
    },
    {
      accessorKey: "size",
      header: "Size",
    },
    {
      accessorKey: "reserved180",
      header: "Reserved180",
    },
    {
      accessorKey: "reserved184",
      header: "Reserved184",
    },
    {
      accessorKey: "soundEffectHash",
      header: "Sound Effect Hash",
    },
    {
      accessorKey: "reserved192",
      header: "Reserved192",
    },
    {
      accessorKey: "reserved196",
      header: "Reserved196",
    },
    {
      accessorKey: "chainedProjectileHash",
      header: "Chained Projectile Hash",
    },
    {
      accessorKey: "reserved204",
      header: "Reserved204",
    },
    {
      accessorKey: "reserved208",
      header: "Reserved208",
    },
    {
      accessorKey: "reserved212",
      header: "Reserved212",
    },
    {
      accessorKey: "reserved216",
      header: "Reserved216",
    },
    {
      accessorKey: "reserved220",
      header: "Reserved220",
    },
    {
      accessorKey: "reserved224",
      header: "Reserved224",
    },
    {
      accessorKey: "reserved228",
      header: "Reserved228",
    },
    {
      accessorKey: "reserved232",
      header: "Reserved232",
    },
    {
      accessorKey: "reserved236",
      header: "Reserved236",
    },
    {
      accessorKey: "reserved240",
      header: "Reserved240",
    },
    {
      accessorKey: "reserved244",
      header: "Reserved244",
    },
    {
      accessorKey: "reserved248",
      header: "Reserved248",
    },
    {
      accessorKey: "reserved252",
      header: "Reserved252",
    },
    {
      accessorKey: "reserved256",
      header: "Reserved256",
    },
    {
      accessorKey: "reserved260",
      header: "Reserved260",
    },
    {
      accessorKey: "reserved264",
      header: "Reserved264",
    },
    {
      accessorKey: "reserved268",
      header: "Reserved268",
    },
    {
      accessorKey: "reserved272",
      header: "Reserved272",
    },
    {
      accessorKey: "reserved276",
      header: "Reserved276",
    },
    {
      id: "actions",
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
                      await createProjectile({
                        createProjectileCommand: {
                          ...data,
                        },
                      })
                      await table.options.meta?.fetchData()
                      toast({
                        title: "Projectile Duplicated!",
                      })
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
                      await deleteProjectile({
                        hash: data.hash!,
                      })
                      await table.options.meta?.fetchData()
                      toast({
                        title: "Projectile Deleted!",
                      })
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
