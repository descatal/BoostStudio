"use client";

import UpsertHitboxDialog from "@/features/hitboxes/components/dialogs/upsert";
import { PlusIcon } from "lucide-react";
import { EnhancedButton } from "@/components/ui/enhanced-button";

type HitboxesTableToolbarActionsProps = {};

export function HitboxesTableToolbarActions({}: HitboxesTableToolbarActionsProps) {
  return (
    <div className="flex items-center gap-2">
      <UpsertHitboxDialog
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
