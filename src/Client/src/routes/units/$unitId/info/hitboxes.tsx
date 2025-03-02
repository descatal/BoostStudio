import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute("/units/$unitId/info/hitboxes")({
  component: RouteComponent,
});

function RouteComponent() {
  return <div>Hello "/units/$unitId/info/hitboxes"!</div>;
}
