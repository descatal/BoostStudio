import React from "react";
import { useTblById } from "@/features/patches/api/get-tbl";
import ExportTblDialog from "@/features/patches/components/dialogs/export-tbl-dialog";
import ResizePatchDialog from "@/features/patches/components/dialogs/resize-patch-dialog";
import { PatchFileTabs, PatchIdNameMap } from "@/pages/patches/libs/store";
import { createFileRoute } from "@tanstack/react-router";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { DataTableSkeleton } from "@/components/data-table-2/data-table-skeleton";
import PatchFilesList from "@/features/patches/components/patch-files-list";

export const Route = createFileRoute("/patches/$patchId")({
  component: PatchInformation,
});

function PatchInformation() {
  const { patchId }: { patchId: PatchFileTabs } = Route.useParams();

  const tblInfo =
    !patchId || patchId === "All" ? undefined : useTblById(patchId)?.data;

  return (
    <>
      <Card>
        <CardHeader className="px-7">
          <div className={"flex flex-row justify-between"}>
            <div>
              <CardTitle>{patchId ? PatchIdNameMap[patchId] : "All"}</CardTitle>
              <CardDescription className={"pt-2"}>
                Cumulative asset index:
                {tblInfo?.cumulativeAssetIndex ?? "-"}
              </CardDescription>
            </div>
            <div className={"sm:space-x-1 space-y-1"}>
              <ResizePatchDialog patchId={patchId} />
              <ExportTblDialog patchId={patchId} />
            </div>
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
              <PatchFilesList
                patchId={patchId === "All" ? undefined : patchId}
              />
            </React.Suspense>
          </div>
        </CardContent>
      </Card>
    </>
  );
}
