import React from "react"
import {UnitDto} from "@/api/exvs"

import {Button} from "@/components/ui/button"
import {Card, CardContent, CardDescription, CardHeader, CardTitle,} from "@/components/ui/card"
import {Separator} from "@/components/ui/separator"
import {toast} from "@/components/ui/use-toast"
import {Icons} from "@/components/icons"
import {ArrowBigDownDash} from "lucide-react";
import {Switch} from "@/components/ui/switch";
import {exportHitboxes} from "@/api/wrapper/hitbox-api";
import {Label} from "@/components/ui/label";
import UnitSwitcher from "@/pages/common/components/custom/unit-switcher";
import {exportAmmo} from "@/api/wrapper/ammo-api";

interface AmmoExportProps {
  onExport: () => void
}

const AmmoExport = ({onExport}: AmmoExportProps) => {
  const [isExportPending, setIsExportPending] = React.useState(false)
  const [hotReload, setHotReload] = React.useState(true)

  const handleExport = async () => {
    try {
      await exportAmmo({
        exportAmmoCommand: {
          replaceWorking: true,
          hotReload: hotReload
        },
      })

      toast({
        title: "Success",
        description: `Successfully exported ammo binary to working directory!`,
      })

      onExport()
    } catch (e) {
      toast({
        title: `Error`,
        description: `Export failed! ${e}`,
        variant: "destructive",
      })
    }
  }

  return (
    <Card>
      <CardHeader>
        <CardTitle>Export Ammo Info</CardTitle>
        <CardDescription>
          Export ammo info for ALL units to working directory.
        </CardDescription>
      </CardHeader>
      <CardContent>
        <div className="grid gap-4">
          <div className="flex items-center space-x-4 rounded-md border p-4">
            <ArrowBigDownDash/>
            <div className="flex-1 space-y-1">
              <p className="text-sm font-medium leading-none">Hot Reload</p>
              <p className="text-sm text-muted-foreground">
                Patch the compiled ammo binary to running RPCS3.
              </p>
            </div>
            <Switch checked={hotReload} onCheckedChange={setHotReload}/>
          </div>
          <Separator/>
          <Button
            disabled={isExportPending}
            onClick={async () => {
              setIsExportPending(true)
              await handleExport()
              setIsExportPending(false)
            }}
          >
            {isExportPending && (
              <Icons.spinner
                className="size-4 mr-2 animate-spin"
                aria-hidden="true"
              />
            )}
            Export
          </Button>
        </div>
      </CardContent>
    </Card>
  )
}

export default AmmoExport
