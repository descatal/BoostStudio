import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/units/$unitId/script')({
  component: RouteComponent,
})

function RouteComponent() {
  return <div>Hello "/units/$unitId/script"!</div>
}
