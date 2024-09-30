import { UnitDto } from "@/api/exvs"
import { create } from "zustand"

type State = {
  selectedUnits: UnitDto[] | undefined
}

type Action = {
  setSelectedUnits: (selectedUnits: State["selectedUnits"]) => void
}

export const useUnitsStore = create<State & Action>((set) => ({
  selectedUnits: undefined,
  setSelectedUnits: (selectedUnits) =>
    set(() => ({
      selectedUnits: selectedUnits,
    })),
}))
