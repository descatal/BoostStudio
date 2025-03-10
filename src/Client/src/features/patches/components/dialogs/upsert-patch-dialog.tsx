import React from "react";
import { PatchFileSummaryVm, PatchFileVersion } from "@/api/exvs";
import { useUpdateTblPatchFile } from "@/features/patches/api/update-tbl-patch-file";
import { PatchFilesForm } from "@/features/patches/components/patch-files-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { Loader } from "lucide-react";
import { useForm } from "react-hook-form";

import { useMediaQuery } from "@/hooks/use-media-query";
import { Button } from "@/components/ui/button";
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
import { Separator } from "@/components/ui/separator";
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
  CreatePatchFileSchema,
  createPatchFileSchema,
  UpdatePatchFileSchema,
  updatePatchFileSchema,
} from "../../libs/validations";
import { PatchIdNameMap } from "@/features/patches/libs/constants";

interface UpsertPatchDialogProps
  extends React.ComponentPropsWithRef<typeof Sheet> {
  patchFile?: PatchFileSummaryVm | undefined;
  triggerButton?: React.ReactElement | undefined;
}

const UpsertPatchDialog = ({
  triggerButton,
  patchFile,
  ...props
}: UpsertPatchDialogProps) => {
  const updateTblPatchFileMutation = useUpdateTblPatchFile({
    mutationConfig: {
      onSuccess: () => {},
    },
  });

  const selectedPatchFileVersion = patchFile?.tblId ?? PatchFileVersion.Base;
  const patchName = PatchIdNameMap[selectedPatchFileVersion];

  const form = useForm<CreatePatchFileSchema | UpdatePatchFileSchema>({
    resolver: patchFile
      ? zodResolver(updatePatchFileSchema)
      : zodResolver(createPatchFileSchema),
    defaultValues: patchFile
      ? patchFile
      : {
          tblId: selectedPatchFileVersion,
          fileInfo: {
            version: selectedPatchFileVersion,
            size1: 0,
            size2: 0,
            size3: 0,
            size4: 0,
          },
          pathInfo: {
            path: `${patchName ?? ""}/`,
            order: undefined,
          },
          assetFileHash: 0,
        },
  });

  const isDesktop = useMediaQuery("(min-width: 640px)");

  const handleFormSubmit = (
    values: CreatePatchFileSchema | UpdatePatchFileSchema,
  ) => {
    if (patchFile) {
      updateTblPatchFileMutation.mutate({
        ...values,
        id: patchFile.id!,
      });
    }
  };

  if (isDesktop) {
    return (
      <Sheet {...props}>
        <SheetTrigger asChild>{triggerButton ?? <></>}</SheetTrigger>
        <SheetContent className="flex flex-col gap-6 sm:max-w-md">
          <SheetHeader className="text-left">
            <SheetTitle>{patchFile ? "Update" : "Create"}</SheetTitle>
            <SheetDescription>
              {patchFile ? "Update existing" : "Create new"} patch file details
              and save the changes
            </SheetDescription>
          </SheetHeader>
          <Separator />
          <PatchFilesForm form={form} onSubmit={handleFormSubmit}>
            <SheetFooter className="gap-2 pt-2 sm:space-x-0">
              <SheetClose asChild>
                <Button type="button" variant="outline">
                  Cancel
                </Button>
              </SheetClose>
              <Button disabled={updateTblPatchFileMutation.isPending}>
                {updateTblPatchFileMutation.isPending && (
                  <Loader
                    className="mr-2 size-4 animate-spin"
                    aria-hidden="true"
                  />
                )}
                Save
              </Button>
            </SheetFooter>
          </PatchFilesForm>
        </SheetContent>
      </Sheet>
    );
  }

  return (
    <Drawer {...props}>
      <DrawerTrigger asChild autoFocus={props.open}>
        {triggerButton ?? <></>}
      </DrawerTrigger>
      <DrawerContent className="flex flex-col gap-6 sm:max-w-md">
        <DrawerHeader className="text-left">
          <DrawerTitle>Update patch file details</DrawerTitle>
          <DrawerDescription>
            Update the patch file details and save the changes
          </DrawerDescription>
        </DrawerHeader>
        <Separator />
        <PatchFilesForm form={form} onSubmit={handleFormSubmit}>
          <DrawerFooter className="gap-2 pt-2 sm:space-x-0">
            <DrawerClose asChild>
              <Button type="button" variant="outline">
                Cancel
              </Button>
            </DrawerClose>
            <Button
              type={"submit"}
              disabled={updateTblPatchFileMutation.isPending}
            >
              {updateTblPatchFileMutation.isPending && (
                <Loader
                  className="mr-2 size-4 animate-spin"
                  aria-hidden="true"
                />
              )}
              Save
            </Button>
          </DrawerFooter>
        </PatchFilesForm>
      </DrawerContent>
    </Drawer>
  );
};

export default UpsertPatchDialog;
