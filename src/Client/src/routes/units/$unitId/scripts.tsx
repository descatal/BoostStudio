import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/units/$unitId/scripts')({
  component: RouteComponent,
})

function RouteComponent() {
  return <div>Hello "/units/$unitId/script"!</div>
}
