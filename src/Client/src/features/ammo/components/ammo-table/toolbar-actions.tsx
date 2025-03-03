"use client";

import React from "react";
import { Button } from "@/components/ui/button";
import UpsertAmmoDialog from "@/features/ammo/components/upsert-ammo-dialog";

type AmmoTableToolbarActionsProps = {};

export function AmmoTableToolbarActions({}: AmmoTableToolbarActionsProps) {
  return (
    <div className="flex items-center gap-2">
      <UpsertAmmoDialog triggerButton={<Button>Create</Button>} />
    </div>
  );
}
