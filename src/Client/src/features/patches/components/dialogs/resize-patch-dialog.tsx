import React from "react";
import ConfirmationDialog from "@/components/custom/confirmation-dialog";
import { PatchIdNameMap } from "@/features/patches/libs/constants";
import { PatchFileVersion } from "@/api/exvs";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import {
  getApiPatchFilesSummaryQueryKey,
  postApiPatchFilesResizeMutation,
} from "@/api/exvs/@tanstack/react-query.gen";
import { EnhancedButton } from "@/components/ui/enhanced-button";
import { CgSize } from "react-icons/cg";
import { Icons } from "@/components/icons";
import { toast } from "sonner";

type ResizeDialogProps = {
  patchId?: PatchFileVersion | undefined;
};

const ResizePatchDialog = ({ patchId }: ResizeDialogProps) => {
  const [open, setOpen] = React.useState(false);

  const queryClient = useQueryClient();
  const mutation = useMutation({
    ...postApiPatchFilesResizeMutation(),
    onSuccess: async () => {
      toast("Success!", {
        description: "Size for all files in this patch file has been updated!",
      });
      setOpen(false);

      await queryClient.invalidateQueries({
        predicate: (query) =>
          // @ts-ignore
          query.queryKey[0]._id === getApiPatchFilesSummaryQueryKey()[0]._id,
      });
    },
  });

  return (
    <ConfirmationDialog
      open={open}
      onOpenChange={setOpen}
      icon="info"
      title="Are you sure?"
      body={`This will update all patch entries' file sizes for ${PatchIdNameMap[patchId ?? "All"]}.`}
      triggerButton={
        <EnhancedButton
          effect={"gooeyLeft"}
          icon={CgSize}
          iconPlacement={"right"}
        >
          Resize
        </EnhancedButton>
      }
      confirmButton={
        <EnhancedButton
          className={"w-full"}
          effect={"expandIcon"}
          icon={CgSize}
          iconPlacement={"right"}
          type="button"
          disabled={mutation.isPending}
          onClick={() => {
            mutation.mutate({
              body: {
                versions: !patchId ? undefined : [patchId],
              },
            });
          }}
        >
          {mutation.isPending && (
            <Icons.spinner
              className="size-4 mr-2 animate-spin"
              aria-hidden="true"
            />
          )}
          Resize
        </EnhancedButton>
      }
    />
  );
};

export default ResizePatchDialog;
