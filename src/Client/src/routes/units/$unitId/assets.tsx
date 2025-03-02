import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/units/$unitId/assets')({
  component: RouteComponent,
})

function RouteComponent() {
  return <div>Hello "/units/$unitId/assets"!</div>
}
