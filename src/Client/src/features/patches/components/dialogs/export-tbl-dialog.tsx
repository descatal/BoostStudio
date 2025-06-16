import React from "react";
import ConfirmationDialog from "@/components/custom/confirmation-dialog";
import { PatchIdNameMap } from "@/features/patches/libs/constants";
import { PatchFileVersion } from "@/api/exvs";
import { useMutation } from "@tanstack/react-query";
import { postApiTblExportMutation } from "@/api/exvs/@tanstack/react-query.gen";
import { Icons } from "@/components/icons";
import { EnhancedButton } from "@/components/ui/enhanced-button";
import { BiExport } from "react-icons/bi";
import { toast } from "sonner";

type ResizeDialogProps = {
  patchId: PatchFileVersion | undefined;
};

const ExportTblDialog = ({ patchId }: ResizeDialogProps) => {
  const [open, setOpen] = React.useState(false);

  const mutation = useMutation({
    ...postApiTblExportMutation(),
    onSuccess: () => {
      toast("Success!", {
        description: "Patch info successfully exported.",
      });
      setOpen(false);
    },
  });

  return (
    <ConfirmationDialog
      open={open}
      onOpenChange={setOpen}
      icon="info"
      title="Are you sure?"
      body={`This will replace the tbl information at ${patchId && PatchIdNameMap[patchId]}/PATCH.TBL`}
      triggerButton={
        <EnhancedButton
          effect={"gooeyRight"}
          icon={BiExport}
          iconPlacement={"right"}
        >
          Export
        </EnhancedButton>
      }
      confirmButton={
        <EnhancedButton
          className={"w-full"}
          effect={"expandIcon"}
          icon={BiExport}
          iconPlacement={"right"}
          type="button"
          disabled={mutation.isPending}
          onClick={() => {
            if (!patchId) return;
            mutation.mutate({
              body: {
                versions: [patchId],
                replaceStaging: true,
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
          Confirm
        </EnhancedButton>
      }
    />
  );
};

export default ExportTblDialog;
