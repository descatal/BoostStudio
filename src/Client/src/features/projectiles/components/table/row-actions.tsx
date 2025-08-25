import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { Button } from "@/components/ui/button";
import { DotsHorizontalIcon } from "@radix-ui/react-icons";
import { ProjectileDto } from "@/api/exvs";
import UpsertProjectileDialog from "@/features/projectiles/components/dialogs/upsert";

type ProjectilesTableRowActionsProps = {
  data: ProjectileDto;
};

const ProjectilesTableRowActions = ({
  data,
}: ProjectilesTableRowActionsProps) => {
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
            <UpsertProjectileDialog existingData={data}>
              <DropdownMenuItem onSelect={(e) => e.preventDefault()}>
                Edit
              </DropdownMenuItem>
            </UpsertProjectileDialog>
          )}
        </DropdownMenuContent>
      </DropdownMenu>
    </>
  );
};

export default ProjectilesTableRowActions;
