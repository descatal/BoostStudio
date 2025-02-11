import { Configuration, SeriesApi, TblApi } from "@/api/exvs"

export const BASE_CONFIG = new Configuration({
  basePath: import.meta.env.VITE_SERVER_URL ?? "",
})

export const tblApi = new TblApi(BASE_CONFIG)
export const seriesApi = new SeriesApi(BASE_CONFIG)
