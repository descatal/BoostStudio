import React from "react";
import {
  Sheet,
  SheetClose,
  SheetContent,
  SheetDescription,
  SheetFooter,
  SheetHeader,
  SheetTitle,
  SheetTrigger,
} from "@/components/ui/sheet";
import { CreateStatCommand, StatDto } from "@/api/exvs";
import { useMediaQuery } from "@/hooks/use-media-query";
import { Separator } from "@/components/ui/separator";
import { Button } from "@/components/ui/button";
import { Loader } from "lucide-react";
import {
  Drawer,
  DrawerClose,
  DrawerContent,
  DrawerDescription,
  DrawerFooter,
  DrawerHeader,
  DrawerTitle,
  DrawerTrigger,
} from "@/components/ui/drawer";
import StatsGroupForm from "@/features/stats/components/stats-group-form";
import { useUpdateStatsMutation } from "@/features/stats/api/update-stats";
import { UpdateStatCommand } from "@/api/exvs/zod";
import { useCreateStatsMutation } from "@/features/stats/api/create-stats";

interface UpsertStatsGroupDialogProps
  extends React.ComponentPropsWithRef<typeof Sheet> {
  data?: StatDto | undefined;
  triggerButton?: React.ReactElement | undefined;
}

const UpsertStatsGroupDialog = ({
  data,
  triggerButton,
  ...props
}: UpsertStatsGroupDialogProps) => {
  // put this into global state
  const [open, setOpen] = React.useState(false);

  const createMutation = useCreateStatsMutation({
    onSuccess: () => setOpen(false),
  });
  const updateMutation = useUpdateStatsMutation({
    onSuccess: () => setOpen(false),
  });

  const isPending = createMutation.isPending || updateMutation.isPending;

  const handleSubmit = (upsertData: CreateStatCommand | UpdateStatCommand) => {
    data
      ? updateMutation.mutate(upsertData as UpdateStatCommand)
      : createMutation.mutate(upsertData as CreateStatCommand);
  };

  const isDesktop = useMediaQuery("(min-width: 640px)");

  if (isDesktop) {
    return (
      <Sheet open={open} onOpenChange={setOpen} {...props}>
        <SheetTrigger asChild>{triggerButton ?? <></>}</SheetTrigger>
        <SheetContent className="flex flex-col gap-6 sm:max-w-md">
          <SheetHeader className="text-left">
            <SheetTitle>{data ? "Update" : "Create"}</SheetTitle>
            <SheetDescription>
              {data ? "Update existing" : "Create new"} stats group details and
              save the changes
            </SheetDescription>
          </SheetHeader>
          <Separator />
          <StatsGroupForm data={data} onSubmit={handleSubmit}>
            <SheetFooter className="gap-2 pt-2 sm:space-x-0">
              <SheetClose asChild>
                <Button type="button" variant="outline">
                  Cancel
                </Button>
              </SheetClose>
              <Button disabled={isPending}>
                {isPending && (
                  <Loader
                    className="mr-2 size-4 animate-spin"
                    aria-hidden="true"
                  />
                )}
                Save
              </Button>
            </SheetFooter>
          </StatsGroupForm>
        </SheetContent>
      </Sheet>
    );
  }

  return (
    <Drawer open={open} onOpenChange={setOpen} {...props}>
      <DrawerTrigger asChild autoFocus={props.open}>
        {triggerButton ?? <></>}
      </DrawerTrigger>
      <DrawerContent className="flex flex-col gap-6 sm:max-w-md h-full">
        <DrawerHeader className="text-left">
          <DrawerTitle>Update stats group details</DrawerTitle>
          <DrawerDescription>
            Update the stats group details and save the changes
          </DrawerDescription>
        </DrawerHeader>
        <Separator />
        <StatsGroupForm data={data} onSubmit={handleSubmit}>
          <DrawerFooter className="gap-2 pt-2 sm:space-x-0">
            <DrawerClose asChild>
              <Button type="button" variant="outline">
                Cancel
              </Button>
            </DrawerClose>
            <Button disabled={isPending}>
              {isPending && (
                <Loader
                  className="mr-2 size-4 animate-spin"
                  aria-hidden="true"
                />
              )}
              Save
            </Button>
          </DrawerFooter>
        </StatsGroupForm>
      </DrawerContent>
    </Drawer>
  );
};

export default UpsertStatsGroupDialog;
