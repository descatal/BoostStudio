import { Configuration } from "@/api/exvs"

export function createOpenApiConfiguration() {
  return new Configuration({
    basePath: import.meta.env.VITE_SERVER_URL ?? "",
  })
}
