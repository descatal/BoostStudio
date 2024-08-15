import {createOpenApiConfiguration} from "@/api/wrapper/api-common";
import {ConfigsApi, GetApiConfigsRequest, PostApiConfigsRequest} from "@/api/exvs";

function createConfigsOpenApiConfiguration(){
  const configuration = createOpenApiConfiguration();
  return new ConfigsApi(configuration);
}

export async function fetchConfigs(
  request: GetApiConfigsRequest
) {
  const openapi = createConfigsOpenApiConfiguration();
  return await openapi.getApiConfigs(request)
}

export async function upsertConfig(
  request: PostApiConfigsRequest
) {
  const openapi = createConfigsOpenApiConfiguration();
  return await openapi.postApiConfigs(request)
}