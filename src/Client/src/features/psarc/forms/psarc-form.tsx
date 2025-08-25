import { useMutation } from "@tanstack/react-query";
import { EnhancedButton } from "@/components/ui/enhanced-button";
import {
  postApiPsarcPackPatchFilesMutation,
  postApiPsarcUnpackPatchFilesMutation,
} from "@/api/exvs/@tanstack/react-query.gen";
import {
  PackPsarcByPatchFilesCommand,
  PatchFileVersion,
  UnpackPsarcByPatchFilesCommand,
} from "@/api/exvs";
import {
  zPackPsarcByPatchFilesCommand,
  zPatchFileVersion,
  zUnpackPsarcByPatchFilesCommand,
} from "@/api/exvs/zod.gen.ts";
import { Label } from "@/components/ui/label.tsx";
import { Icons } from "@/components/icons.tsx";
import { AutoForm } from "@/components/ui/autoform";
import { ZodProvider } from "@autoform/zod/v4";
import { LuPackage, LuPackageOpen } from "react-icons/lu";
import { FieldConfig } from "@autoform/react";
import { z } from "zod";

interface Props {
  mode: "pack" | "unpack";
  version?: PatchFileVersion | undefined;
}

const PsarcForm = ({ mode, version = zPatchFileVersion.enum.Base }: Props) => {
  const packMutation = useMutation({
    ...postApiPsarcPackPatchFilesMutation(),
  });

  const unpackMutation = useMutation({
    ...postApiPsarcUnpackPatchFilesMutation(),
  });

  const isPending = packMutation.isPending || unpackMutation.isPending;

  const handleSubmit = (
    data: PackPsarcByPatchFilesCommand | UnpackPsarcByPatchFilesCommand,
  ) => {
    mode === "pack"
      ? packMutation.mutate({
          body: data as PackPsarcByPatchFilesCommand,
        })
      : unpackMutation.mutate({ body: data as UnpackPsarcByPatchFilesCommand });
  };

  const formSchema = zPackPsarcByPatchFilesCommand.extend({
    patchFileVersions: z.optional(zPatchFileVersion),
  });

  const schemaProvider = new ZodProvider(formSchema);

  return (
    <AutoForm
      schema={schemaProvider}
      defaultValues={{
        patchFileVersions: version,
      }}
      values={{
        patchFileVersions: version,
      }}
      onSubmit={(submitData) => {
        handleSubmit({
          patchFileVersions: [
            submitData.patchFileVersions ?? zPatchFileVersion.enum.Patch6,
          ],
        });
      }}
      formProps={{
        className: "p-4",
      }}
      uiComponents={{
        FieldWrapper: ({ children, label, error }) => (
          <div className={"py-2"}>
            <Label className={"pb-2"}>{label}</Label>
            {children}
            {error}
          </div>
        ),
        SubmitButton: () => (
          <div className={"flex justify-end pt-5"}>
            <EnhancedButton
              effect={"expandIcon"}
              variant={"default"}
              icon={mode === "pack" ? LuPackage : LuPackageOpen}
              iconPlacement={"right"}
              disabled={isPending}
            >
              {isPending && (
                <Icons.spinner
                  className="size-4 mr-2 animate-spin"
                  aria-hidden="true"
                />
              )}
              {mode === "pack" ? "Pack" : "Unpack"}
            </EnhancedButton>
          </div>
        ),
      }}
      withSubmit
    />
  );
};

export default PsarcForm;
