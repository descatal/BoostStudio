"use client";

import React from "react";
import { PatchFileVm } from "@/api/exvs";
import UpsertPatchDialog from "@/features/patches/components/dialogs/upsert-patch-dialog";
import { type Table } from "@tanstack/react-table";
import { PlusIcon } from "lucide-react";
import { EnhancedButton } from "@/components/ui/enhanced-button";

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
          <EnhancedButton
            variant="default"
            effect={"expandIcon"}
            icon={PlusIcon}
            iconPlacement={"right"}
          >
            Create
          </EnhancedButton>
        }
      />
    </div>
  );
}
