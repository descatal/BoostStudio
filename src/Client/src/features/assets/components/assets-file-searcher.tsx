import React from "react";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { Label } from "@/components/ui/label";
import UnitsSelector from "@/features/units/components/units-selector";
import SelectAssetFileType from "@/features/assets/components/select-asset-file-type";
import {
  CommonAssetFileOptionsType,
  UnitAssetFileOptionsType,
} from "@/lib/constants";
import { useApiAssets } from "@/features/assets/api/get-assets";

interface AssetsFileSearcherProps {
  value?: number | undefined;
  defaultValue?: number | undefined;
  defaultUnitId?: number | undefined;
}

const AssetsFileSearcher = ({
  value,
  defaultValue,
  defaultUnitId,
}: AssetsFileSearcherProps) => {
  const [selectedTab, setSelectedTab] = React.useState<"unit" | "common">(
    "unit",
  );

  const { data } = useApiAssets();

  return <></>;
};

export default AssetsFileSearcher;
