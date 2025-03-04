import React from "react";
import { useResizeTblPatches } from "@/features/patches/api/resize-tbl-patches";

import { Button } from "@/components/ui/button";
import { toast } from "@/components/ui/use-toast";
import ConfirmationDialog from "@/components/custom/confirmation-dialog";
import { PatchIdNameMap } from "@/features/patches/libs/constants";
import { PatchFileVersion } from "@/api/exvs";

type ResizeDialogProps = {
  patchId?: PatchFileVersion | undefined;
};

const ResizePatchDialog = ({ patchId }: ResizeDialogProps) => {
  const resizePatchesMutation = useResizeTblPatches({
    mutationConfig: {
      onSuccess: () => {
        toast({
          title: "Resize success!",
        });
      },
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
              versions: !patchId ? undefined : [patchId],
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
