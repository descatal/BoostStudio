import UpsertStatsGroupDialog from "@/features/stats/components/dialogs/upsert-stats-group-dialog";
import { PlusIcon } from "lucide-react";
import { EnhancedButton } from "@/components/ui/enhanced-button.tsx";

type StatsGroupTableToolbarActionsProps = {
  unitId?: number | undefined;
};

export function StatsGroupTableToolbarActions({
  unitId,
}: StatsGroupTableToolbarActionsProps) {
  return (
    <div className="flex items-center gap-2">
      <UpsertStatsGroupDialog unitId={unitId}>
        <EnhancedButton
          variant="default"
          effect={"ringHover"}
          size={"sm"}
          icon={PlusIcon}
          iconPlacement={"right"}
        >
          Create
        </EnhancedButton>
      </UpsertStatsGroupDialog>
    </div>
  );
}
