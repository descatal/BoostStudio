﻿import React from "react"
import {UnitDto} from "@/api/exvs"

import {Button} from "@/components/ui/button"
import {Card, CardContent, CardDescription, CardHeader, CardTitle,} from "@/components/ui/card"
import {Separator} from "@/components/ui/separator"
import {toast} from "@/components/ui/use-toast"
import {Icons} from "@/components/icons"
import {ArrowBigDownDash} from "lucide-react";
import {Switch} from "@/components/ui/switch";
import {Label} from "@/components/ui/label";
import UnitSwitcher from "@/pages/common/components/custom/unit-switcher";
import {exportProjectiles} from "@/api/wrapper/projectile-api";

interface ProjectileExportProps {
  units?: UnitDto[] | undefined
  onExport: () => void
}

const ProjectileExport = ({units, onExport}: ProjectileExportProps) => {
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
      await exportProjectiles({
        exportUnitProjectileCommand: {
          unitIds: selectedExportUnits?.map((x) => x.unitId!),
          replaceWorking: true,
          hotReload: hotReload
        },
      })

      toast({
        title: "Success",
        description: `Successfully exported projectiles binary to working directory!`,
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
        <CardTitle>Export Projectile Info</CardTitle>
        <CardDescription>
          Export projectile info for this unit to working directory.
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
                Patch the compiled projectiles binary to running RPCS3.
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

export default ProjectileExport