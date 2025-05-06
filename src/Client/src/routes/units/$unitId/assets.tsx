import { createFileRoute } from "@tanstack/react-router";
import React from "react";
import PackUnpackAssets from "@/features/assets/components/pack-unpack-assets";

export const Route = createFileRoute("/units/$unitId/assets")({
  component: RouteComponent,
});

function RouteComponent() {
  const { unitId }: { unitId: number } = Route.useParams();
  return <PackUnpackAssets unitIds={[Number(unitId)]} />;
}
