"use client"

import * as React from "react"
import { Table } from "@tanstack/react-table"

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
import { Button, buttonVariants } from "@/components/ui/button"

interface StatGroupDataTableToolbarProps<TData> {
  table: Table<TData>
}

export function StatGroupDataTableToolbar<TData>({
  table,
}: StatGroupDataTableToolbarProps<TData>) {
  const modifiedRows = table.options.meta?.modifiedRows ?? []

  return (
    <div className="flex items-center justify-between">
      <AlertDialog>
        <AlertDialogTrigger asChild>
          <Button
            className={`${modifiedRows.length > 0 ? "" : "hidden"}`}
            variant={"outline"}
          >
            Save
          </Button>
        </AlertDialogTrigger>
        <AlertDialogContent>
          <AlertDialogHeader>
            <AlertDialogTitle>Save changes?</AlertDialogTitle>
            <AlertDialogDescription>
              This action will modify {modifiedRows.length} rows.
            </AlertDialogDescription>
          </AlertDialogHeader>
          <AlertDialogFooter>
            <AlertDialogCancel>Cancel</AlertDialogCancel>
            <AlertDialogAction
              className={buttonVariants({ variant: "outline" })}
              onClick={async () => await table.options.meta?.saveData()}
            >
              Continue
            </AlertDialogAction>
          </AlertDialogFooter>
        </AlertDialogContent>
      </AlertDialog>
      <Button
        className={`${modifiedRows.length > 0 ? "" : "hidden"}`}
        variant={"secondary"}
        onClick={async () => await table.options.meta?.fetchData()}
      >
        Discard
      </Button>
    </div>
  )
}
