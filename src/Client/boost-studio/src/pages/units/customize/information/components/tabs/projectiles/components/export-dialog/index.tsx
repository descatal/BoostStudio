import React from "react"

import { Dialog } from "@/components/ui/dialog"

import { useProjectilesStore } from "../../libs/store"

const ExportProjectileDialog = () => {
  const { openExportDialog, setOpenExportDialog } = useProjectilesStore()

  return <Dialog></Dialog>
}

export default ExportProjectileDialog
