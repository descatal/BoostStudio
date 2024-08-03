import {createOpenApiConfiguration} from "./api-common";
import {
  DeleteApiHitboxesByHashRequest,
  DeleteApiProjectilesByHashRequest,
  GetApiHitboxesRequest,
  HitboxesApi,
  PostApiHitboxesByHashOperationRequest, PostApiHitboxesOperationRequest, PostApiProjectilesOperationRequest,
} from "../exvs";

function createProjectilesOpenApiConfiguration(){
  const configuration = createOpenApiConfiguration();
  return new HitboxesApi(configuration);
}

export async function fetchHitboxes(
  request: GetApiHitboxesRequest
) {
  const openapi = createProjectilesOpenApiConfiguration();
  return await openapi.getApiHitboxes(request)
}

export async function createHitbox(
  request: PostApiHitboxesOperationRequest
) {
  const openapi = createProjectilesOpenApiConfiguration();
  return await openapi.postApiHitboxes(request)
}

export async function updateHitbox(
  request: PostApiHitboxesByHashOperationRequest
) {
  const openapi = createProjectilesOpenApiConfiguration();
  return await openapi.postApiHitboxesByHash(request)
}

export async function deleteHitbox(
  request: DeleteApiHitboxesByHashRequest
) {
  const openapi = createProjectilesOpenApiConfiguration();
  return await openapi.deleteApiHitboxesByHash(request)
}