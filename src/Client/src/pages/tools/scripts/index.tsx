import React from "react"
import { UnitSummaryVm } from "@/api/exvs"
import { compileScexUnits, decompileScexUnits } from "@/api/wrapper/scex-api"
import SeriesUnitsSelector from "@/features/series/components/series-units-selector"
import { ArrowBigDownDash } from "lucide-react"

import { Button } from "@/components/ui/button"
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"
import { Label } from "@/components/ui/label"
import { Separator } from "@/components/ui/separator"
import { Switch } from "@/components/ui/switch"
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs"
import { toast } from "@/components/ui/use-toast"
import { Icons } from "@/components/icons"

interface ScriptToolsProps {
  units?: UnitSummaryVm[] | undefined
}

const ScriptTools = ({ units }: ScriptToolsProps) => {
  const [isCompilePending, setIsCompilePending] = React.useState(false)
  const [isDecompilePending, setIsDecompilePending] = React.useState(false)

  const [hotReload, setHotReload] = React.useState(false)

  const [selectedCompileUnits, setSelectedCompileUnits] =
    React.useState<UnitSummaryVm[]>()

  const [selectedDecompileUnits, setSelectedDecompileUnits] =
    React.useState<UnitSummaryVm[]>()

  React.useEffect(() => {
    if (units) {
      setSelectedCompileUnits(units)
      setSelectedDecompileUnits(units)
    }
  }, [units])

  const compileUnitScripts = async () => {
    if (!selectedCompileUnits) {
      toast({
        title: `Error`,
        description: `Please select at least one unit!`,
        variant: "destructive",
      })
      return
    }

    try {
      await compileScexUnits({
        compileScexByUnitsCommand: {
          unitIds: selectedCompileUnits.map((x) => x.unitId ?? 0),
          replaceWorking: true,
          hotReload: hotReload ?? false,
        },
      })

      toast({
        title: "Success",
        description: `Successfully compiled scripts to working directory!`,
      })
    } catch (e) {
      toast({
        title: `Error`,
        description: `Compilation failed! ${e}`,
        variant: "destructive",
      })
    }
  }

  const decompileUnitScripts = async () => {
    if (!selectedDecompileUnits) {
      toast({
        title: `Error`,
        description: `Please select at least one asset!`,
        variant: "destructive",
      })
      return
    }

    try {
      await decompileScexUnits({
        decompileScexByUnitsCommand: {
          unitIds: selectedDecompileUnits.map((x) => x.unitId ?? 0),
          replaceScript: true,
        },
      })

      toast({
        title: "Success",
        description: `Successfully decompiled scripts to script directory!`,
      })
    } catch (e) {
      toast({
        title: `Error`,
        description: `Decompilation failed! ${e}`,
        variant: "destructive",
      })
    }
  }

  return (
    <Tabs defaultValue={"compile"}>
      <TabsList>
        <TabsTrigger value="compile">Compile</TabsTrigger>
        <TabsTrigger value="decompile">Decompile</TabsTrigger>
      </TabsList>
      <TabsContent className={"w-full"} value={"compile"}>
        <Card>
          <CardHeader>
            <CardTitle>Compile Scripts</CardTitle>
            <CardDescription>
              Compile units' c format code to .scex compiled binary (005.bin) in
              data folder.
            </CardDescription>
          </CardHeader>
          <CardContent>
            <div className="grid gap-4">
              <div className={"space-y-2"}>
                <Label>Units</Label>
                <SeriesUnitsSelector
                  disabled={!!units}
                  multipleSelect={true}
                  selectedUnits={selectedCompileUnits}
                  setSelectedUnits={setSelectedCompileUnits}
                />
              </div>
              <div className="flex items-center space-x-4 rounded-md border p-4">
                <ArrowBigDownDash />
                <div className="flex-1 space-y-1">
                  <p className="text-sm font-medium leading-none">Hot Reload</p>
                  <p className="text-sm text-muted-foreground">
                    Patch the compiled script binary to running RPCS3.
                  </p>
                </div>
                <Switch checked={hotReload} onCheckedChange={setHotReload} />
              </div>
              <Separator />
              <Button
                disabled={isCompilePending}
                onClick={async () => {
                  setIsCompilePending(true)
                  await compileUnitScripts()
                  setIsCompilePending(false)
                }}
              >
                {isCompilePending && (
                  <Icons.spinner
                    className="size-4 mr-2 animate-spin"
                    aria-hidden="true"
                  />
                )}
                Compile
              </Button>
            </div>
          </CardContent>
        </Card>
      </TabsContent>
      <TabsContent className={"w-full"} value={"decompile"}>
        <Card>
          <CardHeader>
            <CardTitle>Decompile Scripts</CardTitle>
            <CardDescription>
              Decompile units' .scex compiled binary (005.bin) in data folder to
              c format code.
            </CardDescription>
          </CardHeader>
          <CardContent>
            <div className="grid gap-6">
              <div className={"space-y-2"}>
                <Label>Units</Label>
                <SeriesUnitsSelector
                  disabled={!!units}
                  multipleSelect={true}
                  selectedUnits={selectedDecompileUnits}
                  setSelectedUnits={setSelectedDecompileUnits}
                />
              </div>
              <Separator />
              <Button
                disabled={isDecompilePending}
                onClick={async () => {
                  setIsDecompilePending(true)
                  await decompileUnitScripts()
                  setIsDecompilePending(false)
                }}
              >
                {isDecompilePending && (
                  <Icons.spinner
                    className="size-4 mr-2 animate-spin"
                    aria-hidden="true"
                  />
                )}
                Decompile
              </Button>
            </div>
          </CardContent>
        </Card>
      </TabsContent>
    </Tabs>
  )
}

export default ScriptTools
