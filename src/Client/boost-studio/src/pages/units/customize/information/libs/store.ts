import { create } from "zustand"

export type InformationTabModes = "stats" | "ammo" | "projectiles" | "hitboxes"

type State = {
  selectedTab: "stats" | "ammo" | "projectiles" | "hitboxes"
}

type Action = {
  setSelectedTab: (selectedTab: State["selectedTab"]) => void
}

export const useCustomizeInformationUnitStore = create<State & Action>(
  (set) => ({
    selectedTab: "stats",
    setSelectedTab: (selectedTab) =>
      set(() => ({
        selectedTab: selectedTab,
      })),
  })
)
