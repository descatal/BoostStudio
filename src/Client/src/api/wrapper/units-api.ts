import {createOpenApiConfiguration} from "./api-common";
import {GetApiUnitsRequest, PostApiUnitProjectilesExportRequest, UnitsApi} from "../exvs";

function createUnitsOpenApiConfiguration(){
  const configuration = createOpenApiConfiguration();
  return new UnitsApi(configuration);
}

export async function fetchUnits(
  request: GetApiUnitsRequest
) {
  const openapi = createUnitsOpenApiConfiguration();
  return await openapi.getApiUnits({
    ...request
  })
}

export async function fetchUnitById(
  unitId: number
) {
  const openapi = createUnitsOpenApiConfiguration();
  return await openapi.getApiUnitsByUnitId({
    unitId: unitId
  })
}

