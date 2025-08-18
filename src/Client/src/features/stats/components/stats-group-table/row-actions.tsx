import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { Button } from "@/components/ui/button";
import { DotsHorizontalIcon } from "@radix-ui/react-icons";
import { StatDto } from "@/api/exvs";
import UpsertStatsGroupDialog from "@/features/stats/components/dialogs/upsert-stats-group-dialog";

type StatsGroupTableRowActionsProps = {
  unitId?: number | undefined;
  data: StatDto;
};

const StatsGroupTableRowActions = ({
  unitId,
  data,
}: StatsGroupTableRowActionsProps) => {
  return (
    <>
      <DropdownMenu>
        <DropdownMenuTrigger asChild>
          <div>
            <Button
              aria-label="Open menu"
              variant="ghost"
              className="flex size-8 p-0 data-[state=open]:bg-muted"
            >
              <DotsHorizontalIcon className="size-4" aria-hidden="true" />
            </Button>
          </div>
        </DropdownMenuTrigger>
        <DropdownMenuContent align="end" className="w-40">
          <DropdownMenuLabel>Actions</DropdownMenuLabel>
          {data.id && (
            <UpsertStatsGroupDialog existingData={data} unitId={unitId}>
              <DropdownMenuItem onSelect={(e) => e.preventDefault()}>
                Edit
              </DropdownMenuItem>
            </UpsertStatsGroupDialog>
          )}
        </DropdownMenuContent>
      </DropdownMenu>
    </>
  );
};

export default StatsGroupTableRowActions;
