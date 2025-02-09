import {
  DeleteApiHitboxesByHashRequest,
  GetApiHitboxesRequest,
  HitboxesApi,
  PostApiHitboxesByHashRequest,
  PostApiHitboxesRequest,
  type PostApiHitboxGroupsExportPathRequest, PostApiHitboxGroupsExportRequest,
} from "../exvs"
import { createOpenApiConfiguration } from "./api-common"

function createHitboxesOpenApiConfiguration() {
  const configuration = createOpenApiConfiguration()
  return new HitboxesApi(configuration)
}

const openapi = createHitboxesOpenApiConfiguration()

export async function fetchHitboxes(request: GetApiHitboxesRequest) {
  return await openapi.getApiHitboxes(request)
}

export async function createHitbox(request: PostApiHitboxesRequest) {
  return await openapi.postApiHitboxes(request)
}

export async function updateHitbox(request: PostApiHitboxesByHashRequest) {
  return await openapi.postApiHitboxesByHash(request)
}

export async function deleteHitbox(request: DeleteApiHitboxesByHashRequest) {
  return await openapi.deleteApiHitboxesByHash(request)
}

export async function exportHitboxes(
  request: PostApiHitboxGroupsExportRequest
) {
  return await openapi.postApiHitboxGroupsExport(request)
}

export async function exportHitboxesByPath(
  request: PostApiHitboxGroupsExportPathRequest
) {
  return await openapi.postApiHitboxGroupsExportPath(request)
}
