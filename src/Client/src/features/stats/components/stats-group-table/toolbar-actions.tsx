import UpsertStatsGroupDialog from "@/features/stats/components/dialogs/upsert-stats-group-dialog";
import { Button } from "@/components/ui/button";
import { PlusIcon } from "lucide-react";

type StatsGroupTableToolbarActionsProps = {};

export function StatsGroupTableToolbarActions({}: StatsGroupTableToolbarActionsProps) {
  return (
    <div className="flex items-center gap-2">
      <UpsertStatsGroupDialog
        triggerButton={
          <Button variant="default" size="sm">
            <PlusIcon className="size-2" aria-hidden="true" />
            Create
          </Button>
        }
      />
    </div>
  );
}
