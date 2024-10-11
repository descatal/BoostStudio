import {
  DeleteApiProjectilesByHashRequest,
  GetApiProjectilesRequest,
  PostApiProjectilesByHashRequest,
  PostApiProjectilesRequest,
  PostApiUnitProjectilesExportPathRequest,
  ProjectilesApi,
} from "../exvs"
import { createOpenApiConfiguration } from "./api-common"

function createProjectilesOpenApiConfiguration() {
  const configuration = createOpenApiConfiguration()
  return new ProjectilesApi(configuration)
}

export async function fetchProjectiles(request: GetApiProjectilesRequest) {
  const openapi = createProjectilesOpenApiConfiguration()
  return await openapi.getApiProjectiles(request)
}

export async function createProjectile(request: PostApiProjectilesRequest) {
  const openapi = createProjectilesOpenApiConfiguration()
  return await openapi.postApiProjectiles(request)
}

export async function updateProjectile(
  request: PostApiProjectilesByHashRequest
) {
  const openapi = createProjectilesOpenApiConfiguration()
  return await openapi.postApiProjectilesByHash(request)
}

export async function deleteProjectile(
  request: DeleteApiProjectilesByHashRequest
) {
  const openapi = createProjectilesOpenApiConfiguration()
  return await openapi.deleteApiProjectilesByHash(request)
}

export async function exportProjectilesByPath(
  request: PostApiUnitProjectilesExportPathRequest
) {
  const openapi = createProjectilesOpenApiConfiguration()
  return await openapi.postApiUnitProjectilesExportPath(request)
}
