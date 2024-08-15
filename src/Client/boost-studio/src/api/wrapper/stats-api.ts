import { createOpenApiConfiguration } from "@/api/wrapper/api-common"

import {
  GetApiStatsRequest,
  PostApiStatsByIdRequest,
  PostApiStatsOperationRequest,
  PostApiUnitStatsAmmoSlotRequest,
  PostApiUnitStatsExportPathOperationRequest,
  StatsApi,
  type PostApiStatsRequest,
  type PostApiUnitStatsAmmoSlotByIdRequest,
  type PostApiUnitStatsExportPathRequest,
} from "../exvs"
import {
  GetApiUnitStatsByUnitIdRequest,
  GetApiUnitStatsRequest,
  UnitStatsApi,
} from "../exvs/apis/UnitStatsApi"

function createUnitStatsOpenApiConfiguration() {
  const configuration = createOpenApiConfiguration()
  return new UnitStatsApi(configuration)
}

function createStatsOpenApiConfiguration() {
  const configuration = createOpenApiConfiguration()
  return new StatsApi(configuration)
}

export async function fetchUnitStats(request: GetApiUnitStatsRequest) {
  const openapi = createUnitStatsOpenApiConfiguration()
  return await openapi.getApiUnitStats(request)
}

export async function fetchUnitStatsByUnitId(
  request: GetApiUnitStatsByUnitIdRequest
) {
  const openapi = createUnitStatsOpenApiConfiguration()
  return await openapi.getApiUnitStatsByUnitId(request)
}

export async function createUnitAmmoSlot(
  data: PostApiUnitStatsAmmoSlotRequest
) {
  const openapi = createUnitStatsOpenApiConfiguration()
  return await openapi.postApiUnitStatsAmmoSlot({
    postApiUnitStatsAmmoSlotRequest: data,
  })
}

export async function deleteUnitAmmoSlot(id: string) {
  const openapi = createUnitStatsOpenApiConfiguration()
  return await openapi.deleteApiUnitStatsAmmoSlotById({
    id: id,
  })
}

export async function updateUnitAmmoSlot(
  data: PostApiUnitStatsAmmoSlotByIdRequest
) {
  const openapi = createUnitStatsOpenApiConfiguration()
  return await openapi.postApiUnitStatsAmmoSlotById({
    id: data.id,
    postApiUnitStatsAmmoSlotByIdRequest: data,
  })
}

export async function fetchStats(request: GetApiStatsRequest) {
  const openapi = createStatsOpenApiConfiguration()
  return await openapi.getApiStats({
    ...request,
  })
}

export async function createStats(data: PostApiStatsRequest) {
  const openapi = createStatsOpenApiConfiguration()
  return await openapi.postApiStats({
    postApiStatsRequest: data,
  })
}

export async function updateStats(id: string, data: PostApiStatsByIdRequest) {
  const openapi = createStatsOpenApiConfiguration()
  return await openapi.postApiStatsById({
    id: id,
    postApiStatsByIdRequest: data,
  })
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
  return await openapi.postApiUnitStatsExportPath({
    postApiUnitStatsExportPathRequest: request,
  })
}
