import { createFileRoute } from "@tanstack/react-router";
import PackUnpackAssets from "@/features/assets/components/pack-unpack-assets";

export const Route = createFileRoute("/units/assets")({
  component: RouteComponent,
});

function RouteComponent() {
  return <PackUnpackAssets />;
}
