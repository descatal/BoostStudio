import {
  PostApiScexCompileUnitsRequest,
  PostApiScexDecompileUnitsRequest,
  ScexApi,
} from "@/api/exvs"
import { createOpenApiConfiguration } from "@/api/wrapper/api-common"

function createScexOpenApiConfiguration() {
  const configuration = createOpenApiConfiguration()
  return new ScexApi(configuration)
}

const openapi = createScexOpenApiConfiguration()

export async function compileScexUnits(
  request: PostApiScexCompileUnitsRequest
) {
  return await openapi.postApiScexCompileUnits(request)
}

export async function decompileScexUnits(
  request: PostApiScexDecompileUnitsRequest
) {
  return await openapi.postApiScexDecompileUnits(request)
}
