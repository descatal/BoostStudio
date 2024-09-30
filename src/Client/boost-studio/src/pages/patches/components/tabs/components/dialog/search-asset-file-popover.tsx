import * as React from "react"
import { AssetFileType, AssetFileVm, UnitDto } from "@/api/exvs"
import { fetchAssetFiles } from "@/api/wrapper/asset-api"
import UnitSwitcher from "@/pages/units/customize/components/unit-switcher"

import { Button } from "@/components/ui/button"
import { Label } from "@/components/ui/label"
import { Popover, PopoverContent } from "@/components/ui/popover"
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
import { HashInput } from "@/components/custom/hash-input"

const UnitAssetFileType = {
  Unknown: "Unknown",
  Dummy: "Dummy",
  Animations: "Animations",
  Models: "Models",
  Data: "Data",
  Effects: "Effects",
  SoundEffects: "SoundEffects",
  InGamePilotVoiceLines: "InGamePilotVoiceLines",
  WeaponSprites: "WeaponSprites",
  InGameCutInSprites: "InGameCutInSprites",
  SpriteFrames: "SpriteFrames",
  VoiceLinesMetadata: "VoiceLinesMetadata",
  PilotVoiceLines: "PilotVoiceLines",
} as const
export type UnitAssetFileType =
  (typeof UnitAssetFileType)[keyof typeof UnitAssetFileType]

const CommonAssetFileType = {
  Hitbox: "Hitbox",
  Projectiles: "Projectiles",
  Ammo: "Ammo",
  RosterInfo: "RosterInfo",
  UnitCostInfo: "UnitCostInfo",
  FigurineSprites: "FigurineSprites",
  MapSelectSprites: "MapSelectSprites",
  ArcadeSelectSmallSprites: "ArcadeSelectSmallSprites",
  ArcadeSelectUnitNameSprites: "ArcadeSelectUnitNameSprites",
  CameraConfigs: "CameraConfigs",
  CommonEffects: "CommonEffects",
  CommonEffectParticles: "CommonEffectParticles",
  CosmeticInfo: "CosmeticInfo",
  TextStrings: "TextStrings",
  SeriesLogoSprites: "SeriesLogoSprites",
  SeriesLogoSprites2: "SeriesLogoSprites2",
} as const
export type CommonAssetFileType =
  (typeof CommonAssetFileType)[keyof typeof CommonAssetFileType]

interface SearchAssetFilePopoverProps
  extends React.ComponentPropsWithRef<typeof Popover> {
  setAssetFile: (assetFile: AssetFileVm | undefined) => void
}

export function SearchAssetFilePopover({
  setAssetFile,
  children,
}: SearchAssetFilePopoverProps) {
  const [open, setOpen] = React.useState(false)
  const [selectedTab, setSelectedTab] = React.useState<"unit" | "common">(
    "unit"
  )
  const [selectedUnit, setSelectedUnit] = React.useState<UnitDto | undefined>()
  const [selectedUnitAssetFileType, setSelectedUnitAssetFileType] =
    React.useState<UnitAssetFileType>()
  const [selectedCommonAssetFileType, setSelectedCommonAssetFileType] =
    React.useState<CommonAssetFileType>()

  const [resultAssetFile, setResultAssetFile] = React.useState<
    AssetFileVm | undefined
  >()

  const getData = React.useCallback(
    async (unitIds: number[] | undefined, assetFileTypes: AssetFileType[]) => {
      const assetFiles = await fetchAssetFiles({
        unitIds: unitIds,
        assetFileTypes: assetFileTypes,
      })

      if (assetFiles.items.length <= 0) {
        setResultAssetFile(undefined)
      } else {
        setResultAssetFile(assetFiles.items[0])
      }
    },
    [
      selectedTab,
      selectedUnit,
      selectedUnitAssetFileType,
      selectedCommonAssetFileType,
    ]
  )

  React.useEffect(() => {
    const search = async () => {
      if (selectedTab === "unit" && selectedUnitAssetFileType && selectedUnit) {
        const unitIds = !selectedUnit.unitId
          ? undefined
          : [selectedUnit.unitId!]
        await getData(unitIds, [selectedUnitAssetFileType])
      } else if (selectedTab === "common" && selectedCommonAssetFileType) {
        await getData(undefined, [selectedCommonAssetFileType])
      }
    }

    search().catch((error) => console.error(error))
  }, [
    selectedTab,
    selectedUnit,
    selectedUnitAssetFileType,
    selectedCommonAssetFileType,
  ])

  return (
    <Popover open={open} onOpenChange={setOpen}>
      <PopoverContent>
        <div className={"space-y-4"}>
          <Tabs
            defaultValue="unit"
            className="space-y-4"
            value={selectedTab}
            onValueChange={(e) => {
              setSelectedTab(e as "unit" | "common")
            }}
          >
            <TabsList>
              <TabsTrigger value="unit">Unit</TabsTrigger>
              <TabsTrigger value="common">Common</TabsTrigger>
            </TabsList>
            <TabsContent value="unit" className="space-y-4">
              <div className={"space-y-2"}>
                <Label>Search by unit asset files</Label>
                <UnitSwitcher
                  setSelectedUnits={setSelectedUnit}
                  selectedUnits={selectedUnit}
                />
              </div>
              <div className={"space-y-2"}>
                <Label>File type</Label>
                <Select
                  value={selectedUnitAssetFileType}
                  onValueChange={(e) =>
                    setSelectedUnitAssetFileType(e as UnitAssetFileType)
                  }
                >
                  <SelectTrigger className="capitalize">
                    <SelectValue placeholder="Select File Type" />
                  </SelectTrigger>
                  <SelectContent className={"max-h-[10rem] overflow-y-auto"}>
                    <SelectGroup>
                      {Object.values(UnitAssetFileType).map((type) => (
                        <SelectItem
                          key={type}
                          value={type}
                          className="capitalize"
                        >
                          {type}
                        </SelectItem>
                      ))}
                    </SelectGroup>
                  </SelectContent>
                </Select>
              </div>
            </TabsContent>
            <TabsContent value="common" className="space-y-4">
              <div className={"space-y-2"}>
                <Label>Search by common asset files</Label>
                <Select
                  value={selectedCommonAssetFileType}
                  onValueChange={(e) =>
                    setSelectedCommonAssetFileType(e as CommonAssetFileType)
                  }
                >
                  <SelectTrigger className="capitalize">
                    <SelectValue placeholder="Select Common File Type" />
                  </SelectTrigger>
                  <SelectContent className={"max-h-[10rem] overflow-y-auto"}>
                    <SelectGroup>
                      {Object.values(CommonAssetFileType).map((type) => (
                        <SelectItem
                          key={type}
                          value={type}
                          className="capitalize"
                        >
                          {type}
                        </SelectItem>
                      ))}
                    </SelectGroup>
                  </SelectContent>
                </Select>
              </div>
            </TabsContent>
          </Tabs>
          <Separator />
          <div className={"space-y-2"}>
            <Label>File Hash</Label>
            <HashInput
              readonly={true}
              className={`${resultAssetFile ? "border-green-800" : "border-red-800"}`}
              initialMode={"hex"}
              initialValue={resultAssetFile?.hash}
              placeholder={"e.g. 1A42E312"}
            />
          </div>
          <Button
            disabled={!resultAssetFile}
            className={"w-full"}
            onClick={() => {
              setAssetFile(resultAssetFile)
              setOpen(false)
            }}
          >
            Select
          </Button>
        </div>
      </PopoverContent>
      {children}
    </Popover>
  )
}
