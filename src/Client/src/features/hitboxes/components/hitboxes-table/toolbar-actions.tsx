"use client";

import React from "react";
import { Button } from "@/components/ui/button";
import UpsertHitboxDialog from "@/features/hitboxes/components/upsert-hitbox-dialog";

type HitboxesTableToolbarActionsProps = {};

export function HitboxesTableToolbarActions({}: HitboxesTableToolbarActionsProps) {
  return (
    <div className="flex items-center gap-2">
      <UpsertHitboxDialog triggerButton={<Button>Create</Button>} />
    </div>
  );
}
