"use client"

import React from "react"
import { PatchFileVm } from "@/api/exvs"
import ResizePatchDialog from "@/features/patches/components/resize-patch-dialog"
import { type Table } from "@tanstack/react-table"
import { PlusIcon } from "lucide-react"

import { Button } from "@/components/ui/button"

import PatchFileDialog from "../dialog/patch-file-dialog"

interface PatchFilesTableToolbarActionsProps {
  table: Table<PatchFileVm>
}

export function PatchFilesTableToolbarActions({
  table,
}: PatchFilesTableToolbarActionsProps) {
  return (
    <div className="flex items-center gap-2">
      <PatchFileDialog>
        <Button variant="outline" size="sm">
          <PlusIcon className="size-4 mr-2" aria-hidden="true" />
          New patch file entry
        </Button>
      </PatchFileDialog>
    </div>
  )
}
