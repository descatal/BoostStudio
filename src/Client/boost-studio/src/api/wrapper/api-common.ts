import {Configuration} from "@/api/exvs";

export function createOpenApiConfiguration(){
  return new Configuration({
    basePath: 'https://localhost:5001',
  });
}