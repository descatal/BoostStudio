import React from "react"
import { AssetFileType, AssetFileVm, UnitDto } from "@/api/exvs"
import { fetchAssetFiles } from "@/api/wrapper/asset-api"
import SelectAssetFileType from "@/pages/common/components/selects/select-asset-file-type"
import {
  CommonAssetFileOptionsType,
  UnitAssetFileOptionsType,
} from "@/pages/common/libs/constants"
import UnitSwitcher from "@/pages/units/customize/components/unit-switcher"

import { Label } from "@/components/ui/label"
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs"

interface AssetFilesSearcherProps {
  setResultAssetFiles: (resultAssetFiles: AssetFileVm[] | undefined) => void
}

const AssetFilesSearcher = ({
  setResultAssetFiles,
}: AssetFilesSearcherProps) => {
  const [selectedTab, setSelectedTab] = React.useState<"unit" | "common">(
    "unit"
  )
  const [selectedUnits, setSelectedUnits] = React.useState<
    UnitDto[] | undefined
  >()
  const [selectedUnitAssetFileType, setSelectedUnitAssetFileType] =
    React.useState<UnitAssetFileOptionsType>()
  const [selectedCommonAssetFileType, setSelectedCommonAssetFileType] =
    React.useState<CommonAssetFileOptionsType>()

  const getData = React.useCallback(
    async (unitIds: number[] | undefined, assetFileTypes: AssetFileType[]) => {
      const assetFiles = await fetchAssetFiles({
        unitIds: unitIds,
        assetFileTypes: assetFileTypes,
      })

      if (assetFiles.items.length <= 0) {
        setResultAssetFiles(undefined)
      } else {
        setResultAssetFiles(assetFiles.items)
      }
    },
    [
      selectedTab,
      selectedUnits,
      selectedUnitAssetFileType,
      selectedCommonAssetFileType,
    ]
  )

  React.useEffect(() => {
    const search = async () => {
      if (
        selectedTab === "unit" &&
        selectedUnitAssetFileType &&
        selectedUnits &&
        selectedUnits.length > 0
      ) {
        const unitIds = !selectedUnits[0].unitId
          ? undefined
          : [selectedUnits[0].unitId!]
        await getData(unitIds, [selectedUnitAssetFileType])
      } else if (selectedTab === "common" && selectedCommonAssetFileType) {
        await getData(undefined, [selectedCommonAssetFileType])
      }
    }

    search().catch((error) => console.error(error))
  }, [
    selectedTab,
    selectedUnits,
    selectedUnitAssetFileType,
    selectedCommonAssetFileType,
  ])

  return (
    <>
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
              setSelectedUnits={setSelectedUnits}
              selectedUnits={selectedUnits}
            />
          </div>
          <div className={"space-y-2"}>
            <Label>File type</Label>
            <SelectAssetFileType
              type="unit"
              selectedUnitAssetFileType={
                selectedUnitAssetFileType as UnitAssetFileOptionsType
              }
              setSelectedUnitAssetFileType={(type) =>
                setSelectedUnitAssetFileType(type as UnitAssetFileOptionsType)
              }
            ></SelectAssetFileType>
          </div>
        </TabsContent>
        <TabsContent value="common" className="space-y-4">
          <div className={"space-y-2"}>
            <Label>Search by common asset files</Label>
            <SelectAssetFileType
              type="common"
              selectedUnitAssetFileType={
                selectedCommonAssetFileType as CommonAssetFileOptionsType
              }
              setSelectedUnitAssetFileType={(type) =>
                setSelectedCommonAssetFileType(
                  type as CommonAssetFileOptionsType
                )
              }
            ></SelectAssetFileType>
          </div>
        </TabsContent>
      </Tabs>
    </>
  )
}

export default AssetFilesSearcher
