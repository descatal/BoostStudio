"use client"

import {Table} from "@tanstack/react-table"

import {Button, buttonVariants} from "@/components/ui/button"
import * as React from "react";
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
} from "@/components/ui/alert-dialog"
import {AmmoDto} from "@/api/exvs";

interface AmmoDataTableToolbarProps<TData> {
  table: Table<TData>
}

export function AmmoDataTableToolbar<TData>(
  {
    table
  }: AmmoDataTableToolbarProps<TData>
) {
  const modifiedRows = table.options.meta?.modifiedRows ?? []

  return (
    <div className="space-x-2">
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
              onClick={async () => {
                await table.options.meta?.saveData()
              }}
            >
              Continue
            </AlertDialogAction>
          </AlertDialogFooter>
        </AlertDialogContent>
      </AlertDialog>
      <Button
        className={`${modifiedRows.length > 0 ? "" : "hidden"}`}
        onClick={async () => {
          await table.options.meta?.fetchData()
        }}
        variant={"destructive"}
      >
        Discard
      </Button>
    </div>
  )
}