import {
  AssetsApi,
  GetApiAssetsByHashRequest,
  GetApiAssetsRequest,
} from "../exvs"
import { createOpenApiConfiguration } from "./api-common"

function createAssetsOpenApiConfiguration() {
  const configuration = createOpenApiConfiguration()
  return new AssetsApi(configuration)
}

export async function fetchAssetFiles(request: GetApiAssetsRequest) {
  const openapi = createAssetsOpenApiConfiguration()
  return await openapi.getApiAssets(request)
}

export async function fetchAssetFilesByHash(
  request: GetApiAssetsByHashRequest
) {
  const openapi = createAssetsOpenApiConfiguration()
  return await openapi.getApiAssetsByHash(request)
}
