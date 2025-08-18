import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { Button } from "@/components/ui/button";
import { DotsHorizontalIcon } from "@radix-ui/react-icons";
import { AmmoDto } from "@/api/exvs";
import UpsertAmmoDialog from "@/features/ammo/components/dialogs/upsert";

type AmmoTableRowActionsProps = {
  unitId?: number | undefined;
  data: AmmoDto;
};

const AmmoTableRowActions = ({ unitId, data }: AmmoTableRowActionsProps) => {
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
          {data.hash && (
            <UpsertAmmoDialog existingData={data} unitId={unitId}>
              <DropdownMenuItem onSelect={(e) => e.preventDefault()}>
                Edit
              </DropdownMenuItem>
            </UpsertAmmoDialog>
          )}
        </DropdownMenuContent>
      </DropdownMenu>
    </>
  );
};

export default AmmoTableRowActions;
