import React from "react"

import {
  Select,
  SelectContent,
  SelectGroup,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select"

import {
  AssetFileType,
  CombinedAssetFileOptions,
  CommonAssetFileOptions,
  UnitAssetFileOptions,
  UnitAssetFileOptionsType,
} from "../../libs/constants"

interface SelectAssetFileTypeProps
  extends React.ComponentPropsWithRef<typeof Select> {
  type: "combined" | "unit" | "common"
  selectedUnitAssetFileType: AssetFileType
  setSelectedUnitAssetFileType: (type: AssetFileType) => void
}

const SelectAssetFileType = ({
  type = "combined",
  selectedUnitAssetFileType,
  setSelectedUnitAssetFileType,
  ...props
}: SelectAssetFileTypeProps) => {
  const assetFileOptions =
    type === "unit"
      ? UnitAssetFileOptions
      : type === "common"
        ? CommonAssetFileOptions
        : CombinedAssetFileOptions

  return (
    <Select
      value={selectedUnitAssetFileType}
      onValueChange={(e) => setSelectedUnitAssetFileType(e as AssetFileType)}
      {...props}
    >
      <SelectTrigger className="capitalize">
        <SelectValue placeholder="Select File Type" />
      </SelectTrigger>
      <SelectContent className={"max-h-[10rem] overflow-y-auto"}>
        <SelectGroup>
          {Object.values(assetFileOptions).map((type) => (
            <SelectItem key={type} value={type} className="capitalize">
              {type}
            </SelectItem>
          ))}
        </SelectGroup>
      </SelectContent>
    </Select>
  )
}

export default SelectAssetFileType
