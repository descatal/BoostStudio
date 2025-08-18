"use client";

import UpsertAmmoDialog from "@/features/ammo/components/dialogs/upsert";
import { EnhancedButton } from "@/components/ui/enhanced-button";
import { PlusIcon } from "lucide-react";

type AmmoTableToolbarActionsProps = {
  unitId?: number | undefined;
};

export function AmmoTableToolbarActions({
  unitId,
}: AmmoTableToolbarActionsProps) {
  return (
    <div className="flex items-center gap-2">
      <UpsertAmmoDialog unitId={unitId}>
        <EnhancedButton
          variant="default"
          effect={"ringHover"}
          size={"sm"}
          icon={PlusIcon}
          iconPlacement={"right"}
        >
          Create
        </EnhancedButton>
      </UpsertAmmoDialog>
    </div>
  );
}
