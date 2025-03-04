import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import React from "react";
import { useTblById } from "@/features/patches/api/get-tbl";
import {
  PatchFileTabs,
  PatchIdNameMap,
} from "@/features/patches/libs/constants";
import ResizePatchDialog from "./dialogs/resize-patch-dialog";
import ExportTblDialog from "./dialogs/export-tbl-dialog";
import { DataTableSkeleton } from "@/components/data-table-2/data-table-skeleton";
import PatchFilesTable from "@/features/patches/components/patch-files-table/patch-files-table";

interface PatchFilesListProps {
  patchId: PatchFileTabs;
}

const PatchFilesList = ({ patchId }: PatchFilesListProps) => {
  const tblInfo =
    !patchId || patchId === "All" ? undefined : useTblById(patchId)?.data;

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
          <div className="space-y-4">
            <React.Suspense
              fallback={
                <DataTableSkeleton
                  columnCount={6}
                  searchableColumnCount={1}
                  filterableColumnCount={2}
                  cellWidths={[
                    "10rem",
                    "40rem",
                    "12rem",
                    "12rem",
                    "8rem",
                    "8rem",
                  ]}
                  shrinkZero
                />
              }
            >
              <PatchFilesTable
                patchId={patchId === "All" ? undefined : patchId}
              />
            </React.Suspense>
          </div>
        </CardContent>
      </Card>
    </>
  );
};

export default PatchFilesList;
