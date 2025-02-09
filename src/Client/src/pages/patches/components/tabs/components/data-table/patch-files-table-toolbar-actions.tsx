"use client"

import React from "react"
import { PatchFileVm } from "@/api/exvs"
import ResizePatchDialog from "@/features/patches/components/resize-patch-dialog"
import UpsertPatchDialog from "@/features/patches/components/upsert-patch-dialog"
import { type Table } from "@tanstack/react-table"
import { PlusIcon } from "lucide-react"

import { Button } from "@/components/ui/button"
import { DropdownMenuItem } from "@/components/ui/dropdown-menu"

import PatchFileDialog from "../dialog/patch-file-dialog"

interface PatchFilesTableToolbarActionsProps {
  table: Table<PatchFileVm>
}

export function PatchFilesTableToolbarActions({
  table,
}: PatchFilesTableToolbarActionsProps) {
  return (
    <div className="flex items-center gap-2">
      <UpsertPatchDialog
        triggerButton={
          <Button variant="outline" size="sm">
            <PlusIcon className="mr-2 size-4" aria-hidden="true" />
            New patch file entry
          </Button>
        }
      />
    </div>
  )
}
