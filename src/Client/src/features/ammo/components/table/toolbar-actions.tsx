"use client";

import React from "react";
import UpsertAmmoDialog from "@/features/ammo/components/dialogs/upsert";
import { EnhancedButton } from "@/components/ui/enhanced-button";
import { PlusIcon } from "lucide-react";

type AmmoTableToolbarActionsProps = {};

export function AmmoTableToolbarActions({}: AmmoTableToolbarActionsProps) {
  return (
    <div className="flex items-center gap-2">
      <UpsertAmmoDialog
        triggerButton={
          <EnhancedButton
            variant="default"
            effect={"ringHover"}
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
