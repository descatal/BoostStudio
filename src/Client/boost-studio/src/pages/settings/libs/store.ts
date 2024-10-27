import { create } from "zustand"

type State = {
  stagingDirectory: string | undefined
  workingDirectory: string | undefined
  productionDirectory: string | undefined
  scriptDirectory: string | undefined
}

type Action = {
  updateStagingDirectory: (stagingDirectory: State["stagingDirectory"]) => void
  updateWorkingDirectory: (workingDirectory: State["workingDirectory"]) => void
  updateProductionDirectory: (
    productionDirectory: State["productionDirectory"]
  ) => void
  updateScriptDirectory: (scriptDirectory: State["scriptDirectory"]) => void
}

export const useSettingsStore = create<State & Action>((set) => ({
  stagingDirectory: undefined,
  workingDirectory: undefined,
  productionDirectory: undefined,
  scriptDirectory: undefined,
  updateStagingDirectory: (stagingDirectory) =>
    set(() => ({ stagingDirectory: stagingDirectory })),
  updateWorkingDirectory: (workingDirectory) =>
    set(() => ({ workingDirectory: workingDirectory })),
  updateProductionDirectory: (productionDirectory) =>
    set(() => ({ productionDirectory: productionDirectory })),
  updateScriptDirectory: (scriptDirectory) =>
    set(() => ({ scriptDirectory: scriptDirectory })),
}))
