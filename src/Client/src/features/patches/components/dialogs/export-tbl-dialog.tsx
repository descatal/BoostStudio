import React from "react";

import { Button } from "@/components/ui/button";
import { toast } from "@/components/ui/use-toast";
import ConfirmationDialog from "@/components/custom/confirmation-dialog";
import { PatchIdNameMap } from "@/features/patches/libs/constants";
import { PatchFileVersion } from "@/api/exvs";
import { useMutation } from "@tanstack/react-query";
import { postApiTblExportMutation } from "@/api/exvs/@tanstack/react-query.gen";

type ResizeDialogProps = {
  patchId: PatchFileVersion | undefined;
};

const ExportTblDialog = ({ patchId }: ResizeDialogProps) => {
  const exportTblMutation = useMutation({
    ...postApiTblExportMutation(),
    onSuccess: () => {
      toast({
        title: "Export success!",
      });
    },
  });

  return (
    <ConfirmationDialog
      isDone={exportTblMutation.isSuccess}
      icon="info"
      title="Are you sure?"
      body={`This will replace the tbl information at ${patchId && PatchIdNameMap[patchId]}/PATCH.TBL`}
      triggerButton={<Button>Export</Button>}
      confirmButton={
        <Button
          type="button"
          onClick={() => {
            if (!patchId) return;
            exportTblMutation.mutate({
              body: {
                versions: [patchId],
                replaceStaging: true,
              },
            });
          }}
        >
          Confirm
        </Button>
      }
    />
  );
};

export default ExportTblDialog;
