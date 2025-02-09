import {
  AmmoApi,
  AmmoDto,
  GetApiAmmoRequest, PostApiAmmoByHashRequest,
  type PostApiAmmoExportPathRequest, PostApiAmmoExportRequest,
  type PostApiUnitStatsExportPathRequest,
} from "../exvs"
import { createOpenApiConfiguration } from "./api-common"

function createAmmoOpenApiConfiguration() {
  const configuration = createOpenApiConfiguration()
  return new AmmoApi(configuration)
}

const openapi = createAmmoOpenApiConfiguration()

export async function fetchAmmo(request: GetApiAmmoRequest) {
  return await openapi.getApiAmmo(request)
}

export async function createAmmo(ammoDto: AmmoDto) {
  return await openapi.postApiAmmo({
    createAmmoCommand: {
      ...ammoDto,
    },
  })
}

export async function updateAmmo(ammoDto: AmmoDto) {
  return await openapi.postApiAmmoByHash({
    hash: ammoDto.hash!,
    updateAmmoCommand: {
      ...ammoDto,
      hash: ammoDto.hash!,
    },
  })
}

export async function deleteAmmo(hash: number) {
  return await openapi.deleteApiAmmoByHash({
    hash: hash,
  })
}

export async function fetchAmmoOptions(unitIds: number[]) {
  return await openapi.getApiAmmoOptions({
    unitIds: unitIds,
  })
}

export async function exportAmmo(request: PostApiAmmoExportRequest) {
  return await openapi.postApiAmmoExport(request)
}

export async function exportAmmoByPath(request: PostApiAmmoExportPathRequest) {
  return await openapi.postApiAmmoExportPath(request)
}
