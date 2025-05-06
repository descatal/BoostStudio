import React from "react";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { Button } from "@/components/ui/button";
import { DotsHorizontalIcon } from "@radix-ui/react-icons";
import { HitboxDto } from "@/api/exvs";
import UpsertHitboxDialog from "@/features/hitboxes/components/dialogs/upsert";

type HitboxesTableRowActionsProps = {
  data: HitboxDto;
};

const HitboxesTableRowActions = ({ data }: HitboxesTableRowActionsProps) => {
  return (
    <>
      <DropdownMenu>
        <DropdownMenuTrigger asChild>
          <Button
            aria-label="Open menu"
            variant="ghost"
            className="flex size-8 p-0 data-[state=open]:bg-muted"
          >
            <DotsHorizontalIcon className="size-4" aria-hidden="true" />
          </Button>
        </DropdownMenuTrigger>
        <DropdownMenuContent align="end" className="w-40">
          <DropdownMenuLabel>Actions</DropdownMenuLabel>
          {data.hash && (
            <UpsertHitboxDialog
              data={data}
              triggerButton={
                <DropdownMenuItem onSelect={(e) => e.preventDefault()}>
                  Edit
                </DropdownMenuItem>
              }
            />
          )}
        </DropdownMenuContent>
      </DropdownMenu>
    </>
  );
};

export default HitboxesTableRowActions;
