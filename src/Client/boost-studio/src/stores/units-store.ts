import {create} from "zustand";
import {UnitSummaryVm} from "@/api/exvs";

type UnitsStore = {
  units: UnitSummaryVm[];
  setUnits: () => void;
};

const useUnitsStore = create<UnitsStore>((set) => ({
  units: [],
  setUnits: () => set((state) => ({ units: state.units })),
}));

export default useUnitsStore;