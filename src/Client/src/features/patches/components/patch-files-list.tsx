import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import React from "react";
import {
  PatchFileTabs,
  PatchIdNameMap,
} from "@/features/patches/libs/constants";
import ResizePatchDialog from "./dialogs/resize-patch-dialog";
import ExportTblDialog from "./dialogs/export-tbl-dialog";
import PatchFilesTable from "@/features/patches/components/patch-files-table/table";
import { useQuery } from "@tanstack/react-query";
import { getApiTblByIdOptions } from "@/api/exvs/@tanstack/react-query.gen";

interface PatchFilesListProps {
  patchId: PatchFileTabs;
}

const PatchFilesList = ({ patchId }: PatchFilesListProps) => {
  const query = useQuery({
    ...getApiTblByIdOptions({
      path: {
        id: !patchId || patchId === "All" ? "Base" : patchId,
      },
    }),
  });

  const tblInfo = !patchId || patchId === "All" ? undefined : query.data;

  return (
    <>
      <Card>
        <CardHeader className="px-7">
          <div className={"flex flex-row justify-between"}>
            <div>
              <CardTitle>{PatchIdNameMap[patchId]}</CardTitle>
              <CardDescription className={"pt-2"}>
                Cumulative asset index:
                {tblInfo?.cumulativeAssetIndex ?? "-"}
              </CardDescription>
            </div>
            {patchId !== "All" && (
              <div className={"sm:space-x-1 space-y-1"}>
                <ResizePatchDialog patchId={patchId} />
                <ExportTblDialog patchId={patchId} />
              </div>
            )}
          </div>
        </CardHeader>
        <CardContent>
          <PatchFilesTable patchId={patchId === "All" ? undefined : patchId} />
        </CardContent>
      </Card>
    </>
  );
};

export default PatchFilesList;
