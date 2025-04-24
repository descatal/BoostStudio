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
import {
  CreateHitboxCommand,
  HitboxDto,
  UpdateHitboxCommand,
} from "@/api/exvs";
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
import { useCreateHitboxes } from "@/features/hitboxes/api/create-hitboxes";
import { useUpdateHitboxes } from "@/features/hitboxes/api/update-hitboxes";
import HitboxForm from "@/features/hitboxes/components/hitbox-form";

interface UpsertHitboxDialogProps
  extends React.ComponentPropsWithRef<typeof Sheet> {
  data?: HitboxDto | undefined;
  triggerButton?: React.ReactElement | undefined;
}

const UpsertHitboxDialog = ({
  data,
  triggerButton,
  ...props
}: UpsertHitboxDialogProps) => {
  // put this into global state
  const [open, setOpen] = React.useState(false);

  const createMutation = useCreateHitboxes({
    mutationConfig: { onSuccess: () => setOpen(false) },
  });
  const updateHitboxes = useUpdateHitboxes({
    mutationConfig: {
      onSuccess: () => setOpen(false),
    },
  });

  const isPending = createMutation.isPending || updateHitboxes.isPending;

  const handleSubmit = (
    upsertData: CreateHitboxCommand | UpdateHitboxCommand,
  ) => {
    data
      ? updateHitboxes.mutate(upsertData as UpdateHitboxCommand)
      : createMutation.mutate(upsertData as CreateHitboxCommand);
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
              {data ? "Update existing" : "Create new"} hitbox entry
            </SheetDescription>
          </SheetHeader>
          <Separator />
          <HitboxForm data={data} onSubmit={handleSubmit}>
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
          </HitboxForm>
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
        <HitboxForm data={data} onSubmit={handleSubmit}>
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
        </HitboxForm>
      </DrawerContent>
    </Drawer>
  );
};

export default UpsertHitboxDialog;
