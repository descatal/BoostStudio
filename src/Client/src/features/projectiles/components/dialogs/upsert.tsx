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
  CreateProjectileCommand,
  ProjectileDto,
  UpdateProjectileByIdCommand,
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
import ProjectileForm from "@/features/projectiles/components/projectile-form";
import { useMutation } from "@tanstack/react-query";
import {
  postApiProjectilesByHashMutation,
  postApiProjectilesMutation,
} from "@/api/exvs/@tanstack/react-query.gen";

interface UpsertProjectileDialogProps
  extends React.ComponentPropsWithRef<typeof Sheet> {
  data?: ProjectileDto | undefined;
  triggerButton?: React.ReactElement | undefined;
}

const UpsertProjectileDialog = ({
  data,
  triggerButton,
  ...props
}: UpsertProjectileDialogProps) => {
  // put this into global state
  const [open, setOpen] = React.useState(false);

  const createMutation = useMutation({
    ...postApiProjectilesMutation(),
    onSuccess: () => () => setOpen(false),
  });

  const updateMutation = useMutation({
    ...postApiProjectilesByHashMutation(),
    onSuccess: () => () => setOpen(false),
  });

  const isPending = createMutation.isPending || updateMutation.isPending;

  const handleSubmit = (
    upsertData: CreateProjectileCommand | UpdateProjectileByIdCommand,
  ) => {
    data
      ? updateMutation.mutate({
          path: {
            hash: (upsertData as UpdateProjectileByIdCommand).hash,
          },
          body: upsertData as UpdateProjectileByIdCommand,
        })
      : createMutation.mutate({ body: upsertData as CreateProjectileCommand });
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
              {data ? "Update existing" : "Create new"} entry
            </SheetDescription>
          </SheetHeader>
          <Separator />
          <ProjectileForm data={data} onSubmit={handleSubmit}>
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
          </ProjectileForm>
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
        <ProjectileForm data={data} onSubmit={handleSubmit}>
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
        </ProjectileForm>
      </DrawerContent>
    </Drawer>
  );
};

export default UpsertProjectileDialog;
