import * as React from "react"
import { AssetFileType, AssetFileVm, UnitDto } from "@/api/exvs"
import { fetchAssetFiles } from "@/api/wrapper/asset-api"
import AssetFilesSearcher from "@/pages/common/components/custom/asset-files-searcher"
import SelectAssetFileType from "@/pages/common/components/selects/select-asset-file-type"
import {
  CommonAssetFileOptions,
  CommonAssetFileOptionsType,
  UnitAssetFileOptions,
  UnitAssetFileOptionsType,
} from "@/pages/common/libs/constants"
import UnitSwitcher from "@/pages/units/customize/components/unit-switcher"

import { Button } from "@/components/ui/button"
import { Label } from "@/components/ui/label"
import { Popover, PopoverContent } from "@/components/ui/popover"
import { Separator } from "@/components/ui/separator"
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs"
import { HashInput } from "@/components/custom/hash-input"

interface SearchAssetFilePopoverProps
  extends React.ComponentPropsWithRef<typeof Popover> {
  setAssetFile: (assetFile: AssetFileVm | undefined) => void
}

export function SearchAssetFilePopover({
  setAssetFile,
  children,
}: SearchAssetFilePopoverProps) {
  const [open, setOpen] = React.useState(false)

  const [selectedAssetFile, setSelectedAssetFile] = React.useState<
    AssetFileVm | undefined
  >()

  return (
    <Popover open={open} onOpenChange={setOpen}>
      <PopoverContent>
        <div className={"space-y-4"}>
          <AssetFilesSearcher
            setResultAssetFiles={(assetFile) => {
              if (assetFile && assetFile.length > 0) {
                setSelectedAssetFile(assetFile[0])
              }
            }}
          />
          <Separator />
          <div className={"space-y-2"}>
            <Label>File Hash</Label>
            <HashInput
              readonly={true}
              className={`${selectedAssetFile ? "border-green-800" : "border-red-800"}`}
              initialMode={"hex"}
              initialValue={selectedAssetFile?.hash}
              placeholder={"e.g. 1A42E312"}
            />
          </div>
          <Button
            disabled={!selectedAssetFile}
            className={"w-full"}
            onClick={() => {
              setAssetFile(selectedAssetFile)
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
