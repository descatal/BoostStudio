import React from "react"
import { AssetFileVm, UnitDto } from "@/api/exvs"
import { packFhmAssets, unpackFhmAssets } from "@/api/wrapper/fhm-api"
import AssetFilesSearcher from "@/pages/common/components/custom/asset-files-searcher"

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

interface AssetToolsProps {
  units?: UnitDto[] | undefined
}

const AssetTools = ({ units }: AssetToolsProps) => {
  const [isPackPending, setIsPackPending] = React.useState(false)
  const [isUnpackPending, setIsUnpackPending] = React.useState(false)

  const [selectedUnpackAssetFile, setSelectedUnpackAssetFile] = React.useState<
    AssetFileVm[] | undefined
  >()

  const [selectedPackAssetFile, setSelectedPackAssetFile] = React.useState<
    AssetFileVm[] | undefined
  >()

  const packFhmAsset = async () => {
    if (!selectedPackAssetFile) {
      toast({
        title: `Error`,
        description: `Please select at least one asset!`,
        variant: "destructive",
      })
      return
    }

    try {
      await packFhmAssets({
        packFhmAssetCommand: {
          assetFileHashes: selectedPackAssetFile.map((x) => x.hash),
          replaceStaging: true,
        },
      })

      toast({
        title: "Success",
        description: `Successfully packed assets to staging directory!`,
      })
    } catch (e) {
      toast({
        title: `Error`,
        description: `Packing failed! ${e}`,
        variant: "destructive",
      })
    }
  }

  const unpackFhmAsset = async () => {
    if (!selectedUnpackAssetFile) {
      toast({
        title: `Error`,
        description: `Please select at least one asset!`,
        variant: "destructive",
      })
      return
    }

    try {
      await unpackFhmAssets({
        unpackFhmAssetCommand: {
          assetFileHashes: selectedUnpackAssetFile.map((x) => x.hash),
          replaceWorking: true,
        },
      })

      toast({
        title: "Success",
        description: `Successfully unpacked assets to working directory!`,
      })
    } catch (e) {
      toast({
        title: `Error`,
        description: `Unpacking failed! ${e}`,
        variant: "destructive",
      })
    }
  }

  return (
    <Tabs defaultValue={"pack"}>
      <TabsList>
        <TabsTrigger value="pack">Pack</TabsTrigger>
        <TabsTrigger value="unpack">Unpack</TabsTrigger>
      </TabsList>
      <TabsContent className={"w-full"} value={"pack"}>
        <Card>
          <CardHeader>
            <CardTitle>Pack Assets</CardTitle>
            <CardDescription>
              Pack asset files to .fhm container format from working directory
              to staging directory.
            </CardDescription>
          </CardHeader>
          <CardContent>
            <div className="grid gap-6">
              <AssetFilesSearcher
                units={units}
                setResultAssetFiles={setSelectedPackAssetFile}
              />
              <Separator />
              <Button
                disabled={isPackPending}
                onClick={async () => {
                  setIsPackPending(true)
                  await packFhmAsset()
                  setIsPackPending(false)
                }}
              >
                {isPackPending && (
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
        <Card>
          <CardHeader>
            <CardTitle>Unpack Assets</CardTitle>
            <CardDescription>
              Unpack asset files from .fhm container format to working
              directory.
            </CardDescription>
          </CardHeader>
          <CardContent>
            <div className="grid gap-6">
              <AssetFilesSearcher
                units={units}
                setResultAssetFiles={setSelectedUnpackAssetFile}
              />
              <Separator />
              <Button
                disabled={isUnpackPending}
                onClick={async () => {
                  setIsUnpackPending(true)
                  await unpackFhmAsset()
                  setIsUnpackPending(false)
                }}
              >
                {isUnpackPending && (
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

export default AssetTools
