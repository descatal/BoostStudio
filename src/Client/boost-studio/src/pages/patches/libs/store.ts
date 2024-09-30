import { create } from "zustand"

export const PatchIdNameMap = {
  Base: "Base Game",
  Patch1: "patch_01_00",
  Patch2: "patch_02_00",
  Patch3: "patch_03_00",
  Patch4: "patch_04_00",
  Patch5: "patch_05_00",
  Patch6: "patch_06_00",
}

export type PatchFileVersions =
  | "Base"
  | "Patch1"
  | "Patch2"
  | "Patch3"
  | "Patch4"
  | "Patch5"
  | "Patch6"

type Patches = { selectedPatchFileVersion: PatchFileVersions }

type Action = {
  setSelectedPatchFileVersion: (
    selectedPatchFileVersion: Patches["selectedPatchFileVersion"]
  ) => void
}

export const useCustomizePatchInformationStore = create<Patches & Action>(
  (set) => ({
    selectedPatchFileVersion: "Base",
    setSelectedPatchFileVersion: (selectedPatchFileVersion) =>
      set(() => ({
        selectedPatchFileVersion: selectedPatchFileVersion,
      })),
  })
)
