import React from "react";
import { createFileRoute } from "@tanstack/react-router";
import PatchFilesList from "@/features/patches/components/patch-files-list";
import { PatchFileTabs } from "@/features/patches/libs/constants";

export const Route = createFileRoute("/patches/$patchId")({
  component: PatchInformation,
});

function PatchInformation() {
  const { patchId }: { patchId: PatchFileTabs } = Route.useParams();
  return <PatchFilesList patchId={patchId} />;
}
