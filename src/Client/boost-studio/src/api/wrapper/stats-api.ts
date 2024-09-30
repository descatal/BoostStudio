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
  type PostApiUnitStatsExportPathRequest,
} from "../exvs"

function createStatsOpenApiConfiguration() {
  const configuration = createOpenApiConfiguration()
  return new StatsApi(configuration)
}

export async function fetchUnitStats(request: GetApiUnitStatsRequest) {
  const openapi = createStatsOpenApiConfiguration()
  return await openapi.getApiUnitStats(request)
}

export async function fetchUnitStatsByUnitId(
  request: GetApiUnitStatsByUnitIdRequest
) {
  const openapi = createStatsOpenApiConfiguration()
  return await openapi.getApiUnitStatsByUnitId(request)
}

export async function createUnitAmmoSlot(
  data: PostApiUnitStatsAmmoSlotRequest
) {
  const openapi = createStatsOpenApiConfiguration()
  return await openapi.postApiUnitStatsAmmoSlot(data)
}

export async function deleteUnitAmmoSlot(id: string) {
  const openapi = createStatsOpenApiConfiguration()
  return await openapi.deleteApiUnitStatsAmmoSlotById({
    id: id,
  })
}

export async function updateUnitAmmoSlot(
  data: PostApiUnitStatsAmmoSlotByIdRequest
) {
  const openapi = createStatsOpenApiConfiguration()
  return await openapi.postApiUnitStatsAmmoSlotById(data)
}

export async function fetchStats(request: GetApiStatsRequest) {
  const openapi = createStatsOpenApiConfiguration()
  return await openapi.getApiStats({
    ...request,
  })
}

export async function createStats(data: PostApiStatsRequest) {
  const openapi = createStatsOpenApiConfiguration()
  return await openapi.postApiStats(data)
}

export async function updateStats(data: PostApiStatsByIdRequest) {
  const openapi = createStatsOpenApiConfiguration()
  return await openapi.postApiStatsById(data)
}

export async function deleteStats(id: string) {
  const openapi = createStatsOpenApiConfiguration()
  return await openapi.deleteApiStatsById({
    id: id,
  })
}

export async function exportStatsByPath(
  request: PostApiUnitStatsExportPathRequest
) {
  const openapi = createStatsOpenApiConfiguration()
  return await openapi.postApiUnitStatsExportPath(request)
}
