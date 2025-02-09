import {Configuration, TblApi} from "@/api/exvs";

export const BASE_CONFIG = new Configuration({
  basePath: "https://localhost:5001"
});

export const tblApi = new TblApi(BASE_CONFIG);
