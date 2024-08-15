import {
  AmmoApi,
  AmmoDto,
  GetApiAmmoRequest,
  type PostApiAmmoExportPathRequest,
  type PostApiUnitStatsExportPathRequest,
} from "../exvs"
import { createOpenApiConfiguration } from "./api-common"

function createAmmoOpenApiConfiguration() {
  const configuration = createOpenApiConfiguration()
  return new AmmoApi(configuration)
}

export async function fetchAmmo(request: GetApiAmmoRequest) {
  const openapi = createAmmoOpenApiConfiguration()
  return await openapi.getApiAmmo(request)
}

export async function createAmmo(ammoDto: AmmoDto) {
  const openapi = createAmmoOpenApiConfiguration()
  return await openapi.postApiAmmo({
    postApiAmmoRequest: {
      ...ammoDto,
    },
  })
}

export async function updateAmmo(ammoDto: AmmoDto) {
  const openapi = createAmmoOpenApiConfiguration()
  return await openapi.postApiAmmoByHash({
    hash: ammoDto.hash!,
    postApiAmmoByHashRequest: {
      ...ammoDto,
      hash: ammoDto.hash!,
    },
  })
}

export async function deleteAmmo(hash: number) {
  const openapi = createAmmoOpenApiConfiguration()
  return await openapi.deleteApiAmmoByHash({
    hash: hash,
  })
}

export async function fetchAmmoOptions(unitIds: number[]) {
  const openapi = createAmmoOpenApiConfiguration()
  return await openapi.getApiAmmoOptions({
    unitIds: unitIds,
  })
}

export async function exportAmmoByPath(request: PostApiAmmoExportPathRequest) {
  const openapi = createAmmoOpenApiConfiguration()
  return await openapi.postApiAmmoExportPath({
    postApiAmmoExportPathRequest: request,
  })
}
