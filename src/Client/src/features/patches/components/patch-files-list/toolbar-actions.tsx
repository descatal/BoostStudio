"use client";

import React from "react";
import { PatchFileVm } from "@/api/exvs";
import UpsertPatchDialog from "@/features/patches/components/dialogs/upsert-patch-dialog";
import { type Table } from "@tanstack/react-table";
import { PlusIcon } from "lucide-react";

import { Button } from "@/components/ui/button";

type PatchFilesListToolbarActionsProps = {
  table: Table<PatchFileVm>;
};

export function PatchFilesListToolbarActions({
  table,
}: PatchFilesListToolbarActionsProps) {
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
  );
}
