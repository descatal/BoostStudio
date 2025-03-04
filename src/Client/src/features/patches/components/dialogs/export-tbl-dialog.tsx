import React from "react";
import { useExportTbl } from "@/features/patches/api/export-tbl";

import { Button } from "@/components/ui/button";
import { toast } from "@/components/ui/use-toast";
import ConfirmationDialog from "@/components/custom/confirmation-dialog";
import { PatchIdNameMap } from "@/features/patches/libs/constants";
import { PatchFileVersion } from "@/api/exvs";

type ResizeDialogProps = {
  patchId: PatchFileVersion | undefined;
};

const ExportTblDialog = ({ patchId }: ResizeDialogProps) => {
  const exportTblMutation = useExportTbl({
    mutationConfig: {
      onSuccess: () => {
        toast({
          title: "Export success!",
        });
      },
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
              versions: [patchId],
              replaceStaging: true,
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
