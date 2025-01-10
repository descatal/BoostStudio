import { UnitSummaryVm } from "@/api/exvs"
import { create } from "zustand"

export const CustomizeSections = ["info", "script", "assets"] as const
export type CustomizeSections = (typeof CustomizeSections)[number]

type State = {
  customizeSection: CustomizeSections
  selectedUnits: UnitSummaryVm[] | undefined
}

type Action = {
  setCustomizeSection: (customizeSection: State["customizeSection"]) => void
  setSelectedUnits: (selectedUnits: State["selectedUnits"]) => void
}

export const useUnitsStore = create<State & Action>((set) => ({
  customizeSection: "info",
  selectedUnits: undefined,
  setCustomizeSection: (customizeSection) =>
    set(() => ({
      customizeSection: customizeSection,
    })),
  setSelectedUnits: (selectedUnits) =>
    set(() => ({
      selectedUnits: selectedUnits,
    })),
}))
