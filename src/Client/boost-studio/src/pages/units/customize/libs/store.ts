import { create } from "zustand"

export type UnitCustomizeModes = "information" | "script" | "model" | "misc"

type State = {
  selectedTab: UnitCustomizeModes
}

type Action = {
  setSelectedTab: (selectedTab: State["selectedTab"]) => void
}

export const useCustomizeUnitStore = create<State & Action>((set) => ({
  selectedTab: "information",
  setSelectedTab: (selectedTab) =>
    set(() => ({
      selectedTab: selectedTab,
    })),
}))
