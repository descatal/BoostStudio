import {
  FhmApi,
  PostApiFhmPackAssetRequest,
  PostApiFhmUnpackAssetRequest,
} from "@/api/exvs"
import { createOpenApiConfiguration } from "@/api/wrapper/api-common"

function createFhmOpenApiConfiguration() {
  const configuration = createOpenApiConfiguration()
  return new FhmApi(configuration)
}

const openapi = createFhmOpenApiConfiguration()

export async function packFhmAssets(request: PostApiFhmPackAssetRequest) {
  return await openapi.postApiFhmPackAsset(request)
}

export async function unpackFhmAssets(request: PostApiFhmUnpackAssetRequest) {
  return await openapi.postApiFhmUnpackAsset(request)
}
