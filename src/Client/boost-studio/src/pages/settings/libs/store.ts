import { create } from "zustand"

type State = {
  stagingDirectory: string | undefined
  workingDirectory: string | undefined
}

type Action = {
  updateStagingDirectory: (stagingDirectory: State["stagingDirectory"]) => void
  updateWorkingDirectory: (workingDirectory: State["workingDirectory"]) => void
}

export const useSettingsStore = create<State & Action>((set) => ({
  stagingDirectory: undefined,
  workingDirectory: undefined,
  updateStagingDirectory: (stagingDirectory) =>
    set(() => ({ stagingDirectory: stagingDirectory })),
  updateWorkingDirectory: (workingDirectory) =>
    set(() => ({ workingDirectory: workingDirectory })),
}))
