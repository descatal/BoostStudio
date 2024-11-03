"use client"

import React from "react"
import {
  PatchFileVm,
  type PaginatedListOfPatchFileSummaryVmItemsInner,
} from "@/api/exvs"
import { deletePatchFiles } from "@/api/wrapper/tbl-api"
import PatchFileDialog from "@/pages/patches/components/tabs/components/dialog/patch-file-dialog"
import { DotsHorizontalIcon } from "@radix-ui/react-icons"
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
          initialValue={
            row.original.assetFile?.hash ??
            row.original.assetFileHash ??
            undefined
          }
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
          {row.original.assetFile?.fileType ?? "Not Valid"}
        </Badge>
      ),
    },
    {
      accessorKey: "unit",
      header: "Unit",
      cell: ({ row, table }) => {
        const data = row.original

        return (
          <ul>
            {data.assetFile?.units.map((unit, i) => (
              <li key={`${unit.unitId}-${i}`}>{unit.name}</li>
            )) ?? <li>-</li>}
          </ul>
        )
      },
    },
    {
      accessorKey: "filePath",
      header: "File Path",
      cell: ({ row, table }) => (
        <div className="text-wrap">{row.original.pathInfo?.path}</div>
      ),
    },
    {
      id: "actions",
      size: 40,
      cell: ({ row, table }) => {
        const data = row.original
        if (!data.id) return

        const [showEditPatchFileDialog, setShowEditPatchFileDialog] =
          React.useState(false)

        return (
          <>
            <PatchFileDialog
              open={showEditPatchFileDialog}
              onOpenChange={setShowEditPatchFileDialog}
              // @ts-ignore
              existingPatchFile={data}
              onSuccess={async () => {
                await table.options.meta?.fetchData()
              }}
            />
            <AlertDialog>
              <DropdownMenu>
                <DropdownMenuTrigger asChild>
                  <Button
                    aria-label="Open menu"
                    variant="ghost"
                    className="size-8 flex p-0 data-[state=open]:bg-muted"
                  >
                    <DotsHorizontalIcon className="size-4" aria-hidden="true" />
                  </Button>
                </DropdownMenuTrigger>
                <DropdownMenuContent align="end" className="w-40">
                  <DropdownMenuLabel>Actions</DropdownMenuLabel>
                  {data.id ? (
                    <>
                      <DropdownMenuItem
                        onSelect={() => setShowEditPatchFileDialog(true)}
                      >
                        Edit
                      </DropdownMenuItem>
                      <AlertDialogTrigger asChild>
                        <DropdownMenuItem>Delete</DropdownMenuItem>
                      </AlertDialogTrigger>
                    </>
                  ) : (
                    <></>
                  )}
                </DropdownMenuContent>
              </DropdownMenu>
              <AlertDialogContent>
                <AlertDialogHeader>
                  <AlertDialogTitle>Are you sure?</AlertDialogTitle>
                  <AlertDialogDescription>
                    This will permanently delete this entry from the database.
                  </AlertDialogDescription>
                </AlertDialogHeader>
                <AlertDialogFooter>
                  <AlertDialogCancel>Cancel</AlertDialogCancel>
                  <AlertDialogAction
                    onClick={async () => {
                      if (!data.id) return

                      await deletePatchFiles({ id: data.id })
                      await table.options.meta?.fetchData()

                      toast({
                        title: "Entry Deleted!",
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
