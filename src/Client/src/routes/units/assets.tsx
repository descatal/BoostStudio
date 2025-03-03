import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute("/units/assets")({
  component: RouteComponent,
});

function RouteComponent() {
  return <div>Hello "/units/assets"!</div>;
}
