import React from "react";
import { Button } from "@/components/ui/button";
import { toast } from "@/components/ui/use-toast";
import ConfirmationDialog from "@/components/custom/confirmation-dialog";
import { PatchIdNameMap } from "@/features/patches/libs/constants";
import { PatchFileVersion } from "@/api/exvs";
import { useMutation } from "@tanstack/react-query";
import { postApiPatchFilesResizeMutation } from "@/api/exvs/@tanstack/react-query.gen";

type ResizeDialogProps = {
  patchId?: PatchFileVersion | undefined;
};

const ResizePatchDialog = ({ patchId }: ResizeDialogProps) => {
  const resizePatchesMutation = useMutation({
    ...postApiPatchFilesResizeMutation(),
    onSuccess: () => {
      toast({
        title: "Export success!",
      });
    },
  });

  return (
    <ConfirmationDialog
      isDone={resizePatchesMutation.isSuccess}
      icon="info"
      title="Are you sure?"
      body={`Resize all patch entries for ${PatchIdNameMap[patchId ?? "All"]}?`}
      triggerButton={<Button>Resize</Button>}
      confirmButton={
        <Button
          type="button"
          onClick={() => {
            resizePatchesMutation.mutate({
              body: {
                versions: !patchId ? undefined : [patchId],
              },
            });
          }}
        >
          Resize
        </Button>
      }
    />
  );
};

export default ResizePatchDialog;
