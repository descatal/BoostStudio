export const PatchIdNameMap = {
  All: "All",
  Base: "Base Game",
  Patch1: "patch_01_00",
  Patch2: "patch_02_00",
  Patch3: "patch_03_00",
  Patch4: "patch_04_00",
  Patch5: "patch_05_00",
  Patch6: "patch_06_00",
};

export const PatchFileTabs = {
  All: "All",
  Base: "Base",
  Patch1: "Patch1",
  Patch2: "Patch2",
  Patch3: "Patch3",
  Patch4: "Patch4",
  Patch5: "Patch5",
  Patch6: "Patch6",
} as const;
export type PatchFileTabs = (typeof PatchFileTabs)[keyof typeof PatchFileTabs];
