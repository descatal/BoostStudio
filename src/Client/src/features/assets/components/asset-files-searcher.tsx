import React from "react";
import { AssetFileType, AssetFileVm } from "@/api/exvs";
import SelectAssetFileType from "@/features/assets/components/select-asset-file-type";
import {
  CommonAssetFileOptionsType,
  UnitAssetFileOptionsType,
} from "@/lib/constants";

import { Label } from "@/components/ui/label";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";

import UnitsSelector from "../../units/components/units-selector";
import { useQuery } from "@tanstack/react-query";
import { getApiAssetsOptions } from "@/api/exvs/@tanstack/react-query.gen";

interface AssetFilesSearcherProps {
  unitIds?: number[];
  setResultAssetFiles: (resultAssetFiles: AssetFileVm[] | undefined) => void;
}

const AssetFilesSearcher = ({
  unitIds,
  setResultAssetFiles,
}: AssetFilesSearcherProps) => {
  const [selectedTab, setSelectedTab] = React.useState<"unit" | "common">(
    "unit",
  );

  const [selectedUnitIds, setSelectedUnitIds] = React.useState<
    number[] | undefined
  >(unitIds);

  const [selectedUnitAssetFileType, setSelectedUnitAssetFileType] =
    React.useState<UnitAssetFileOptionsType>();

  const [selectedCommonAssetFileType, setSelectedCommonAssetFileType] =
    React.useState<CommonAssetFileOptionsType>();

  const [selectedFileType, setSelectedFileType] = React.useState<
    AssetFileType[]
  >([]);

  const assetFilesQuery = useQuery({
    ...getApiAssetsOptions({
      query: {
        UnitIds: selectedTab === "unit" ? selectedUnitIds : undefined,
        AssetFileTypes: selectedFileType,
        ListAll: true,
      },
    }),
    enabled: selectedFileType.length > 0,
  });

  const assetFiles = assetFilesQuery.data?.items ?? [];

  React.useEffect(() => {
    if (assetFiles.length <= 0) {
      setResultAssetFiles(undefined);
    } else {
      setResultAssetFiles(assetFiles);
    }
  }, [assetFiles]);

  React.useEffect(() => {
    const selectedFileType =
      selectedTab === "unit"
        ? selectedUnitAssetFileType && [selectedUnitAssetFileType]
        : selectedCommonAssetFileType && [selectedCommonAssetFileType];
    setSelectedFileType(selectedFileType ?? []);
  }, [selectedTab, selectedUnitAssetFileType, selectedCommonAssetFileType]);

  return (
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
            multiple
            disabled={!!unitIds}
            className={"w-full"}
            values={selectedUnitIds}
            onChange={setSelectedUnitIds}
            fixedValues={unitIds}
            placeholder={unitIds ? undefined : "Select units..."}
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
              setSelectedCommonAssetFileType(type as CommonAssetFileOptionsType)
            }
          ></SelectAssetFileType>
        </div>
      </TabsContent>
    </Tabs>
  );
};

export default AssetFilesSearcher;
