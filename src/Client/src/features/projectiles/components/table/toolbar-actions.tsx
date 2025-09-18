"use client";

import UpsertProjectileDialog from "@/features/projectiles/components/dialogs/upsert";
import { PlusIcon } from "lucide-react";
import { EnhancedButton } from "@/components/ui/enhanced-button";

interface Props {
  unitId?: number | undefined;
}

export function ProjectilesTableToolbarActions({ unitId }: Props) {
  return (
    <div className="flex items-center gap-2">
      <UpsertProjectileDialog unitId={unitId}>
        <EnhancedButton
          variant="default"
          effect={"expandIcon"}
          size={"sm"}
          icon={PlusIcon}
          iconPlacement={"right"}
        >
          Create
        </EnhancedButton>
      </UpsertProjectileDialog>
    </div>
  );
}
