import {
  type PostApiUnitStatsAmmoSlotByIdRequest,
  StatsApi,
  UnitStatsApi,
  PostApiStatsByIdRequest,
  PostApiStatsOperationRequest, type PostApiStatsRequest, GetApiStatsRequest, PostApiUnitStatsAmmoSlotRequest
} from "../exvs";
import {createOpenApiConfiguration} from "@/api/wrapper/api-common";

function createUnitStatsOpenApiConfiguration(){
  const configuration = createOpenApiConfiguration();
  return new UnitStatsApi(configuration);
}

function createStatsOpenApiConfiguration(){
  const configuration = createOpenApiConfiguration();
  return new StatsApi(configuration);
}

export async function fetchUnitStats(
  unitId: number
) {
  const openapi = createUnitStatsOpenApiConfiguration();
  return await openapi.getApiUnitStatsByUnitId({
    unitId: unitId
  })
}

export async function createUnitAmmoSlot(
  data: PostApiUnitStatsAmmoSlotRequest
) {
  const openapi = createUnitStatsOpenApiConfiguration();
  return await openapi.postApiUnitStatsAmmoSlot({
    postApiUnitStatsAmmoSlotRequest: data
  })
}

export async function deleteUnitAmmoSlot(
  id: string
) {
  const openapi = createUnitStatsOpenApiConfiguration();
  return await openapi.deleteApiUnitStatsAmmoSlotById({
    id: id
  })
}

export async function updateUnitAmmoSlot(
  data: PostApiUnitStatsAmmoSlotByIdRequest
) {
  const openapi = createUnitStatsOpenApiConfiguration();
  return await openapi.postApiUnitStatsAmmoSlotById({
    id: data.id,
    postApiUnitStatsAmmoSlotByIdRequest: data
  })
}

export async function fetchStats(
  request: GetApiStatsRequest
) {
  const openapi = createStatsOpenApiConfiguration();
  return await openapi.getApiStats({
    ...request
  })
}

export async function createStats(
  data: PostApiStatsRequest
) {
  const openapi = createStatsOpenApiConfiguration();
  return await openapi.postApiStats({
    postApiStatsRequest: data
  })
}

export async function updateStats(
  id: string,
  data: PostApiStatsByIdRequest
) {
  const openapi = createStatsOpenApiConfiguration();
  return await openapi.postApiStatsById({
    id: id,
    postApiStatsByIdRequest: data
  })
}

export async function deleteStats(
  id: string,
) {
  const openapi = createStatsOpenApiConfiguration();
  return await openapi.deleteApiStatsById({
    id: id
  })
}