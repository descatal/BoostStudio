"use client"

import React from "react"
import { type PatchFileSummaryVm } from "@/api/exvs"
import UpsertPatchDialog from "@/features/patches/components/upsert-patch-dialog"
import { DotsHorizontalIcon } from "@radix-ui/react-icons"
import { ColumnDef } from "@tanstack/react-table"

import { Badge } from "@/components/ui/badge"
import { Button } from "@/components/ui/button"
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu"
import { HashInput } from "@/components/custom/hash-input"

export const patchFileColumns: ColumnDef<PatchFileSummaryVm>[] = [
  {
    accessorKey: "assetFileHash",
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
    cell: ({ row, table }) => {
      return (
        <Badge className={"justify-center"}>
          {row.original.assetFile?.fileType ?? "Not Valid"}
        </Badge>
      )
    },
  },
  {
    accessorKey: "units",
    header: "Units",
    cell: ({ row, table }) => {
      if (
        !row.original.assetFile?.units ||
        row.original.assetFile?.units.length === 0
      )
        return (
          <Badge className={"w-fit"} variant={"outline"}>
            -
          </Badge>
        )

      return (
        <div className={"flex flex-col gap-2"}>
          {row.original.assetFile.units.map((unit, i) => (
            <Badge
              className={"w-fit"}
              variant={"outline"}
              key={`${unit.unitId}-${i}`}
            >
              {unit.nameEnglish}
            </Badge>
          ))}
        </div>
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
          <DropdownMenu>
            <DropdownMenuTrigger asChild>
              <Button
                aria-label="Open menu"
                variant="ghost"
                className="flex size-8 p-0 data-[state=open]:bg-muted"
              >
                <DotsHorizontalIcon className="size-4" aria-hidden="true" />
              </Button>
            </DropdownMenuTrigger>
            <DropdownMenuContent align="end" className="w-40">
              <DropdownMenuLabel>Actions</DropdownMenuLabel>
              {data.id ? (
                <>
                  <UpsertPatchDialog
                    triggerButton={
                      <DropdownMenuItem onSelect={(e) => e.preventDefault()}>
                        Edit
                      </DropdownMenuItem>
                    }
                    patchFile={data}
                  />
                  <DropdownMenuItem>Delete</DropdownMenuItem>
                </>
              ) : (
                <></>
              )}
            </DropdownMenuContent>
          </DropdownMenu>
        </>
      )
    },
  },
]
