"use client";

import React from "react";
import UpsertProjectileDialog from "@/features/projectiles/components/dialogs/upsert";
import { PlusIcon } from "lucide-react";
import { EnhancedButton } from "@/components/ui/enhanced-button";

interface ProjectilesTableToolbarActionsProps {}

export function ProjectilesTableToolbarActions({}: ProjectilesTableToolbarActionsProps) {
  return (
    <div className="flex items-center gap-2">
      <UpsertProjectileDialog
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
