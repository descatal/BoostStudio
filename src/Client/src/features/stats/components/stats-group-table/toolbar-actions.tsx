"use client";

import React from "react";
import UpsertStatsGroupDialog from "@/features/stats/components/dialogs/upsert-stats-group-dialog";
import { Button } from "@/components/ui/button";

type StatsGroupTableToolbarActionsProps = {};

export function StatsGroupTableToolbarActions({}: StatsGroupTableToolbarActionsProps) {
  return (
    <div className="flex items-center gap-2">
      <UpsertStatsGroupDialog triggerButton={<Button>Create</Button>} />
    </div>
  );
}
