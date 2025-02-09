import { create } from "zustand"

export type InformationTabModes = "stats" | "ammo" | "projectiles" | "hitboxes"

type State = {
  selectedInformationTab: InformationTabModes
}

type Action = {
  setSelectedInformationTab: (
    selectedTab: State["selectedInformationTab"]
  ) => void
}

export const useCustomizeInformationUnitStore = create<State & Action>(
  (set) => ({
    selectedInformationTab: "stats",
    setSelectedInformationTab: (selectedInformationTab) =>
      set(() => ({
        selectedInformationTab: selectedInformationTab,
      })),
  })
)
