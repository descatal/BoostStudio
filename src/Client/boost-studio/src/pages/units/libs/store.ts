import { UnitDto } from "@/api/exvs"
import { create } from "zustand"

type State = {
  selectedUnit: UnitDto | undefined
}

type Action = {
  setSelectedUnit: (selectedUnit: State["selectedUnit"]) => void
}

export const useUnitsStore = create<State & Action>((set) => ({
  selectedUnit: undefined,
  setSelectedUnit: (selectedUnit) =>
    set(() => ({
      selectedUnit: selectedUnit,
    })),
}))
