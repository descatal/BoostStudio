export const UnitAssetFileOptions = {
  Unknown: "Unknown",
  Dummy: "Dummy",
  Animations: "Animations",
  ModelAndTexture: "ModelAndTexture",
  Data: "Data",
  Effects: "Effects",
  SoundEffects: "SoundEffects",
  InGamePilotVoiceLines: "InGamePilotVoiceLines",
  WeaponSprites: "WeaponSprites",
  InGameCutInSprites: "InGameCutInSprites",
  SpriteFrames: "SpriteFrames",
  VoiceLinesMetadata: "VoiceLinesMetadata",
  PilotVoiceLines: "PilotVoiceLines",
} as const
export type UnitAssetFileOptionsType =
  (typeof UnitAssetFileOptions)[keyof typeof UnitAssetFileOptions]

export const CommonAssetFileOptions = {
  Hitboxes: "Hitboxes",
  Projectiles: "Projectiles",
  Ammo: "Ammo",
  ListInfo: "ListInfo",
  UnitCostInfo: "UnitCostInfo",
  FigurineSprites: "FigurineSprites",
  MapSelectSprites: "MapSelectSprites",
  ArcadeSelectSmallSprites: "ArcadeSelectSmallSprites",
  ArcadeSelectUnitNameSprites: "ArcadeSelectUnitNameSprites",
  CameraConfigs: "CameraConfigs",
  CommonEffects: "CommonEffects",
  CommonEffectParticles: "CommonEffectParticles",
  CosmeticInfo: "CosmeticInfo",
  TextStrings: "TextStrings",
  SeriesLogoSprites: "SeriesLogoSprites",
  SeriesLogoSprites2: "SeriesLogoSprites2",
} as const
export type CommonAssetFileOptionsType =
  (typeof CommonAssetFileOptions)[keyof typeof CommonAssetFileOptions]

export const CombinedAssetFileOptions = {
  ...UnitAssetFileOptions,
  ...CommonAssetFileOptions,
}

export type AssetFileType =
  | UnitAssetFileOptionsType
  | CommonAssetFileOptionsType
