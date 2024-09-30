"use client"

import {
  PatchFileVm,
  type PaginatedListOfPatchFileSummaryVmItemsInner,
} from "@/api/exvs"
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
import { Badge } from "@/components/ui/badge"
import { Button } from "@/components/ui/button"
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu"
import { toast } from "@/components/ui/use-toast"
import { HashInput } from "@/components/custom/hash-input"

export const patchFileColumns: ColumnDef<PaginatedListOfPatchFileSummaryVmItemsInner>[] =
  [
    {
      accessorKey: "fileHash",
      header: "File Hash",
      cell: ({ row, table }) => (
        <HashInput
          className={"border-none"}
          initialValue={row.original.assetFile?.hash}
          readonly={true}
          initialMode={"hex"}
        />
      ),
    },
    {
      accessorKey: "fileType",
      header: "File Type",
      cell: ({ row, table }) => (
        <Badge variant="outline" className={"justify-center"}>
          {row.original.assetFile?.fileType}
        </Badge>
      ),
    },
    {
      accessorKey: "unit",
      header: "Unit",
      cell: ({ row, table }) => {
        const data = row.original

        return (
          <>
            {data.assetFile?.units.map((unit) => (
              <div key={unit.unitId}>{unit.name}</div>
            )) ?? <div>-</div>}
          </>
        )
      },
    },
    {
      id: "actions",
      meta: {
        isAction: true,
      },
      size: 40,
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
                      await table.options.meta?.fetchData()
                      toast({
                        title: "Duplication successful!",
                      })
                    }}
                  >
                    Duplicate
                  </DropdownMenuItem>
                  {data.id ? (
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
                      await table.options.meta?.fetchData()
                      toast({
                        title: "Hitbox Deleted!",
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
