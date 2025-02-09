import { create } from "zustand"

type State = {
  openExportDialog: boolean
}

type Action = {
  setOpenExportDialog: (openExportAlert: State["openExportDialog"]) => void
}

export const useProjectilesStore = create<State & Action>((set) => ({
  openExportDialog: false,
  setOpenExportDialog: (openExportDialog) =>
    set(() => ({ openExportDialog: openExportDialog })),
}))
