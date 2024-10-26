import {
  PostApiPsarcPackPatchFilesRequest,
  PostApiPsarcUnpackPatchFilesRequest,
  PsarcApi,
} from "@/api/exvs"
import { createOpenApiConfiguration } from "@/api/wrapper/api-common"

function createPsarcOpenApiConfiguration() {
  const configuration = createOpenApiConfiguration()
  return new PsarcApi(configuration)
}

const openapi = createPsarcOpenApiConfiguration()

export async function packPsarcByPatchFiles(
  request: PostApiPsarcPackPatchFilesRequest
) {
  return await openapi.postApiPsarcPackPatchFiles(request)
}

export async function unpackPsarcByPatchFiles(
  request: PostApiPsarcUnpackPatchFilesRequest
) {
  return await openapi.postApiPsarcUnpackPatchFiles(request)
}
