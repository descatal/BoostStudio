import { BASE_PATH, Configuration, UnitsApi } from "@/api/exvs"
import { createStore } from "zustand"

const openApiConfiguration = new Configuration({
  basePath: BASE_PATH,
})

export interface AppProps {
  unitsApi: UnitsApi
}

export interface AppState extends AppProps {}

export type AppStore = ReturnType<typeof createAppStore>

export const createAppStore = (initProps?: Partial<AppProps>) => {
  const DEFAULT_PROPS: AppProps = {
    unitsApi: new UnitsApi(openApiConfiguration),
  }
  return createStore<AppState>()((set) => ({
    ...DEFAULT_PROPS,
    ...initProps,
  }))
}