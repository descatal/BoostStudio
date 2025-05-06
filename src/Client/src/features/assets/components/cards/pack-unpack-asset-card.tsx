import React from "react";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import AssetFilesSearcher from "@/features/assets/components/asset-files-searcher";
import { Separator } from "@/components/ui/separator";
import { Label } from "@/components/ui/label";
import SelectedAssetFileList from "@/features/assets/components/selected-asset-file-list";
import { EnhancedButton } from "@/components/ui/enhanced-button";
import { LuPackage, LuPackageOpen } from "react-icons/lu";
import { Icons } from "@/components/icons";
import { useMutation } from "@tanstack/react-query";
import { postApiFhmPackAssetMutation } from "@/api/exvs/@tanstack/react-query.gen";
import { toast } from "@/hooks/use-toast";
import { AssetFileVm } from "@/api/exvs";

interface Props extends React.ComponentPropsWithoutRef<typeof Card> {
  type: "Pack" | "Unpack";
  unitIds?: number[];
}

const PackUnpackAssetCard = ({ type, unitIds, ...props }: Props) => {
  const [selectedFiles, setSelectedFiles] = React.useState<
    AssetFileVm[] | undefined
  >();

  const handleSuccess = () => {
    toast({
      title: "Success",
      description: `Successfully ${type === "Pack" ? "packed" : "unpacked"} assets to staging directory!`,
    });
  };

  const mutation =
    type === "Pack"
      ? useMutation({
          ...postApiFhmPackAssetMutation(),
          onSuccess: handleSuccess,
        })
      : useMutation({
          ...postApiFhmPackAssetMutation(),
          onSuccess: handleSuccess,
        });

  return (
    <Card {...props}>
      <CardHeader>
        <CardTitle>{type} Assets</CardTitle>
        <CardDescription>
          {type} asset files to {type === "Pack" ? ".fhm container" : "binary"}{" "}
          format from {type === "Pack" ? "working" : "staging"} directory to{" "}
          {type === "Pack" ? "staging" : "working"} directory.
        </CardDescription>
      </CardHeader>
      <CardContent>
        <div className="grid gap-6">
          <AssetFilesSearcher
            unitIds={unitIds}
            setResultAssetFiles={setSelectedFiles}
          />
          <Separator />
          <div className={"space-y-2"}>
            <Label>Selected File Hashes</Label>
            <SelectedAssetFileList value={selectedFiles} />
          </div>
          <EnhancedButton
            className={"w-full"}
            effect={"expandIcon"}
            variant={"default"}
            icon={type === "Pack" ? LuPackage : LuPackageOpen}
            iconPlacement={"right"}
            disabled={mutation.isPending}
            onClick={() => {
              if (!selectedFiles || selectedFiles.length <= 0) {
                return;
              }

              type === "Pack"
                ? mutation.mutate({
                    body: {
                      assetFileHashes: selectedFiles.map((x) => x.hash) ?? [],
                      replaceStaging: true,
                    },
                  })
                : mutation.mutate({
                    body: {
                      assetFileHashes: selectedFiles.map((x) => x.hash) ?? [],
                      replaceStaging: true,
                    },
                  });
            }}
          >
            {mutation.isPending && (
              <Icons.spinner
                className="mr-2 size-4 animate-spin"
                aria-hidden="true"
              />
            )}
            {type}
          </EnhancedButton>
        </div>
      </CardContent>
    </Card>
  );
};

export default PackUnpackAssetCard;
