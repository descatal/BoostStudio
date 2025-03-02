import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/units/$unitId/info/projectiles')({
  component: RouteComponent,
})

function RouteComponent() {
  return <div>Hello "/units/$unitId/info/projectiles"!</div>
}
