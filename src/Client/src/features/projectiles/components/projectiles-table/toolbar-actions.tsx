"use client";

import React from "react";
import { Button } from "@/components/ui/button";
import UpsertProjectileDialog from "@/features/projectiles/components/upsert-projectile-dialog";

interface ProjectilesTableToolbarActionsProps {}

export function ProjectilesTableToolbarActions({}: ProjectilesTableToolbarActionsProps) {
  return (
    <div className="flex items-center gap-2">
      <UpsertProjectileDialog triggerButton={<Button>Create</Button>} />
    </div>
  );
}
