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

interface HitboxExportProps {
  units?: UnitDto[] | undefined
  onExport: () => void
}

const HitboxExport = ({units, onExport}: HitboxExportProps) => {
  const [isExportPending, setIsExportPending] = React.useState(false)
  const [hotReload, setHotReload] = React.useState(true)

  const [selectedExportUnits, setSelectedExportUnits] =
    React.useState<UnitDto[]>()

  React.useEffect(() => {
    if (units) {
      setSelectedExportUnits(units)
    }
  }, [units])

  const handleExport = async () => {
    try {
      await exportHitboxes({
        exportHitboxGroupCommand: {
          unitIds: selectedExportUnits?.map((x) => x.unitId!),
          replaceWorking: true,
          hotReload: hotReload
        },
      })

      toast({
        title: "Success",
        description: `Successfully exported hitbox binary to working directory!`,
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
        <CardTitle>Export Hitbox Info</CardTitle>
        <CardDescription>
          Export hitbox info for this unit to working directory.
        </CardDescription>
      </CardHeader>
      <CardContent>
        <div className="grid gap-4">
          <div className={"space-y-2"}>
            <Label>Units</Label>
            <UnitSwitcher
              disabled={!!units}
              multipleSelect={true}
              selectedUnits={selectedExportUnits}
              setSelectedUnits={setSelectedExportUnits}
            />
          </div>
          <div className="flex items-center space-x-4 rounded-md border p-4">
            <ArrowBigDownDash/>
            <div className="flex-1 space-y-1">
              <p className="text-sm font-medium leading-none">Hot Reload</p>
              <p className="text-sm text-muted-foreground">
                Patch the compiled hitboxes binary to running RPCS3.
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

export default HitboxExport
