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
  SpriteFrames: "SpriteFrames",
  VoiceLinesMetadata: "VoiceLinesMetadata",
  PilotVoiceLines: "PilotVoiceLines",
  ArcadeSelectionCostume1Sprite: "ArcadeSelectionCostume1Sprite",
  ArcadeSelectionCostume2Sprite: "ArcadeSelectionCostume2Sprite",
  ArcadeSelectionCostume3Sprite: "ArcadeSelectionCostume3Sprite",
  LoadingLeftCostume1Sprite: "LoadingLeftCostume1Sprite",
  LoadingLeftCostume2Sprite: "LoadingLeftCostume2Sprite",
  LoadingLeftCostume3Sprite: "LoadingLeftCostume3Sprite",
  LoadingRightCostume1Sprite: "LoadingRightCostume1Sprite",
  LoadingRightCostume2Sprite: "LoadingRightCostume2Sprite",
  LoadingRightCostume3Sprite: "LoadingRightCostume3Sprite",
  GenericSelectionCostume1Sprite: "GenericSelectionCostume1Sprite",
  GenericSelectionCostume2Sprite: "GenericSelectionCostume2Sprite",
  GenericSelectionCostume3Sprite: "GenericSelectionCostume3Sprite",
  LoadingTargetUnitSprite: "LoadingTargetUnitSprite",
  LoadingTargetPilotCostume1Sprite: "LoadingTargetPilotCostume1Sprite",
  LoadingTargetPilotCostume2Sprite: "LoadingTargetPilotCostume2Sprite",
  LoadingTargetPilotCostume3Sprite: "LoadingTargetPilotCostume3Sprite",
  InGameSortieAndAwakeningPilotCostume1Sprite:
    "InGameSortieAndAwakeningPilotCostume1Sprite",
  InGameSortieAndAwakeningPilotCostume2Sprite:
    "InGameSortieAndAwakeningPilotCostume2Sprite",
  InGameSortieAndAwakeningPilotCostume3Sprite:
    "InGameSortieAndAwakeningPilotCostume3Sprite",
  ResultSmallUnitSprite: "ResultSmallUnitSprite",
  FigurineSprite: "FigurineSprite",
  LoadingTargetUnitSmallSprite: "LoadingTargetUnitSmallSprite",
  CatalogStorePilotCostume2Sprite: "CatalogStorePilotCostume2Sprite",
  CatalogStorePilotCostume3Sprite: "CatalogStorePilotCostume3Sprite",
} as const;
export type UnitAssetFileOptionsType =
  (typeof UnitAssetFileOptions)[keyof typeof UnitAssetFileOptions];

export const CommonAssetFileOptions = {
  Hitboxes: "Hitboxes",
  Projectiles: "Projectiles",
  Ammo: "Ammo",
  ListInfo: "ListInfo",
  UnitCostInfo: "UnitCostInfo",
  SharedFigurineSprites: "SharedFigurineSprites",
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
} as const;
export type CommonAssetFileOptionsType =
  (typeof CommonAssetFileOptions)[keyof typeof CommonAssetFileOptions];

export const SeriesFileOptions = {
  Movie: "Movie",
} as const;
export type SeriesFileOptionsType =
  (typeof SeriesFileOptions)[keyof typeof SeriesFileOptions];

export const CombinedAssetFileOptions = {
  ...UnitAssetFileOptions,
  ...SeriesFileOptions,
  ...CommonAssetFileOptions,
};

export type AssetFileType =
  | UnitAssetFileOptionsType
  | SeriesFileOptionsType
  | CommonAssetFileOptionsType;

export const UnitCustomizableSections = {
  Info: "info",
  Scripts: "scripts",
  Assets: "assets",
} as const;

export const UnitCustomizableInfoSections = {
  Stats: "stats",
  Ammo: "ammo",
  Projectiles: "projectiles",
  Hitboxes: "hitboxes",
} as const;
