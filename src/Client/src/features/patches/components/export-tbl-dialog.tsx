import React from "react"
import { useExportTbl } from "@/features/patches/api/export-tbl"
import { PatchFileTabs, PatchIdNameMap } from "@/pages/patches/libs/store"

import { Button } from "@/components/ui/button"
import { toast } from "@/components/ui/use-toast"
import ConfirmationDialog from "@/components/custom/confirmation-dialog"

type ResizeDialogProps = {
  patchId: PatchFileTabs | undefined
}

const ExportTblDialog = ({ patchId }: ResizeDialogProps) => {
  const exportTblMutation = useExportTbl({
    mutationConfig: {
      onSuccess: () => {
        toast({
          title: "Export success!",
        })
      },
    },
  })

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
            if (!patchId || patchId === "All") return

            exportTblMutation.mutate({
              versions: [patchId],
              replaceStaging: true,
            })
          }}
        >
          Confirm
        </Button>
      }
    />
  )
}

export default ExportTblDialog
