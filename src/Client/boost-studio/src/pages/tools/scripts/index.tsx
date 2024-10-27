import React from "react"
import { UnitDto } from "@/api/exvs"
import { compileScexUnits, decompileScexUnits } from "@/api/wrapper/scex-api"
import AssetFilesSearcher from "@/pages/common/components/custom/asset-files-searcher"
import UnitSwitcher from "@/pages/units/customize/components/unit-switcher"

import { Button } from "@/components/ui/button"
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"
import { Separator } from "@/components/ui/separator"
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs"
import { toast } from "@/components/ui/use-toast"
import { Icons } from "@/components/icons"

const ScriptTools = () => {
  const [isCompilePending, setIsCompilePending] = React.useState(false)
  const [isDecompilePending, setIsDecompilePending] = React.useState(false)

  const [selectedCompileUnits, setSelectedCompileUnits] =
    React.useState<UnitDto[]>()

  const [selectedDecompileUnits, setSelectedDecompileUnits] =
    React.useState<UnitDto[]>()

  const compileUnitScripts = async () => {
    if (!selectedCompileUnits) {
      toast({
        title: `Error`,
        description: `Please select at least one unit!`,
        variant: "destructive",
      })
      return
    }

    await compileScexUnits({
      compileScexByUnitsCommand: {
        unitIds: selectedCompileUnits.map((x) => x.unitId ?? 0),
        replaceWorking: true,
      },
    })

    toast({
      title: "Success",
      description: `Successfully compiled scripts to working directory!`,
    })
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
  }

  return (
    <Tabs defaultValue={"pack"}>
      <TabsList>
        <TabsTrigger value="pack">Pack</TabsTrigger>
        <TabsTrigger value="unpack">Unpack</TabsTrigger>
      </TabsList>
      <TabsContent className={"w-full"} value={"pack"}>
        <Card className={"md:max-w-[40vw]"}>
          <CardHeader>
            <CardTitle>Compile Scripts</CardTitle>
            <CardDescription>
              Compile units' c format code to .scex compiled binary (005.bin) in
              data folder.
            </CardDescription>
          </CardHeader>
          <CardContent>
            <div className="grid gap-6">
              <UnitSwitcher
                selectedUnits={selectedCompileUnits}
                setSelectedUnits={setSelectedCompileUnits}
              />
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
                Pack
              </Button>
            </div>
          </CardContent>
        </Card>
      </TabsContent>
      <TabsContent className={"w-full"} value={"unpack"}>
        <Card className={"md:max-w-[40vw]"}>
          <CardHeader>
            <CardTitle>Decompile Scripts</CardTitle>
            <CardDescription>
              Decompile units' .scex compiled binary (005.bin) in data folder to
              c format code.
            </CardDescription>
          </CardHeader>
          <CardContent>
            <div className="grid gap-6">
              <UnitSwitcher
                selectedUnits={selectedDecompileUnits}
                setSelectedUnits={setSelectedDecompileUnits}
              />
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
                Unpack
              </Button>
            </div>
          </CardContent>
        </Card>
      </TabsContent>
    </Tabs>
  )
}

export default ScriptTools
