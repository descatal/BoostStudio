import {
  GetApiPatchFilesRequest,
  GetApiPatchFilesSummaryRequest,
  GetApiTblByIdRequest,
  TblApi,
} from "../exvs"
import { createOpenApiConfiguration } from "./api-common"

function createTblOpenApiConfiguration() {
  const configuration = createOpenApiConfiguration()
  return new TblApi(configuration)
}

export async function fetchTbl(request: GetApiTblByIdRequest) {
  const openapi = createTblOpenApiConfiguration()
  return await openapi.getApiTblById(request)
}

export async function fetchPatchFileSummaries(
  request: GetApiPatchFilesSummaryRequest
) {
  const openapi = createTblOpenApiConfiguration()
  return await openapi.getApiPatchFilesSummary(request)
}

export async function fetchPatchFiles(request: GetApiPatchFilesRequest) {
  const openapi = createTblOpenApiConfiguration()
  return await openapi.getApiPatchFiles(request)
}
