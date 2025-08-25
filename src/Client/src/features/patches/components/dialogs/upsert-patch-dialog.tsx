import React from "react";
import { PatchFileSummaryVm } from "@/api/exvs";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import { Separator } from "@/components/ui/separator";

import { PatchIdNameMap } from "@/features/patches/libs/constants";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import {
  getApiPatchFilesSummaryQueryKey,
  postApiPatchFilesByIdMutation,
  postApiPatchFilesMutation,
} from "@/api/exvs/@tanstack/react-query.gen";
import {
  zCreatePatchFileCommand,
  zPatchFileVersion,
  zUpdatePatchFileByIdCommand,
} from "@/api/exvs/zod.gen";
import {
  Credenza,
  CredenzaContent,
  CredenzaDescription,
  CredenzaFooter,
  CredenzaHeader,
  CredenzaTitle,
  CredenzaTrigger,
} from "@/components/credenza";
import { EnhancedButton } from "@/components/ui/enhanced-button";
import { BiSave } from "react-icons/bi";
import { PatchFilesForm } from "@/features/patches/components/patch-files-form";
import {
  CreatePatchFileSchema,
  UpdatePatchFileSchema,
} from "@/features/patches/libs/validations";
import { Route } from "@/routes/patches/route";
import { Icons } from "@/components/icons";
import { toast } from "sonner";

interface UpsertPatchDialogProps
  extends Omit<React.ComponentPropsWithRef<typeof Credenza>, "children"> {
  patchFile?: PatchFileSummaryVm | undefined;
  triggerButton?: React.ReactElement | undefined;
}

const UpsertPatchDialog = ({
  triggerButton,
  patchFile,
  ...props
}: UpsertPatchDialogProps) => {
  const [open, setOpen] = React.useState(false);

  const queryClient = useQueryClient();
  const createMutation = useMutation({
    ...postApiPatchFilesMutation(),
    onSuccess: async () => {
      toast("Success!", {
        description: "Entry created.",
      });
      setOpen(false);

      await queryClient.invalidateQueries({
        predicate: (query) =>
          // @ts-ignore
          query.queryKey[0]._id === getApiPatchFilesSummaryQueryKey()[0]._id,
      });
    },
  });

  const updateMutation = useMutation({
    ...postApiPatchFilesByIdMutation(),
    onSuccess: async () => {
      toast("Success!", {
        description: "Entry updated.",
      });
      setOpen(false);

      await queryClient.invalidateQueries({
        predicate: (query) =>
          // @ts-ignore
          query.queryKey[0]._id === getApiPatchFilesSummaryQueryKey()[0]._id,
      });
    },
  });

  const { patchId }: { patchId: string } = Route.useParams();

  const selectedPatchFileVersion =
    patchFile?.tblId ??
    zPatchFileVersion.options.find((x) => x === patchId) ??
    zPatchFileVersion.enum.Base;
  const patchName = PatchIdNameMap[selectedPatchFileVersion];

  const form = useForm<CreatePatchFileSchema | UpdatePatchFileSchema>({
    resolver: patchFile
      ? zodResolver(zUpdatePatchFileByIdCommand)
      : zodResolver(zCreatePatchFileCommand),
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

  const handleFormSubmit = (
    data: CreatePatchFileSchema | UpdatePatchFileSchema,
  ) => {
    if (patchFile) {
      const typedData = data as UpdatePatchFileSchema;
      updateMutation.mutate({
        body: { ...typedData, id: typedData.id! },
        path: {
          id: typedData.id!,
        },
      });
    } else {
      const typedData = data as CreatePatchFileSchema;
      createMutation.mutate({
        body: typedData,
      });
    }
  };

  return (
    <Credenza {...props} open={open} onOpenChange={setOpen}>
      <CredenzaTrigger asChild>{triggerButton ?? <></>}</CredenzaTrigger>
      <CredenzaContent className="flex flex-col gap-6 sm:max-w-md">
        <CredenzaHeader className="text-left">
          <CredenzaTitle>{patchFile ? "Update" : "Create"}</CredenzaTitle>
          <CredenzaDescription>
            {patchFile ? "Update existing" : "Create new"} patch file details
            and save the changes
          </CredenzaDescription>
        </CredenzaHeader>
        <Separator />
        <PatchFilesForm form={form} onSubmit={handleFormSubmit}>
          <CredenzaFooter>
            <EnhancedButton
              className={"mb-2 w-full"}
              effect={"expandIcon"}
              variant={"default"}
              icon={BiSave}
              iconPlacement={"right"}
              disabled={
                patchFile ? updateMutation.isPending : createMutation.isPending
              }
              type={"submit"}
            >
              {(patchFile
                ? updateMutation.isPending
                : createMutation.isPending) && (
                <Icons.spinner
                  className="size-4 mr-2 animate-spin"
                  aria-hidden="true"
                />
              )}
              Save
            </EnhancedButton>
          </CredenzaFooter>
        </PatchFilesForm>
      </CredenzaContent>
    </Credenza>
  );
};

export default UpsertPatchDialog;
