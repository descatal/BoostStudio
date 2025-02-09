import { create } from "zustand"

type State = {
  openExportDialog: boolean
  statsSelected: boolean
  ammoSelected: boolean
  projectilesSelected: boolean
  hitboxesSelected: boolean
}

type Action = {
  setOpenExportDialog: (openExportDialog: State["openExportDialog"]) => void
  setStatsSelected: (statsSelected: State["statsSelected"]) => void
  setAmmoSelected: (ammoSelected: State["ammoSelected"]) => void
  setProjectilesSelected: (
    projectilesSelected: State["projectilesSelected"]
  ) => void
  setHitboxesSelected: (hitboxesSelected: State["hitboxesSelected"]) => void
}

export const useExportDialogStore = create<State & Action>((set) => ({
  openExportDialog: false,
  statsSelected: true,
  ammoSelected: true,
  projectilesSelected: true,
  hitboxesSelected: true,
  setOpenExportDialog: (openExportDialog) =>
    set(() => ({
      openExportDialog: openExportDialog,
    })),
  setStatsSelected: (statsSelected) =>
    set(() => ({
      statsSelected: statsSelected,
    })),
  setAmmoSelected: (ammoSelected) =>
    set(() => ({
      ammoSelected: ammoSelected,
    })),
  setProjectilesSelected: (projectilesSelected) =>
    set(() => ({
      projectilesSelected: projectilesSelected,
    })),
  setHitboxesSelected: (hitboxesSelected) =>
    set(() => ({
      hitboxesSelected: hitboxesSelected,
    })),
}))
