import React from "react"
import {useExportDialogStore} from "@/pages/units/customize/information/components/export-dialog/libs/store"
import {useUnitsStore} from "@/pages/units/libs/store"
import {Dialog, DialogContent, DialogTitle,} from "@/components/ui/dialog"
import HitboxExport from "@/pages/units/customize/information/components/export-dialog/hitboxes";
import {Tabs, TabsContent, TabsList, TabsTrigger} from "@/components/ui/tabs";
import {useCustomizeInformationUnitStore} from "@/pages/units/customize/information/libs/store";
import ProjectileExport from "@/pages/units/customize/information/components/export-dialog/projectiles";
import AmmoExport from "@/pages/units/customize/information/components/export-dialog/ammo";

interface ExportDialogProps extends React.ComponentPropsWithRef<typeof Dialog> {
}

const ExportDialog = ({...props}: ExportDialogProps) => {
  const {
    openExportDialog,
    setOpenExportDialog,
  } = useExportDialogStore()

  const unit = useUnitsStore((state) => state.selectedUnits)
  const { selectedInformationTab } = useCustomizeInformationUnitStore(state => state);

  return (
    <Dialog open={openExportDialog} onOpenChange={setOpenExportDialog}>
      <DialogContent>
        <DialogTitle>
          Export Information
        </DialogTitle>
        <Tabs defaultValue={selectedInformationTab}>
          <TabsList>
            <TabsTrigger value="stats">Stats</TabsTrigger>
            <TabsTrigger value="ammo">Ammo</TabsTrigger>
            <TabsTrigger value="projectiles">Projectiles</TabsTrigger>
            <TabsTrigger value="hitboxes">Hitboxes</TabsTrigger>
          </TabsList>
          <TabsContent value="stats" className="space-y-4">
          </TabsContent>
          <TabsContent value="ammo" className="space-y-4">
            <AmmoExport
              onExport={() => {
                setOpenExportDialog(false)
              }}
            />
          </TabsContent>
          <TabsContent value="projectiles" className="space-y-4">
            <ProjectileExport
              units={unit}
              onExport={() => {
                setOpenExportDialog(false)
              }}
            />
          </TabsContent>
          <TabsContent value="hitboxes" className="space-y-4">
            <HitboxExport
              units={unit}
              onExport={() => {
                setOpenExportDialog(false)
              }}/>
          </TabsContent>
        </Tabs>
      </DialogContent>
    </Dialog>
  )
}

export default ExportDialog
