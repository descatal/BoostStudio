import { createOpenApiConfiguration } from "@/api/wrapper/api-common"

import {
  GetApiStatsRequest,
  GetApiUnitStatsByUnitIdRequest,
  GetApiUnitStatsRequest,
  PostApiStatsByIdRequest,
  PostApiUnitStatsAmmoSlotRequest,
  StatsApi,
  type PostApiStatsRequest,
  type PostApiUnitStatsAmmoSlotByIdRequest,
  type PostApiUnitStatsExportPathRequest, PostApiUnitProjectilesExportRequest, PostApiUnitStatsExportRequest,
} from "../exvs"

function createStatsOpenApiConfiguration() {
  const configuration = createOpenApiConfiguration()
  return new StatsApi(configuration)
}

const openapi = createStatsOpenApiConfiguration()

export async function fetchUnitStats(request: GetApiUnitStatsRequest) {
  return await openapi.getApiUnitStats(request)
}

export async function fetchUnitStatsByUnitId(
  request: GetApiUnitStatsByUnitIdRequest
) {
  return await openapi.getApiUnitStatsByUnitId(request)
}

export async function createUnitAmmoSlot(
  data: PostApiUnitStatsAmmoSlotRequest
) {
  return await openapi.postApiUnitStatsAmmoSlot(data)
}

export async function deleteUnitAmmoSlot(id: string) {
  return await openapi.deleteApiUnitStatsAmmoSlotById({
    id: id,
  })
}

export async function updateUnitAmmoSlot(
  data: PostApiUnitStatsAmmoSlotByIdRequest
) {
  return await openapi.postApiUnitStatsAmmoSlotById(data)
}

export async function fetchStats(request: GetApiStatsRequest) {
  return await openapi.getApiStats({
    ...request,
  })
}

export async function createStats(data: PostApiStatsRequest) {
  return await openapi.postApiStats(data)
}

export async function updateStats(data: PostApiStatsByIdRequest) {
  return await openapi.postApiStatsById(data)
}

export async function deleteStats(id: string) {
  return await openapi.deleteApiStatsById({
    id: id,
  })
}

export async function exportUnitStats(
  request: PostApiUnitStatsExportRequest
) {
  return await openapi.postApiUnitStatsExport(request)
}
