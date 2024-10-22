import React from "react"
import { AssetFileVm } from "@/api/exvs"
import { packFhmAssets, unpackFhmAssets } from "@/api/wrapper/fhm-api"
import { createPatchFiles, updatePatchFiles } from "@/api/wrapper/tbl-api"
import AssetFilesSearcher from "@/pages/common/components/custom/asset-files-searcher"
import {
  createPatchFileSchema,
  UpdatePatchFileSchema,
  updatePatchFileSchema,
} from "@/pages/patches/components/tabs/libs/validations"
import { ReloadIcon } from "@radix-ui/react-icons"

import { Button } from "@/components/ui/button"
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"
import { Separator } from "@/components/ui/separator"
import { Tabs, TabsContent, TabsList } from "@/components/ui/tabs"
import { toast } from "@/components/ui/use-toast"

const AssetTools = () => {
  const [isPackPending, startPackTransition] = React.useTransition()
  const [isUnpackPending, startUnpackTransition] = React.useTransition()

  const [selectedUnpackAssetFile, setSelectedUnpackAssetFile] = React.useState<
    AssetFileVm[] | undefined
  >()

  const [selectedPackAssetFile, setSelectedPackAssetFile] = React.useState<
    AssetFileVm[] | undefined
  >()

  return (
    <div className="grid gap-4 md:grid-cols-[1fr_250px] lg:grid-cols-3 lg:gap-8">
      <Tabs>
        <TabsList></TabsList>
        <div className="grid auto-rows-max items-start gap-4 lg:col-span-2 lg:gap-8">
          <TabsContent value={"pack"}>
            <Card>
              <CardHeader>
                <CardTitle>Pack Assets</CardTitle>
                <CardDescription>
                  Pack asset files to .fhm container format from working
                  directory to staging directory.
                </CardDescription>
              </CardHeader>
              <CardContent>
                <div className="grid gap-6">
                  <AssetFilesSearcher
                    setResultAssetFiles={setSelectedPackAssetFile}
                  />
                  <Separator />
                  <Button
                    disabled={isPackPending}
                    onClick={() => {
                      startPackTransition(async () => {
                        if (!selectedPackAssetFile) return

                        await packFhmAssets({
                          packFhmAssetCommand: {
                            assetFileHashes: selectedPackAssetFile.map(
                              (x) => x.hash
                            ),
                            replaceStaging: true,
                          },
                        })
                      })
                    }}
                  >
                    {isPackPending && (
                      <ReloadIcon
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
        </div>
        <div className="grid auto-rows-max items-start gap-4 lg:col-span-2 lg:gap-8">
          <TabsContent value={"unpack"}>
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
                    setResultAssetFiles={setSelectedUnpackAssetFile}
                  />
                  {selectedUnpackAssetFile && (
                    <>
                      <Separator />
                      <Button
                        disabled={isUnpackPending}
                        onClick={() => {
                          startUnpackTransition(async () => {
                            if (!selectedUnpackAssetFile) return

                            await unpackFhmAssets({
                              unpackFhmAssetCommand: {
                                assetFileHashes: selectedUnpackAssetFile.map(
                                  (x) => x.hash
                                ),
                                replaceWorking: true,
                              },
                            })
                          })
                        }}
                      >
                        {isUnpackPending && (
                          <ReloadIcon
                            className="size-4 mr-2 animate-spin"
                            aria-hidden="true"
                          />
                        )}
                        Unpack
                      </Button>
                    </>
                  )}
                </div>
              </CardContent>
            </Card>
          </TabsContent>
        </div>
      </Tabs>
    </div>
  )
}

export default AssetTools
