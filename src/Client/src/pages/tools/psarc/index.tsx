import React from "react"
import { PatchFileVersion } from "@/api/exvs/models/PatchFileVersion"
import {
  packPsarcByPatchFiles,
  unpackPsarcByPatchFiles,
} from "@/api/wrapper/psarc-api"

import { Button } from "@/components/ui/button"
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"
import { Label } from "@/components/ui/label"
import {
  Select,
  SelectContent,
  SelectGroup,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select"
import { Separator } from "@/components/ui/separator"
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs"
import { toast } from "@/components/ui/use-toast"
import { Icons } from "@/components/icons"

const PsarcTools = () => {
  const [isPackPending, setIsPackPending] = React.useState(false)
  const [isUnpackPending, setIsUnpackPending] = React.useState(false)

  const [selectedPackPsarcPatchFileVersion, setSelectedPackPatchFileVersion] =
    React.useState<PatchFileVersion>(PatchFileVersion.Patch6)

  const [
    selectedUnpackPsarcPatchFileVersion,
    setSelectedUnpackPatchFileVersion,
  ] = React.useState<PatchFileVersion>(PatchFileVersion.Patch6)

  const packPsarc = async () => {
    if (!selectedPackPsarcPatchFileVersion) {
      toast({
        title: `Error`,
        description: `Please select at least one patch file version!`,
        variant: "destructive",
      })
      return
    }

    try {
      await packPsarcByPatchFiles({
        packPsarcByPatchFilesCommand: {
          patchFileVersions: [selectedPackPsarcPatchFileVersion],
        },
      })

      toast({
        title: "Success",
        description: `Successfully packed ${selectedPackPsarcPatchFileVersion} psarc to production directory!`,
      })
    } catch (e) {
      toast({
        title: `Error`,
        description: `Packing failed! ${e}`,
        variant: "destructive",
      })
    }
  }

  const handlePackPsarcSubmit = async () => {
    setIsPackPending(true)
    await packPsarc()
    setIsPackPending(false)
  }

  const unpackPsarc = async () => {
    if (!selectedUnpackPsarcPatchFileVersion) {
      toast({
        title: `Error`,
        description: `Please select at least one patch file version!`,
        variant: "destructive",
      })
      return
    }

    try {
      await unpackPsarcByPatchFiles({
        unpackPsarcByPatchFilesCommand: {
          patchFileVersions: [selectedUnpackPsarcPatchFileVersion],
        },
      })

      toast({
        title: "Success",
        description: `Successfully unpacked ${selectedUnpackPsarcPatchFileVersion} psarc to staging directory!`,
      })
    } catch (e) {
      toast({
        title: `Error`,
        description: `Unpacking failed! ${e}`,
        variant: "destructive",
      })
    }
  }

  const handleUnpackPsarcSubmit = async () => {
    setIsUnpackPending(true)
    await unpackPsarc()
    setIsUnpackPending(false)
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
            <CardTitle>Pack Psarc</CardTitle>
            <CardDescription>
              Pack staging directory's patch files to .psarc format in
              production directory.
            </CardDescription>
          </CardHeader>
          <CardContent>
            <div className="grid gap-6">
              <div className={"space-y-2"}>
                <Label>Version</Label>
                <Select
                  onValueChange={(e) => {
                    setSelectedPackPatchFileVersion(e as PatchFileVersion)
                  }}
                  defaultValue={selectedPackPsarcPatchFileVersion}
                >
                  <SelectTrigger className="capitalize">
                    <SelectValue placeholder="Select Patch File Version" />
                  </SelectTrigger>
                  <SelectContent>
                    <SelectGroup>
                      {Object.values(PatchFileVersion).map((version) => (
                        <SelectItem
                          key={version}
                          value={version}
                          className="capitalize"
                        >
                          {version}
                        </SelectItem>
                      ))}
                    </SelectGroup>
                  </SelectContent>
                </Select>
              </div>
              <Separator />
              <Button disabled={isPackPending} onClick={handlePackPsarcSubmit}>
                {isPackPending && (
                  <Icons.spinner
                    className="mr-2 size-4 animate-spin"
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
            <CardTitle>Unpack Psarc</CardTitle>
            <CardDescription>
              Unpack patch files from .psarc container format to staging
              directory.
            </CardDescription>
          </CardHeader>
          <CardContent>
            <div className="grid gap-6">
              <div className={"space-y-2"}>
                <Label>Version</Label>
                <Select
                  onValueChange={(e) => {
                    setSelectedUnpackPatchFileVersion(e as PatchFileVersion)
                  }}
                  defaultValue={selectedUnpackPsarcPatchFileVersion}
                >
                  <SelectTrigger className="capitalize">
                    <SelectValue placeholder="Select Patch File Version" />
                  </SelectTrigger>
                  <SelectContent>
                    <SelectGroup>
                      {Object.values(PatchFileVersion).map((version) => (
                        <SelectItem
                          key={version}
                          value={version}
                          className="capitalize"
                        >
                          {version}
                        </SelectItem>
                      ))}
                    </SelectGroup>
                  </SelectContent>
                </Select>
              </div>
              <Separator />
              <Button
                disabled={isUnpackPending}
                onClick={handleUnpackPsarcSubmit}
              >
                {isUnpackPending && (
                  <Icons.spinner
                    className="mr-2 size-4 animate-spin"
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

export default PsarcTools
