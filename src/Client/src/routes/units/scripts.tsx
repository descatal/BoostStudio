import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/units/scripts')({
  component: RouteComponent,
})

function RouteComponent() {
  return <div>Hello "/units/scripts"!</div>
}
