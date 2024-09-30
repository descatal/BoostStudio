"use client"

import { PatchFileVm } from "@/api/exvs"
import { type Table } from "@tanstack/react-table"

import CreatePatchFileDialog from "../dialog/create-patch-file-dialog"

interface PatchFilesTableToolbarActionsProps {
  table: Table<PatchFileVm>
}

export function PatchFilesTableToolbarActions({
  table,
}: PatchFilesTableToolbarActionsProps) {
  return (
    <div className="flex items-center gap-2">
      {/*{table.getFilteredSelectedRowModel().rows.length > 0 ? (*/}
      {/*  <DeleteTasksDialog*/}
      {/*    tasks={table*/}
      {/*      .getFilteredSelectedRowModel()*/}
      {/*      .rows.map((row) => row.original)}*/}
      {/*    onSuccess={() => table.toggleAllRowsSelected(false)}*/}
      {/*  />*/}
      {/*) : null}*/}
      <CreatePatchFileDialog />
      {/*<Button*/}
      {/*  variant="outline"*/}
      {/*  size="sm"*/}
      {/*  onClick={() =>*/}
      {/*    exportTableToCSV(table, {*/}
      {/*      filename: "tasks",*/}
      {/*      excludeColumns: ["select", "actions"],*/}
      {/*    })*/}
      {/*  }*/}
      {/*>*/}
      {/*  <DownloadIcon className="size-4 mr-2" aria-hidden="true" />*/}
      {/*  Export*/}
      {/*</Button>*/}
      {/**
       * Other actions can be added here.
       * For example, import, view, etc.
       */}
    </div>
  )
}
