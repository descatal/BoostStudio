import React from "react";
import { AssetFileType, AssetFileVm, UnitSummaryVm } from "@/api/exvs";
import SelectAssetFileType from "@/features/assets/components/select-asset-file-type";
import {
  CommonAssetFileOptionsType,
  UnitAssetFileOptionsType,
} from "@/lib/constants";

import { Label } from "@/components/ui/label";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";

import UnitsSelector from "../../../../features/units/components/units-selector";
import { useQuery } from "@tanstack/react-query";
import { getApiAssetsOptions } from "@/api/exvs/@tanstack/react-query.gen";

interface AssetFilesSearcherProps {
  units?: UnitSummaryVm[] | undefined;
  setResultAssetFiles: (resultAssetFiles: AssetFileVm[] | undefined) => void;
}

// Burn this with fire

const AssetFilesSearcher = ({
  units,
  setResultAssetFiles,
}: AssetFilesSearcherProps) => {
  const [selectedTab, setSelectedTab] = React.useState<"unit" | "common">(
    "unit",
  );

  const [selectedUnits, setSelectedUnits] = React.useState<
    UnitSummaryVm[] | undefined
  >();

  const [selectedUnitAssetFileType, setSelectedUnitAssetFileType] =
    React.useState<UnitAssetFileOptionsType>();

  const [selectedCommonAssetFileType, setSelectedCommonAssetFileType] =
    React.useState<CommonAssetFileOptionsType>();

  React.useEffect(() => {
    if (units) setSelectedUnits(units);
  }, []);

  const getData = React.useCallback(
    async (unitIds: number[] | undefined, assetFileTypes: AssetFileType[]) => {
      const assetFilesQuery = useQuery({
        ...getApiAssetsOptions({
          query: {
            UnitIds: unitIds,
            AssetFileTypes: assetFileTypes,
          },
        }),
      });

      const assetFiles = assetFilesQuery.data?.items ?? [];

      if (assetFiles.length <= 0) {
        setResultAssetFiles(undefined);
      } else {
        setResultAssetFiles(assetFiles);
      }
    },
    [
      selectedTab,
      selectedUnits,
      selectedUnitAssetFileType,
      selectedCommonAssetFileType,
    ],
  );

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
          : [selectedUnits[0].unitId!];
        await getData(unitIds, [selectedUnitAssetFileType]);
      } else if (selectedTab === "common" && selectedCommonAssetFileType) {
        await getData(undefined, [selectedCommonAssetFileType]);
      }
    };

    search().catch((error) => console.error(error));
  }, [
    selectedTab,
    selectedUnits,
    selectedUnitAssetFileType,
    selectedCommonAssetFileType,
  ]);

  return (
    <>
      <Tabs
        defaultValue="unit"
        className="space-y-4"
        value={selectedTab}
        onValueChange={(e) => {
          setSelectedTab(e as "unit" | "common");
        }}
      >
        <TabsList>
          <TabsTrigger value="unit">Unit</TabsTrigger>
          <TabsTrigger value="common">Common</TabsTrigger>
        </TabsList>
        <TabsContent value="unit" className="space-y-4">
          <div className={"space-y-2"}>
            <Label>Search by unit asset files</Label>
            <UnitsSelector
              disabled={!!units}
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
            />
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
                  type as CommonAssetFileOptionsType,
                )
              }
            ></SelectAssetFileType>
          </div>
        </TabsContent>
      </Tabs>
    </>
  );
};

export default AssetFilesSearcher;
