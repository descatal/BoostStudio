import {
  DeleteApiPatchFilesByIdRequest,
  GetApiPatchFilesRequest,
  GetApiPatchFilesSummaryRequest,
  GetApiTblByIdRequest,
  PostApiPatchFilesRequest,
  PostApiTblExportRequest,
  TblApi,
} from "../exvs"
import { createOpenApiConfiguration } from "./api-common"

function createTblOpenApiConfiguration() {
  const configuration = createOpenApiConfiguration()
  return new TblApi(configuration)
}

const openapi = createTblOpenApiConfiguration()

export async function fetchTblById(request: GetApiTblByIdRequest) {
  return await openapi.getApiTblById(request)
}

export async function fetchPatchFileSummaries(
  request: GetApiPatchFilesSummaryRequest
) {
  return await openapi.getApiPatchFilesSummary(request)
}

export async function fetchPatchFiles(request: GetApiPatchFilesRequest) {
  return await openapi.getApiPatchFiles(request)
}

export async function createPatchFiles(request: PostApiPatchFilesRequest) {
  return await openapi.postApiPatchFiles(request)
}

export async function deletePatchFiles(
  request: DeleteApiPatchFilesByIdRequest
) {
  return await openapi.deleteApiPatchFilesById(request)
}

export async function exportTbl(request: PostApiTblExportRequest) {
  return await openapi.postApiTblExport(request)
}
