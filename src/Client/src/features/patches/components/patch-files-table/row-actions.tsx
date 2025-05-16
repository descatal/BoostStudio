import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { Button } from "@/components/ui/button";
import { DotsHorizontalIcon } from "@radix-ui/react-icons";
import UpsertPatchDialog from "@/features/patches/components/dialogs/upsert-patch-dialog";
import { PatchFileSummaryVm } from "@/api/exvs";
import DeletePatchDialog from "@/features/patches/components/dialogs/delete-patch-dialog";

type PatchFilesListRowActionsProps = {
  data: PatchFileSummaryVm;
};

const PatchFilesListRowActions = ({ data }: PatchFilesListRowActionsProps) => {
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
          {data.id ? (
            <>
              <UpsertPatchDialog
                triggerButton={
                  <DropdownMenuItem onSelect={(e) => e.preventDefault()}>
                    Edit
                  </DropdownMenuItem>
                }
                patchFile={data}
              />
              <DeletePatchDialog
                id={data.id}
                triggerButton={
                  <DropdownMenuItem onSelect={(e) => e.preventDefault()}>
                    Delete
                  </DropdownMenuItem>
                }
              ></DeletePatchDialog>
            </>
          ) : (
            <></>
          )}
        </DropdownMenuContent>
      </DropdownMenu>
    </>
  );
};

export default PatchFilesListRowActions;
