import React from "react"
import { useExportTbl } from "@/features/patches/api/export-tbl"
import { useResizeTblPatches } from "@/features/patches/api/resize-tbl-patches"
import { PatchFileTabs, PatchIdNameMap } from "@/pages/patches/libs/store"

import { Button } from "@/components/ui/button"
import { toast } from "@/components/ui/use-toast"
import ConfirmationDialog from "@/components/custom/confirmation-dialog"

type ResizeDialogProps = {
  patchId: PatchFileTabs | undefined
}

const ResizePatchDialog = ({ patchId }: ResizeDialogProps) => {
  const resizePatchesMutation = useResizeTblPatches({
    mutationConfig: {
      onSuccess: () => {
        toast({
          title: "Resize success!",
        })
      },
    },
  })

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
              versions: !patchId || patchId === "All" ? undefined : [patchId],
            })
          }}
        >
          Resize
        </Button>
      }
    />
  )
}

export default ResizePatchDialog
