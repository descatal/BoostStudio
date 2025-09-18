"use client";

import UpsertHitboxDialog from "@/features/hitboxes/components/dialogs/upsert";
import { PlusIcon } from "lucide-react";
import { EnhancedButton } from "@/components/ui/enhanced-button";

type Props = {
  unitId?: number | undefined;
};

export function HitboxesTableToolbarActions({ unitId }: Props) {
  return (
    <div className="flex items-center gap-2">
      <UpsertHitboxDialog unitId={unitId}>
        <EnhancedButton
          variant="default"
          effect={"expandIcon"}
          size={"sm"}
          icon={PlusIcon}
          iconPlacement={"right"}
        >
          Create
        </EnhancedButton>
      </UpsertHitboxDialog>
    </div>
  );
}
