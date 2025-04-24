import {
  AmmoApi,
  AssetsApi,
  Configuration,
  FhmApi,
  HitboxesApi,
  ProjectilesApi,
  ScexApi,
  SeriesApi,
  StatsApi,
  TblApi,
  UnitsApi,
} from "@/api/exvs";

export const BASE_CONFIG = new Configuration({
  basePath: import.meta.env.VITE_SERVER_URL ?? "",
});

export const tblApi = new TblApi(BASE_CONFIG);
export const seriesApi = new SeriesApi(BASE_CONFIG);
export const unitsApi = new UnitsApi(BASE_CONFIG);
export const statsApi = new StatsApi(BASE_CONFIG);
export const ammoApi = new AmmoApi(BASE_CONFIG);
export const hitboxesApi = new HitboxesApi(BASE_CONFIG);
export const projectilesApi = new ProjectilesApi(BASE_CONFIG);
export const scexApi = new ScexApi(BASE_CONFIG);
export const assetsApi = new AssetsApi(BASE_CONFIG);
export const fhmApi = new FhmApi(BASE_CONFIG);
