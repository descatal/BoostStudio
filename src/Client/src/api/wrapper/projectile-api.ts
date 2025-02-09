import {
  DeleteApiProjectilesByHashRequest,
  GetApiProjectilesRequest,
  PostApiProjectilesByHashRequest,
  PostApiProjectilesRequest,
  PostApiUnitProjectilesExportPathRequest,
  PostApiUnitProjectilesExportRequest,
  ProjectilesApi,
} from "../exvs"
import {createOpenApiConfiguration} from "./api-common"

function createProjectilesOpenApiConfiguration() {
  const configuration = createOpenApiConfiguration()
  return new ProjectilesApi(configuration)
}

const openapi = createProjectilesOpenApiConfiguration()

export async function fetchProjectiles(request: GetApiProjectilesRequest) {
  return await openapi.getApiProjectiles(request)
}

export async function createProjectile(request: PostApiProjectilesRequest) {
  return await openapi.postApiProjectiles(request)
}

export async function updateProjectile(
  request: PostApiProjectilesByHashRequest
) {
  return await openapi.postApiProjectilesByHash(request)
}

export async function deleteProjectile(
  request: DeleteApiProjectilesByHashRequest
) {
  return await openapi.deleteApiProjectilesByHash(request)
}

export async function exportProjectiles(
  request: PostApiUnitProjectilesExportRequest
) {
  return await openapi.postApiUnitProjectilesExport(request)
}

export async function exportProjectilesByPath(
  request: PostApiUnitProjectilesExportPathRequest
) {
  return await openapi.postApiUnitProjectilesExportPath(request)
}
