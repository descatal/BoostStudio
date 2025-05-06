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
import { AmmoDto, CreateAmmoCommand, UpdateAmmoCommand } from "@/api/exvs";
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
import AmmoForm from "@/features/ammo/components/ammo-form";
import { useMutation } from "@tanstack/react-query";
import {
  postApiAmmoByHashMutation,
  postApiAmmoMutation,
} from "@/api/exvs/@tanstack/react-query.gen";

interface UpsertAmmoDialogProps
  extends React.ComponentPropsWithRef<typeof Sheet> {
  data?: AmmoDto | undefined;
  triggerButton?: React.ReactElement | undefined;
}

const UpsertAmmoDialog = ({
  data,
  triggerButton,
  ...props
}: UpsertAmmoDialogProps) => {
  // put this into global state
  const [open, setOpen] = React.useState(false);

  const createMutation = useMutation({
    ...postApiAmmoMutation(),
    onSuccess: () => () => setOpen(false),
  });

  const updateMutation = useMutation({
    ...postApiAmmoByHashMutation(),
    onSuccess: () => () => setOpen(false),
  });

  const isPending = createMutation.isPending || updateMutation.isPending;

  const handleSubmit = (upsertData: CreateAmmoCommand | UpdateAmmoCommand) => {
    data
      ? updateMutation.mutate({
          path: {
            hash: (upsertData as UpdateAmmoCommand).hash,
          },
          body: upsertData as UpdateAmmoCommand,
        })
      : createMutation.mutate({ body: upsertData as CreateAmmoCommand });
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
              {data ? "Update existing" : "Create new"} ammo entry
            </SheetDescription>
          </SheetHeader>
          <Separator />
          <AmmoForm data={data} onSubmit={handleSubmit}>
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
          </AmmoForm>
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
        <AmmoForm data={data} onSubmit={handleSubmit}>
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
        </AmmoForm>
      </DrawerContent>
    </Drawer>
  );
};

export default UpsertAmmoDialog;
