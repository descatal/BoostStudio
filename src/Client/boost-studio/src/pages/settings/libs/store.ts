import { create } from "zustand"

type State = {
  moddedBoostDirectory: string
}

type Action = {
  updateModdedBoostDirectory: (
    moddedBoostDirectory: State["moddedBoostDirectory"]
  ) => void
}

const useSettingsStore = create<State & Action>((set) => ({
  moddedBoostDirectory: "",
  updateModdedBoostDirectory: (moddedBoostDirectory) =>
    set(() => ({ moddedBoostDirectory: moddedBoostDirectory })),
}))
