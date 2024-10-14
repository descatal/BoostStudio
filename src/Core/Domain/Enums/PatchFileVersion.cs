namespace BoostStudio.Domain.Enums;

public enum PatchFileVersion
{
    Base = 0,
    Patch1 = 100,
    Patch2 = 200,
    Patch3 = 300,
    Patch4 = 400,
    Patch5 = 500,
    Patch6 = 600,
}

public static class PatchFileVersionExtensions
{
    public static string GetPatchName(this PatchFileVersion version) 
        => version switch
        {
            PatchFileVersion.Patch1 => "patch_01_00",
            PatchFileVersion.Patch2 => "patch_02_00",
            PatchFileVersion.Patch3 => "patch_03_00",
            PatchFileVersion.Patch4 => "patch_04_00",
            PatchFileVersion.Patch5 => "patch_05_00",
            PatchFileVersion.Patch6 => "patch_06_00",
            _ => string.Empty
        }; 
}

