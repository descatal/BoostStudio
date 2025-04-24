import { z } from "zod";

export type AmmoDto = z.infer<typeof AmmoDto>;
export const AmmoDto = z.object({
  hash: z.coerce.number().optional(),
  ammoType: z.coerce.number().optional(),
  maxAmmo: z.coerce.number().optional(),
  initialAmmo: z.coerce.number().optional(),
  timedDurationFrame: z.coerce.number().optional(),
  unk16: z.coerce.number().optional(),
  reloadType: z.coerce.number().optional(),
  cooldownDurationFrame: z.coerce.number().optional(),
  reloadDurationFrame: z.coerce.number().optional(),
  assaultBurstReloadDurationFrame: z.coerce.number().optional(),
  blastBurstReloadDurationFrame: z.coerce.number().optional(),
  unk40: z.coerce.number().optional(),
  unk44: z.coerce.number().optional(),
  inactiveUnk48: z.coerce.number().optional(),
  inactiveCooldownDurationFrame: z.coerce.number().optional(),
  inactiveReloadDurationFrame: z.coerce.number().optional(),
  inactiveAssaultBurstReloadDurationFrame: z.coerce.number().optional(),
  inactiveBlastBurstReloadDurationFrame: z.coerce.number().optional(),
  inactiveUnk68: z.coerce.number().optional(),
  inactiveUnk72: z.coerce.number().optional(),
  burstReplenish: z.coerce.number().optional(),
  unk80: z.coerce.number().optional(),
  unk84: z.coerce.number().optional(),
  unk88: z.coerce.number().optional(),
  chargeInput: z.coerce.number().optional(),
  chargeDurationFrame: z.coerce.number().optional(),
  assaultBurstChargeDurationFrame: z.coerce.number().optional(),
  blastBurstChargeDurationFrame: z.coerce.number().optional(),
  unk108: z.coerce.number().optional(),
  unk112: z.coerce.number().optional(),
  releaseChargeLingerDurationFrame: z.coerce.number().optional(),
  maxChargeLevel: z.coerce.number().optional(),
  unk124: z.coerce.number().optional(),
  unk128: z.coerce.number().optional(),
  order: z.coerce.number().optional(),
  unitId: z.union([z.coerce.number(), z.null()]).optional(),
});

export type AssetFileType = z.infer<typeof AssetFileType>;
export const AssetFileType = z.unknown();

export type UnitSummaryVm = z.infer<typeof UnitSummaryVm>;
export const UnitSummaryVm = z.object({
  unitId: z.coerce.number().optional(),
  slugName: z.union([z.string(), z.null()]).optional(),
  nameEnglish: z.union([z.string(), z.null()]).optional(),
  nameJapanese: z.union([z.string(), z.null()]).optional(),
  nameChinese: z.union([z.string(), z.null()]).optional(),
  seriesId: z.union([z.coerce.number(), z.null()]).optional(),
});

export type AssetFileDto = z.infer<typeof AssetFileDto>;
export const AssetFileDto = z.union([
  z.object({
    hash: z.coerce.number(),
    order: z.coerce.number(),
    fileType: z.array(AssetFileType),
    units: z.array(UnitSummaryVm),
  }),
  z.null(),
]);

export type AssetFileVm = z.infer<typeof AssetFileVm>;
export const AssetFileVm = z.object({
  hash: z.coerce.number(),
  order: z.union([z.coerce.number(), z.undefined()]).optional(),
  fileType: z.union([z.array(AssetFileType), z.undefined()]).optional(),
  gameUnitId: z.union([z.coerce.number(), z.null(), z.undefined()]).optional(),
});

export type BulkCreateUnitCommand = z.infer<typeof BulkCreateUnitCommand>;
export const BulkCreateUnitCommand = z.object({
  units: z.array(UnitSummaryVm),
});

export type CompileScexByPathCommand = z.infer<typeof CompileScexByPathCommand>;
export const CompileScexByPathCommand = z.object({
  sourcePath: z.string(),
  destinationPath: z.string(),
  fileName: z.union([z.string(), z.null(), z.undefined()]).optional(),
  hotReload: z.union([z.boolean(), z.undefined()]).optional(),
});

export type CompileScexByUnitsCommand = z.infer<
  typeof CompileScexByUnitsCommand
>;
export const CompileScexByUnitsCommand = z.object({
  unitIds: z.array(z.coerce.number()),
  replaceWorking: z.union([z.boolean(), z.undefined()]).optional(),
  hotReload: z.union([z.boolean(), z.undefined()]).optional(),
});

export type ConfigDto = z.infer<typeof ConfigDto>;
export const ConfigDto = z.object({
  key: z.string(),
  value: z.string(),
});

export type CreateAmmoCommand = z.infer<typeof CreateAmmoCommand>;
export const CreateAmmoCommand = z.object({
  ammoType: z.coerce.number().optional(),
  maxAmmo: z.coerce.number().optional(),
  initialAmmo: z.coerce.number().optional(),
  timedDurationFrame: z.coerce.number().optional(),
  unk16: z.coerce.number().optional(),
  reloadType: z.coerce.number().optional(),
  cooldownDurationFrame: z.coerce.number().optional(),
  reloadDurationFrame: z.coerce.number().optional(),
  assaultBurstReloadDurationFrame: z.coerce.number().optional(),
  blastBurstReloadDurationFrame: z.coerce.number().optional(),
  unk40: z.coerce.number().optional(),
  unk44: z.coerce.number().optional(),
  inactiveUnk48: z.coerce.number().optional(),
  inactiveCooldownDurationFrame: z.coerce.number().optional(),
  inactiveReloadDurationFrame: z.coerce.number().optional(),
  inactiveAssaultBurstReloadDurationFrame: z.coerce.number().optional(),
  inactiveBlastBurstReloadDurationFrame: z.coerce.number().optional(),
  inactiveUnk68: z.coerce.number().optional(),
  inactiveUnk72: z.coerce.number().optional(),
  burstReplenish: z.coerce.number().optional(),
  unk80: z.coerce.number().optional(),
  unk84: z.coerce.number().optional(),
  unk88: z.coerce.number().optional(),
  chargeInput: z.coerce.number().optional(),
  chargeDurationFrame: z.coerce.number().optional(),
  assaultBurstChargeDurationFrame: z.coerce.number().optional(),
  blastBurstChargeDurationFrame: z.coerce.number().optional(),
  unk108: z.coerce.number().optional(),
  unk112: z.coerce.number().optional(),
  releaseChargeLingerDurationFrame: z.coerce.number().optional(),
  maxChargeLevel: z.coerce.number().optional(),
  unk124: z.coerce.number().optional(),
  unk128: z.coerce.number().optional(),
  order: z.coerce.number().optional(),
  unitId: z.union([z.coerce.number(), z.null()]).optional(),
});

export type CreateAssetFileCommand = z.infer<typeof CreateAssetFileCommand>;
export const CreateAssetFileCommand = z.object({
  order: z.coerce.number().optional(),
  fileType: z.array(AssetFileType).optional(),
  gameUnitId: z.union([z.coerce.number(), z.null()]).optional(),
});

export type CreateHitboxCommand = z.infer<typeof CreateHitboxCommand>;
export const CreateHitboxCommand = z.object({
  hitboxType: z.coerce.number().optional(),
  damage: z.coerce.number().optional(),
  unk8: z.coerce.number().optional(),
  downValueThreshold: z.coerce.number().optional(),
  yorukeValueThreshold: z.coerce.number().optional(),
  unk20: z.coerce.number().optional(),
  unk24: z.coerce.number().optional(),
  damageCorrection: z.coerce.number().optional(),
  specialEffect: z.coerce.number().optional(),
  hitEffect: z.coerce.number().optional(),
  flyDirection1: z.coerce.number().optional(),
  flyDirection2: z.coerce.number().optional(),
  flyDirection3: z.coerce.number().optional(),
  enemyCameraShakeMultiplier: z.coerce.number().optional(),
  playerCameraShakeMultiplier: z.coerce.number().optional(),
  unk56: z.coerce.number().optional(),
  knockUpAngle: z.coerce.number().optional(),
  knockUpRange: z.coerce.number().optional(),
  unk68: z.coerce.number().optional(),
  multipleHitIntervalFrame: z.coerce.number().optional(),
  multipleHitCount: z.coerce.number().optional(),
  enemyStunDuration: z.coerce.number().optional(),
  playerStunDuration: z.coerce.number().optional(),
  hitVisualEffect: z.coerce.number().optional(),
  hitVisualEffectSizeMultiplier: z.coerce.number().optional(),
  hitSoundEffectHash: z.coerce.number().optional(),
  unk100: z.coerce.number().optional(),
  friendlyDamageFlag: z.coerce.number().optional(),
  unk108: z.coerce.number().optional(),
  hitboxGroupHash: z.coerce.number().optional(),
});

export type CreateHitboxGroupCommand = z.infer<typeof CreateHitboxGroupCommand>;
export const CreateHitboxGroupCommand = z.object({
  hash: z.coerce.number(),
  unitIds: z
    .union([z.array(z.coerce.number()), z.null(), z.undefined()])
    .optional(),
});

export type PatchFileVersion = z.infer<typeof PatchFileVersion>;
export const PatchFileVersion = z.unknown();

export type PathInfoDto = z.infer<typeof PathInfoDto>;
export const PathInfoDto = z.union([
  z.object({
    path: z.string(),
    order: z.union([z.coerce.number(), z.null(), z.undefined()]).optional(),
  }),
  z.null(),
]);

export type FileInfoDto = z.infer<typeof FileInfoDto>;
export const FileInfoDto = z.union([
  z.object({
    version: PatchFileVersion,
    size1: z.coerce.number(),
    size2: z.coerce.number(),
    size3: z.coerce.number(),
    size4: z.coerce.number(),
  }),
  z.null(),
]);

export type CreatePatchFileCommand = z.infer<typeof CreatePatchFileCommand>;
export const CreatePatchFileCommand = z.object({
  tblId: PatchFileVersion,
  pathInfo: z.union([PathInfoDto, z.undefined()]).optional(),
  fileInfo: z.union([FileInfoDto, z.undefined()]).optional(),
  assetFileHash: z
    .union([z.coerce.number(), z.null(), z.undefined()])
    .optional(),
});

export type CreateProjectileCommand = z.infer<typeof CreateProjectileCommand>;
export const CreateProjectileCommand = z.object({
  projectileType: z.coerce.number().optional(),
  hitboxHash: z.union([z.coerce.number(), z.null()]).optional(),
  modelHash: z.coerce.number().optional(),
  skeletonIndex: z.coerce.number().optional(),
  aimType: z.coerce.number().optional(),
  translateY: z.coerce.number().optional(),
  translateZ: z.coerce.number().optional(),
  translateX: z.coerce.number().optional(),
  rotateX: z.coerce.number().optional(),
  rotateZ: z.coerce.number().optional(),
  cosmeticHash: z.coerce.number().optional(),
  unk44: z.coerce.number().optional(),
  unk48: z.coerce.number().optional(),
  unk52: z.coerce.number().optional(),
  unk56: z.coerce.number().optional(),
  ammoConsumption: z.coerce.number().optional(),
  durationFrame: z.coerce.number().optional(),
  maxTravelDistance: z.coerce.number().optional(),
  initialSpeed: z.coerce.number().optional(),
  acceleration: z.coerce.number().optional(),
  accelerationStartFrame: z.coerce.number().optional(),
  unk84: z.coerce.number().optional(),
  maxSpeed: z.coerce.number().optional(),
  reserved92: z.coerce.number().optional(),
  reserved96: z.coerce.number().optional(),
  reserved100: z.coerce.number().optional(),
  reserved104: z.coerce.number().optional(),
  reserved108: z.coerce.number().optional(),
  reserved112: z.coerce.number().optional(),
  reserved116: z.coerce.number().optional(),
  horizontalGuidance: z.coerce.number().optional(),
  horizontalGuidanceAngle: z.coerce.number().optional(),
  verticalGuidance: z.coerce.number().optional(),
  verticalGuidanceAngle: z.coerce.number().optional(),
  reserved136: z.coerce.number().optional(),
  reserved140: z.coerce.number().optional(),
  reserved144: z.coerce.number().optional(),
  reserved148: z.coerce.number().optional(),
  reserved152: z.coerce.number().optional(),
  reserved156: z.coerce.number().optional(),
  reserved160: z.coerce.number().optional(),
  reserved164: z.coerce.number().optional(),
  reserved168: z.coerce.number().optional(),
  reserved172: z.coerce.number().optional(),
  size: z.coerce.number().optional(),
  reserved180: z.coerce.number().optional(),
  reserved184: z.coerce.number().optional(),
  soundEffectHash: z.coerce.number().optional(),
  reserved192: z.coerce.number().optional(),
  reserved196: z.coerce.number().optional(),
  chainedProjectileHash: z.coerce.number().optional(),
  reserved204: z.coerce.number().optional(),
  reserved208: z.coerce.number().optional(),
  reserved212: z.coerce.number().optional(),
  reserved216: z.coerce.number().optional(),
  reserved220: z.coerce.number().optional(),
  reserved224: z.coerce.number().optional(),
  reserved228: z.coerce.number().optional(),
  reserved232: z.coerce.number().optional(),
  reserved236: z.coerce.number().optional(),
  reserved240: z.coerce.number().optional(),
  reserved244: z.coerce.number().optional(),
  reserved248: z.coerce.number().optional(),
  reserved252: z.coerce.number().optional(),
  reserved256: z.coerce.number().optional(),
  reserved260: z.coerce.number().optional(),
  reserved264: z.coerce.number().optional(),
  reserved268: z.coerce.number().optional(),
  reserved272: z.coerce.number().optional(),
  reserved276: z.coerce.number().optional(),
  unitId: z.union([z.coerce.number(), z.null()]).optional(),
});

export type PlayableSeriesDetailsDto = z.infer<typeof PlayableSeriesDetailsDto>;
export const PlayableSeriesDetailsDto = z.union([
  z.object({
    unk2: z.coerce.number().optional(),
    unk3: z.coerce.number().optional(),
    unk4: z.coerce.number().optional(),
    selectOrder: z.coerce.number().optional(),
    logoSpriteIndex: z.coerce.number().optional(),
    logoSprite2Index: z.coerce.number().optional(),
    unk11: z.coerce.number().optional(),
    movieAssetHash: z.union([z.coerce.number(), z.null()]).optional(),
  }),
  z.null(),
]);

export type CreateSeriesCommand = z.infer<typeof CreateSeriesCommand>;
export const CreateSeriesCommand = z.object({
  id: z.coerce.number().optional(),
  playableSeries: PlayableSeriesDetailsDto.optional(),
  slugName: z.string().optional(),
  nameEnglish: z.union([z.string(), z.null()]).optional(),
  nameJapanese: z.union([z.string(), z.null()]).optional(),
  nameChinese: z.union([z.string(), z.null()]).optional(),
});

export type CreateStatCommand = z.infer<typeof CreateStatCommand>;
export const CreateStatCommand = z.object({
  unitCost: z.coerce.number().optional(),
  unitCost2: z.coerce.number().optional(),
  maxHp: z.coerce.number().optional(),
  downValueThreshold: z.coerce.number().optional(),
  yorukeValueThreshold: z.coerce.number().optional(),
  unk20: z.coerce.number().optional(),
  unk24: z.coerce.number().optional(),
  unk28: z.coerce.number().optional(),
  maxBoost: z.coerce.number().optional(),
  unk36: z.coerce.number().optional(),
  unk40: z.coerce.number().optional(),
  unk44: z.coerce.number().optional(),
  gravityMultiplierAir: z.coerce.number().optional(),
  gravityMultiplierLand: z.coerce.number().optional(),
  unk56: z.coerce.number().optional(),
  unk60: z.coerce.number().optional(),
  unk64: z.coerce.number().optional(),
  unk68: z.coerce.number().optional(),
  unk72: z.coerce.number().optional(),
  unk76: z.coerce.number().optional(),
  unk80: z.coerce.number().optional(),
  cameraZoomMultiplier: z.coerce.number().optional(),
  unk88: z.coerce.number().optional(),
  unk92: z.coerce.number().optional(),
  unk96: z.coerce.number().optional(),
  unk100: z.coerce.number().optional(),
  unk104: z.coerce.number().optional(),
  unk108: z.coerce.number().optional(),
  sizeMultiplier: z.coerce.number().optional(),
  unk116: z.coerce.number().optional(),
  unk120: z.coerce.number().optional(),
  unk124: z.coerce.number().optional(),
  unk128: z.coerce.number().optional(),
  unk132: z.coerce.number().optional(),
  unk136: z.coerce.number().optional(),
  unk140: z.coerce.number().optional(),
  unk144: z.coerce.number().optional(),
  unk148: z.coerce.number().optional(),
  unk152: z.coerce.number().optional(),
  unk156: z.coerce.number().optional(),
  unk160: z.coerce.number().optional(),
  unk164: z.coerce.number().optional(),
  unk168: z.coerce.number().optional(),
  unk172: z.coerce.number().optional(),
  unk176: z.coerce.number().optional(),
  unk180: z.coerce.number().optional(),
  unk184: z.coerce.number().optional(),
  redLockRangeMelee: z.coerce.number().optional(),
  redLockRange: z.coerce.number().optional(),
  unk196: z.coerce.number().optional(),
  unk200: z.coerce.number().optional(),
  unk204: z.coerce.number().optional(),
  unk208: z.coerce.number().optional(),
  boostReplenish: z.coerce.number().optional(),
  unk216: z.coerce.number().optional(),
  boostInitialConsumption: z.coerce.number().optional(),
  boostFuwaInitialConsumption: z.coerce.number().optional(),
  boostFlyConsumption: z.coerce.number().optional(),
  boostGroundStepInitialConsumption: z.coerce.number().optional(),
  boostGroundStepConsumption: z.coerce.number().optional(),
  boostAirStepInitialConsumption: z.coerce.number().optional(),
  boostAirStepConsumption: z.coerce.number().optional(),
  boostBdInitialConsumption: z.coerce.number().optional(),
  boostBdConsumption: z.coerce.number().optional(),
  unk256: z.coerce.number().optional(),
  unk260: z.coerce.number().optional(),
  unk264: z.coerce.number().optional(),
  unk268: z.coerce.number().optional(),
  boostTransformInitialConsumption: z.coerce.number().optional(),
  boostTransformConsumption: z.coerce.number().optional(),
  boostNonVernierActionConsumption: z.coerce.number().optional(),
  boostPostActionConsumption: z.coerce.number().optional(),
  boostRainbowStepInitialConsumption: z.coerce.number().optional(),
  unk292: z.coerce.number().optional(),
  unk296: z.coerce.number().optional(),
  unk300: z.coerce.number().optional(),
  unk304: z.coerce.number().optional(),
  unk308: z.coerce.number().optional(),
  unk312: z.coerce.number().optional(),
  unk316: z.coerce.number().optional(),
  unk320: z.coerce.number().optional(),
  unk324: z.coerce.number().optional(),
  unk328: z.coerce.number().optional(),
  unk332: z.coerce.number().optional(),
  assaultBurstRedLockMelee: z.coerce.number().optional(),
  assaultBurstRedLock: z.coerce.number().optional(),
  assaultBurstDamageDealtMultiplier: z.coerce.number().optional(),
  assaultBurstDamageTakenMultiplier: z.coerce.number().optional(),
  assaultBurstMobilityMultiplier: z.coerce.number().optional(),
  assaultBurstDownValueDealtMultiplier: z.coerce.number().optional(),
  assaultBurstBoostConsumptionMultiplier: z.coerce.number().optional(),
  unk364: z.coerce.number().optional(),
  unk368: z.coerce.number().optional(),
  assaultBurstDamageDealtBurstGaugeIncreaseMultiplier: z.coerce
    .number()
    .optional(),
  assaultBurstDamageTakenBurstGaugeIncreaseMultiplier: z.coerce
    .number()
    .optional(),
  unk380: z.coerce.number().optional(),
  unk384: z.coerce.number().optional(),
  unk388: z.coerce.number().optional(),
  unk392: z.coerce.number().optional(),
  unk396: z.coerce.number().optional(),
  blastBurstRedLockMelee: z.coerce.number().optional(),
  blastBurstRedLock: z.coerce.number().optional(),
  blastBurstDamageDealtMultiplier: z.coerce.number().optional(),
  blastBurstDamageTakenMultiplier: z.coerce.number().optional(),
  blastBurstMobilityMultiplier: z.coerce.number().optional(),
  blastBurstDownValueDealtMultiplier: z.coerce.number().optional(),
  blastBurstBoostConsumptionMultiplier: z.coerce.number().optional(),
  unk428: z.coerce.number().optional(),
  unk432: z.coerce.number().optional(),
  blastBurstDamageDealtBurstGaugeIncreaseMultiplier: z.coerce
    .number()
    .optional(),
  blastBurstDamageTakenBurstGaugeIncreaseMultiplier: z.coerce
    .number()
    .optional(),
  unk444: z.coerce.number().optional(),
  unk448: z.coerce.number().optional(),
  unk452: z.coerce.number().optional(),
  unk456: z.coerce.number().optional(),
  unk460: z.coerce.number().optional(),
  thirdBurstRedLockMelee: z.coerce.number().optional(),
  thirdBurstRedLock: z.coerce.number().optional(),
  thirdBurstDamageDealtMultiplier: z.coerce.number().optional(),
  thirdBurstDamageTakenMultiplier: z.coerce.number().optional(),
  thirdBurstMobilityMultiplier: z.coerce.number().optional(),
  thirdBurstDownValueDealtMultiplier: z.coerce.number().optional(),
  thirdBurstBoostConsumptionMultiplier: z.coerce.number().optional(),
  unk492: z.coerce.number().optional(),
  unk496: z.coerce.number().optional(),
  thirdBurstDamageDealtBurstGaugeIncreaseMultiplier: z.coerce
    .number()
    .optional(),
  thirdBurstDamageTakenBurstGaugeIncreaseMultiplier: z.coerce
    .number()
    .optional(),
  unk508: z.coerce.number().optional(),
  unk512: z.coerce.number().optional(),
  unk516: z.coerce.number().optional(),
  unk520: z.coerce.number().optional(),
  unk524: z.coerce.number().optional(),
  fourthBurstRedLockMelee: z.coerce.number().optional(),
  fourthBurstRedLock: z.coerce.number().optional(),
  fourthBurstDamageDealtMultiplier: z.coerce.number().optional(),
  fourthBurstDamageTakenMultiplier: z.coerce.number().optional(),
  fourthBurstMobilityMultiplier: z.coerce.number().optional(),
  fourthBurstDownValueDealtMultiplier: z.coerce.number().optional(),
  fourthBurstBoostConsumptionMultiplier: z.coerce.number().optional(),
  unk572: z.coerce.number().optional(),
  unk576: z.coerce.number().optional(),
  fourthBurstDamageDealtBurstGaugeIncreaseMultiplier: z.coerce
    .number()
    .optional(),
  fourthBurstDamageTakenBurstGaugeIncreaseMultiplier: z.coerce
    .number()
    .optional(),
  unk588: z.coerce.number().optional(),
  unk592: z.coerce.number().optional(),
  unk596: z.coerce.number().optional(),
  unk600: z.coerce.number().optional(),
  unk604: z.coerce.number().optional(),
  unk608: z.coerce.number().optional(),
  order: z.coerce.number().optional(),
  unitId: z.union([z.coerce.number(), z.null()]).optional(),
});

export type CreateUnitAmmoSlotCommand = z.infer<
  typeof CreateUnitAmmoSlotCommand
>;
export const CreateUnitAmmoSlotCommand = z.object({
  ammoHash: z.coerce.number(),
  unitId: z.coerce.number(),
  slotOrder: z.coerce.number(),
});

export type CreateUnitCommand = z.infer<typeof CreateUnitCommand>;
export const CreateUnitCommand = z.object({
  unitId: z.coerce.number().optional(),
  slugName: z.union([z.string(), z.null()]).optional(),
  nameEnglish: z.union([z.string(), z.null()]).optional(),
  nameJapanese: z.union([z.string(), z.null()]).optional(),
  nameChinese: z.union([z.string(), z.null()]).optional(),
  seriesId: z.union([z.coerce.number(), z.null()]).optional(),
});

export type DebugCommand = z.infer<typeof DebugCommand>;
export const DebugCommand = z.unknown();

export type DecompileScexByPathCommand = z.infer<
  typeof DecompileScexByPathCommand
>;
export const DecompileScexByPathCommand = z.object({
  sourcePath: z.string(),
  destinationPath: z.string(),
  fileName: z.union([z.string(), z.null(), z.undefined()]).optional(),
});

export type DecompileScexByUnitsCommand = z.infer<
  typeof DecompileScexByUnitsCommand
>;
export const DecompileScexByUnitsCommand = z.object({
  unitIds: z.array(z.coerce.number()),
  replaceScript: z.union([z.boolean(), z.undefined()]).optional(),
});

export type ExportAmmoByPathCommand = z.infer<typeof ExportAmmoByPathCommand>;
export const ExportAmmoByPathCommand = z.object({
  exportPath: z.union([z.string(), z.null()]).optional(),
});

export type ExportAmmoCommand = z.infer<typeof ExportAmmoCommand>;
export const ExportAmmoCommand = z.object({
  replaceWorking: z.boolean().optional(),
  hotReload: z.boolean().optional(),
});

export type ExportHitboxGroupByPathCommand = z.infer<
  typeof ExportHitboxGroupByPathCommand
>;
export const ExportHitboxGroupByPathCommand = z.object({
  hashes: z.union([z.array(z.coerce.number()), z.null()]).optional(),
  unitIds: z.union([z.array(z.coerce.number()), z.null()]).optional(),
  outputPath: z.union([z.string(), z.null()]).optional(),
});

export type ExportHitboxGroupCommand = z.infer<typeof ExportHitboxGroupCommand>;
export const ExportHitboxGroupCommand = z.object({
  hashes: z.union([z.array(z.coerce.number()), z.null()]).optional(),
  unitIds: z.union([z.array(z.coerce.number()), z.null()]).optional(),
  replaceWorking: z.boolean().optional(),
  hotReload: z.boolean().optional(),
});

export type ExportPlayableCharactersCommand = z.infer<
  typeof ExportPlayableCharactersCommand
>;
export const ExportPlayableCharactersCommand = z.object({
  replaceWorking: z.boolean().optional(),
});

export type ExportPlayableSeriesCommand = z.infer<
  typeof ExportPlayableSeriesCommand
>;
export const ExportPlayableSeriesCommand = z.object({
  replaceWorking: z.boolean().optional(),
});

export type ExportTblCommand = z.infer<typeof ExportTblCommand>;
export const ExportTblCommand = z.object({
  versions: z.union([z.array(PatchFileVersion), z.null()]).optional(),
  replaceStaging: z.boolean().optional(),
});

export type ExportUnitProjectileByPathCommand = z.infer<
  typeof ExportUnitProjectileByPathCommand
>;
export const ExportUnitProjectileByPathCommand = z.object({
  unitIds: z.union([z.array(z.coerce.number()), z.null()]).optional(),
  exportPath: z.union([z.string(), z.null()]).optional(),
});

export type ExportUnitProjectileCommand = z.infer<
  typeof ExportUnitProjectileCommand
>;
export const ExportUnitProjectileCommand = z.object({
  unitIds: z.union([z.array(z.coerce.number()), z.null()]).optional(),
  replaceWorking: z.boolean().optional(),
  hotReload: z.boolean().optional(),
});

export type ExportUnitStatByPathCommand = z.infer<
  typeof ExportUnitStatByPathCommand
>;
export const ExportUnitStatByPathCommand = z.object({
  unitIds: z.union([z.array(z.coerce.number()), z.null()]).optional(),
  exportPath: z.union([z.string(), z.null()]).optional(),
});

export type ExportUnitStatCommand = z.infer<typeof ExportUnitStatCommand>;
export const ExportUnitStatCommand = z.object({
  unitIds: z.union([z.array(z.coerce.number()), z.null()]).optional(),
  replaceWorking: z.boolean().optional(),
});

export type HitboxDto = z.infer<typeof HitboxDto>;
export const HitboxDto = z.object({
  hash: z.coerce.number().optional(),
  hitboxType: z.coerce.number().optional(),
  damage: z.coerce.number().optional(),
  unk8: z.coerce.number().optional(),
  downValueThreshold: z.coerce.number().optional(),
  yorukeValueThreshold: z.coerce.number().optional(),
  unk20: z.coerce.number().optional(),
  unk24: z.coerce.number().optional(),
  damageCorrection: z.coerce.number().optional(),
  specialEffect: z.coerce.number().optional(),
  hitEffect: z.coerce.number().optional(),
  flyDirection1: z.coerce.number().optional(),
  flyDirection2: z.coerce.number().optional(),
  flyDirection3: z.coerce.number().optional(),
  enemyCameraShakeMultiplier: z.coerce.number().optional(),
  playerCameraShakeMultiplier: z.coerce.number().optional(),
  unk56: z.coerce.number().optional(),
  knockUpAngle: z.coerce.number().optional(),
  knockUpRange: z.coerce.number().optional(),
  unk68: z.coerce.number().optional(),
  multipleHitIntervalFrame: z.coerce.number().optional(),
  multipleHitCount: z.coerce.number().optional(),
  enemyStunDuration: z.coerce.number().optional(),
  playerStunDuration: z.coerce.number().optional(),
  hitVisualEffect: z.coerce.number().optional(),
  hitVisualEffectSizeMultiplier: z.coerce.number().optional(),
  hitSoundEffectHash: z.coerce.number().optional(),
  unk100: z.coerce.number().optional(),
  friendlyDamageFlag: z.coerce.number().optional(),
  unk108: z.coerce.number().optional(),
  hitboxGroupHash: z.coerce.number().optional(),
});

export type HitboxGroupDto = z.infer<typeof HitboxGroupDto>;
export const HitboxGroupDto = z.object({
  hash: z.coerce.number().optional(),
  unitIds: z.array(z.coerce.number()).optional(),
  hitboxes: z.array(HitboxDto).optional(),
});

export type HotReloadScex = z.infer<typeof HotReloadScex>;
export const HotReloadScex = z.object({
  sourcePath: z.string(),
});

export type IFormFile = z.infer<typeof IFormFile>;
export const IFormFile = z.string();

export type IFormFileCollection = z.infer<typeof IFormFileCollection>;
export const IFormFileCollection = z.array(IFormFile);

export type NullableOfCompressionType = z.infer<
  typeof NullableOfCompressionType
>;
export const NullableOfCompressionType = z.union([z.unknown(), z.null()]);

export type PackFhmAssetCommand = z.infer<typeof PackFhmAssetCommand>;
export const PackFhmAssetCommand = z.object({
  assetFileHashes: z.union([z.array(z.coerce.number()), z.null()]).optional(),
  assetFileTypes: z.union([z.array(AssetFileType), z.null()]).optional(),
  unitIds: z.union([z.array(z.coerce.number()), z.null()]).optional(),
  patchFileVersions: z.union([z.array(PatchFileVersion), z.null()]).optional(),
  replaceStaging: z.boolean().optional(),
});

export type PackPsarcByPatchFilesCommand = z.infer<
  typeof PackPsarcByPatchFilesCommand
>;
export const PackPsarcByPatchFilesCommand = z.object({
  patchFileVersions: z.union([z.array(PatchFileVersion), z.null()]).optional(),
});

export type PackPsarcByPathCommand = z.infer<typeof PackPsarcByPathCommand>;
export const PackPsarcByPathCommand = z.object({
  sourcePath: z.string(),
  destinationPath: z.string(),
  filename: z.union([z.string(), z.null(), z.undefined()]).optional(),
  compressionType: z
    .union([NullableOfCompressionType, z.undefined()])
    .optional(),
  compressionLevel: z
    .union([z.coerce.number(), z.null(), z.undefined()])
    .optional(),
});

export type PaginatedListOfAmmoDto = z.infer<typeof PaginatedListOfAmmoDto>;
export const PaginatedListOfAmmoDto = z.object({
  items: z.array(AmmoDto),
  pageNumber: z.coerce.number(),
  totalPages: z.union([z.coerce.number(), z.undefined()]).optional(),
  totalCount: z.coerce.number(),
  hasPreviousPage: z.union([z.boolean(), z.undefined()]).optional(),
  hasNextPage: z.union([z.boolean(), z.undefined()]).optional(),
});

export type PaginatedListOfAssetFileVm = z.infer<
  typeof PaginatedListOfAssetFileVm
>;
export const PaginatedListOfAssetFileVm = z.object({
  items: z.array(AssetFileVm),
  pageNumber: z.coerce.number(),
  totalPages: z.union([z.coerce.number(), z.undefined()]).optional(),
  totalCount: z.coerce.number(),
  hasPreviousPage: z.union([z.boolean(), z.undefined()]).optional(),
  hasNextPage: z.union([z.boolean(), z.undefined()]).optional(),
});

export type PaginatedListOfHitboxDto = z.infer<typeof PaginatedListOfHitboxDto>;
export const PaginatedListOfHitboxDto = z.object({
  items: z.array(HitboxDto),
  pageNumber: z.coerce.number(),
  totalPages: z.union([z.coerce.number(), z.undefined()]).optional(),
  totalCount: z.coerce.number(),
  hasPreviousPage: z.union([z.boolean(), z.undefined()]).optional(),
  hasNextPage: z.union([z.boolean(), z.undefined()]).optional(),
});

export type PaginatedListOfHitboxGroupDto = z.infer<
  typeof PaginatedListOfHitboxGroupDto
>;
export const PaginatedListOfHitboxGroupDto = z.object({
  items: z.array(HitboxGroupDto),
  pageNumber: z.coerce.number(),
  totalPages: z.union([z.coerce.number(), z.undefined()]).optional(),
  totalCount: z.coerce.number(),
  hasPreviousPage: z.union([z.boolean(), z.undefined()]).optional(),
  hasNextPage: z.union([z.boolean(), z.undefined()]).optional(),
});

export type PatchFileSummaryVm = z.infer<typeof PatchFileSummaryVm>;
export const PatchFileSummaryVm = z.object({
  assetFile: z.union([AssetFileDto, z.undefined()]).optional(),
  id: z.union([z.string(), z.null()]),
  tblId: PatchFileVersion,
  pathInfo: z.union([PathInfoDto, z.undefined()]).optional(),
  fileInfo: z.union([FileInfoDto, z.undefined()]).optional(),
  assetFileHash: z
    .union([z.coerce.number(), z.null(), z.undefined()])
    .optional(),
});

export type PaginatedListOfPatchFileSummaryVm = z.infer<
  typeof PaginatedListOfPatchFileSummaryVm
>;
export const PaginatedListOfPatchFileSummaryVm = z.object({
  items: z.array(PatchFileSummaryVm),
  pageNumber: z.coerce.number(),
  totalPages: z.union([z.coerce.number(), z.undefined()]).optional(),
  totalCount: z.coerce.number(),
  hasPreviousPage: z.union([z.boolean(), z.undefined()]).optional(),
  hasNextPage: z.union([z.boolean(), z.undefined()]).optional(),
});

export type PatchFileVm = z.infer<typeof PatchFileVm>;
export const PatchFileVm = z.object({
  id: z.union([z.string(), z.null()]),
  tblId: PatchFileVersion,
  pathInfo: z.union([PathInfoDto, z.undefined()]).optional(),
  fileInfo: z.union([FileInfoDto, z.undefined()]).optional(),
  assetFileHash: z
    .union([z.coerce.number(), z.null(), z.undefined()])
    .optional(),
});

export type PaginatedListOfPatchFileVm = z.infer<
  typeof PaginatedListOfPatchFileVm
>;
export const PaginatedListOfPatchFileVm = z.object({
  items: z.array(PatchFileVm),
  pageNumber: z.coerce.number(),
  totalPages: z.union([z.coerce.number(), z.undefined()]).optional(),
  totalCount: z.coerce.number(),
  hasPreviousPage: z.union([z.boolean(), z.undefined()]).optional(),
  hasNextPage: z.union([z.boolean(), z.undefined()]).optional(),
});

export type ProjectileDto = z.infer<typeof ProjectileDto>;
export const ProjectileDto = z.object({
  hash: z.coerce.number().optional(),
  projectileType: z.coerce.number().optional(),
  hitboxHash: z.union([z.coerce.number(), z.null()]).optional(),
  modelHash: z.coerce.number().optional(),
  skeletonIndex: z.coerce.number().optional(),
  aimType: z.coerce.number().optional(),
  translateY: z.coerce.number().optional(),
  translateZ: z.coerce.number().optional(),
  translateX: z.coerce.number().optional(),
  rotateX: z.coerce.number().optional(),
  rotateZ: z.coerce.number().optional(),
  cosmeticHash: z.coerce.number().optional(),
  unk44: z.coerce.number().optional(),
  unk48: z.coerce.number().optional(),
  unk52: z.coerce.number().optional(),
  unk56: z.coerce.number().optional(),
  ammoConsumption: z.coerce.number().optional(),
  durationFrame: z.coerce.number().optional(),
  maxTravelDistance: z.coerce.number().optional(),
  initialSpeed: z.coerce.number().optional(),
  acceleration: z.coerce.number().optional(),
  accelerationStartFrame: z.coerce.number().optional(),
  unk84: z.coerce.number().optional(),
  maxSpeed: z.coerce.number().optional(),
  reserved92: z.coerce.number().optional(),
  reserved96: z.coerce.number().optional(),
  reserved100: z.coerce.number().optional(),
  reserved104: z.coerce.number().optional(),
  reserved108: z.coerce.number().optional(),
  reserved112: z.coerce.number().optional(),
  reserved116: z.coerce.number().optional(),
  horizontalGuidance: z.coerce.number().optional(),
  horizontalGuidanceAngle: z.coerce.number().optional(),
  verticalGuidance: z.coerce.number().optional(),
  verticalGuidanceAngle: z.coerce.number().optional(),
  reserved136: z.coerce.number().optional(),
  reserved140: z.coerce.number().optional(),
  reserved144: z.coerce.number().optional(),
  reserved148: z.coerce.number().optional(),
  reserved152: z.coerce.number().optional(),
  reserved156: z.coerce.number().optional(),
  reserved160: z.coerce.number().optional(),
  reserved164: z.coerce.number().optional(),
  reserved168: z.coerce.number().optional(),
  reserved172: z.coerce.number().optional(),
  size: z.coerce.number().optional(),
  reserved180: z.coerce.number().optional(),
  reserved184: z.coerce.number().optional(),
  soundEffectHash: z.coerce.number().optional(),
  reserved192: z.coerce.number().optional(),
  reserved196: z.coerce.number().optional(),
  chainedProjectileHash: z.coerce.number().optional(),
  reserved204: z.coerce.number().optional(),
  reserved208: z.coerce.number().optional(),
  reserved212: z.coerce.number().optional(),
  reserved216: z.coerce.number().optional(),
  reserved220: z.coerce.number().optional(),
  reserved224: z.coerce.number().optional(),
  reserved228: z.coerce.number().optional(),
  reserved232: z.coerce.number().optional(),
  reserved236: z.coerce.number().optional(),
  reserved240: z.coerce.number().optional(),
  reserved244: z.coerce.number().optional(),
  reserved248: z.coerce.number().optional(),
  reserved252: z.coerce.number().optional(),
  reserved256: z.coerce.number().optional(),
  reserved260: z.coerce.number().optional(),
  reserved264: z.coerce.number().optional(),
  reserved268: z.coerce.number().optional(),
  reserved272: z.coerce.number().optional(),
  reserved276: z.coerce.number().optional(),
  unitId: z.union([z.coerce.number(), z.null()]).optional(),
});

export type PaginatedListOfProjectileDto = z.infer<
  typeof PaginatedListOfProjectileDto
>;
export const PaginatedListOfProjectileDto = z.object({
  items: z.array(ProjectileDto),
  pageNumber: z.coerce.number(),
  totalPages: z.union([z.coerce.number(), z.undefined()]).optional(),
  totalCount: z.coerce.number(),
  hasPreviousPage: z.union([z.boolean(), z.undefined()]).optional(),
  hasNextPage: z.union([z.boolean(), z.undefined()]).optional(),
});

export type SeriesDto = z.infer<typeof SeriesDto>;
export const SeriesDto = z.object({
  id: z.coerce.number().optional(),
  playableSeries: PlayableSeriesDetailsDto.optional(),
  slugName: z.string().optional(),
  nameEnglish: z.union([z.string(), z.null()]).optional(),
  nameJapanese: z.union([z.string(), z.null()]).optional(),
  nameChinese: z.union([z.string(), z.null()]).optional(),
});

export type PaginatedListOfSeriesDto = z.infer<typeof PaginatedListOfSeriesDto>;
export const PaginatedListOfSeriesDto = z.object({
  items: z.array(SeriesDto),
  pageNumber: z.coerce.number(),
  totalPages: z.union([z.coerce.number(), z.undefined()]).optional(),
  totalCount: z.coerce.number(),
  hasPreviousPage: z.union([z.boolean(), z.undefined()]).optional(),
  hasNextPage: z.union([z.boolean(), z.undefined()]).optional(),
});

export type SeriesUnitsVm = z.infer<typeof SeriesUnitsVm>;
export const SeriesUnitsVm = z.object({
  units: z.array(UnitSummaryVm).optional(),
  id: z.coerce.number().optional(),
  slugName: z.string().optional(),
  nameEnglish: z.union([z.string(), z.null()]).optional(),
  nameJapanese: z.union([z.string(), z.null()]).optional(),
  nameChinese: z.union([z.string(), z.null()]).optional(),
});

export type PaginatedListOfSeriesUnitsVm = z.infer<
  typeof PaginatedListOfSeriesUnitsVm
>;
export const PaginatedListOfSeriesUnitsVm = z.object({
  items: z.array(SeriesUnitsVm),
  pageNumber: z.coerce.number(),
  totalPages: z.union([z.coerce.number(), z.undefined()]).optional(),
  totalCount: z.coerce.number(),
  hasPreviousPage: z.union([z.boolean(), z.undefined()]).optional(),
  hasNextPage: z.union([z.boolean(), z.undefined()]).optional(),
});

export type StatDto = z.infer<typeof StatDto>;
export const StatDto = z.object({
  id: z.string().optional(),
  unitCost: z.coerce.number().optional(),
  unitCost2: z.coerce.number().optional(),
  maxHp: z.coerce.number().optional(),
  downValueThreshold: z.coerce.number().optional(),
  yorukeValueThreshold: z.coerce.number().optional(),
  unk20: z.coerce.number().optional(),
  unk24: z.coerce.number().optional(),
  unk28: z.coerce.number().optional(),
  maxBoost: z.coerce.number().optional(),
  unk36: z.coerce.number().optional(),
  unk40: z.coerce.number().optional(),
  unk44: z.coerce.number().optional(),
  gravityMultiplierAir: z.coerce.number().optional(),
  gravityMultiplierLand: z.coerce.number().optional(),
  unk56: z.coerce.number().optional(),
  unk60: z.coerce.number().optional(),
  unk64: z.coerce.number().optional(),
  unk68: z.coerce.number().optional(),
  unk72: z.coerce.number().optional(),
  unk76: z.coerce.number().optional(),
  unk80: z.coerce.number().optional(),
  cameraZoomMultiplier: z.coerce.number().optional(),
  unk88: z.coerce.number().optional(),
  unk92: z.coerce.number().optional(),
  unk96: z.coerce.number().optional(),
  unk100: z.coerce.number().optional(),
  unk104: z.coerce.number().optional(),
  unk108: z.coerce.number().optional(),
  sizeMultiplier: z.coerce.number().optional(),
  unk116: z.coerce.number().optional(),
  unk120: z.coerce.number().optional(),
  unk124: z.coerce.number().optional(),
  unk128: z.coerce.number().optional(),
  unk132: z.coerce.number().optional(),
  unk136: z.coerce.number().optional(),
  unk140: z.coerce.number().optional(),
  unk144: z.coerce.number().optional(),
  unk148: z.coerce.number().optional(),
  unk152: z.coerce.number().optional(),
  unk156: z.coerce.number().optional(),
  unk160: z.coerce.number().optional(),
  unk164: z.coerce.number().optional(),
  unk168: z.coerce.number().optional(),
  unk172: z.coerce.number().optional(),
  unk176: z.coerce.number().optional(),
  unk180: z.coerce.number().optional(),
  unk184: z.coerce.number().optional(),
  redLockRangeMelee: z.coerce.number().optional(),
  redLockRange: z.coerce.number().optional(),
  unk196: z.coerce.number().optional(),
  unk200: z.coerce.number().optional(),
  unk204: z.coerce.number().optional(),
  unk208: z.coerce.number().optional(),
  boostReplenish: z.coerce.number().optional(),
  unk216: z.coerce.number().optional(),
  boostInitialConsumption: z.coerce.number().optional(),
  boostFuwaInitialConsumption: z.coerce.number().optional(),
  boostFlyConsumption: z.coerce.number().optional(),
  boostGroundStepInitialConsumption: z.coerce.number().optional(),
  boostGroundStepConsumption: z.coerce.number().optional(),
  boostAirStepInitialConsumption: z.coerce.number().optional(),
  boostAirStepConsumption: z.coerce.number().optional(),
  boostBdInitialConsumption: z.coerce.number().optional(),
  boostBdConsumption: z.coerce.number().optional(),
  unk256: z.coerce.number().optional(),
  unk260: z.coerce.number().optional(),
  unk264: z.coerce.number().optional(),
  unk268: z.coerce.number().optional(),
  boostTransformInitialConsumption: z.coerce.number().optional(),
  boostTransformConsumption: z.coerce.number().optional(),
  boostNonVernierActionConsumption: z.coerce.number().optional(),
  boostPostActionConsumption: z.coerce.number().optional(),
  boostRainbowStepInitialConsumption: z.coerce.number().optional(),
  unk292: z.coerce.number().optional(),
  unk296: z.coerce.number().optional(),
  unk300: z.coerce.number().optional(),
  unk304: z.coerce.number().optional(),
  unk308: z.coerce.number().optional(),
  unk312: z.coerce.number().optional(),
  unk316: z.coerce.number().optional(),
  unk320: z.coerce.number().optional(),
  unk324: z.coerce.number().optional(),
  unk328: z.coerce.number().optional(),
  unk332: z.coerce.number().optional(),
  assaultBurstRedLockMelee: z.coerce.number().optional(),
  assaultBurstRedLock: z.coerce.number().optional(),
  assaultBurstDamageDealtMultiplier: z.coerce.number().optional(),
  assaultBurstDamageTakenMultiplier: z.coerce.number().optional(),
  assaultBurstMobilityMultiplier: z.coerce.number().optional(),
  assaultBurstDownValueDealtMultiplier: z.coerce.number().optional(),
  assaultBurstBoostConsumptionMultiplier: z.coerce.number().optional(),
  unk364: z.coerce.number().optional(),
  unk368: z.coerce.number().optional(),
  assaultBurstDamageDealtBurstGaugeIncreaseMultiplier: z.coerce
    .number()
    .optional(),
  assaultBurstDamageTakenBurstGaugeIncreaseMultiplier: z.coerce
    .number()
    .optional(),
  unk380: z.coerce.number().optional(),
  unk384: z.coerce.number().optional(),
  unk388: z.coerce.number().optional(),
  unk392: z.coerce.number().optional(),
  unk396: z.coerce.number().optional(),
  blastBurstRedLockMelee: z.coerce.number().optional(),
  blastBurstRedLock: z.coerce.number().optional(),
  blastBurstDamageDealtMultiplier: z.coerce.number().optional(),
  blastBurstDamageTakenMultiplier: z.coerce.number().optional(),
  blastBurstMobilityMultiplier: z.coerce.number().optional(),
  blastBurstDownValueDealtMultiplier: z.coerce.number().optional(),
  blastBurstBoostConsumptionMultiplier: z.coerce.number().optional(),
  unk428: z.coerce.number().optional(),
  unk432: z.coerce.number().optional(),
  blastBurstDamageDealtBurstGaugeIncreaseMultiplier: z.coerce
    .number()
    .optional(),
  blastBurstDamageTakenBurstGaugeIncreaseMultiplier: z.coerce
    .number()
    .optional(),
  unk444: z.coerce.number().optional(),
  unk448: z.coerce.number().optional(),
  unk452: z.coerce.number().optional(),
  unk456: z.coerce.number().optional(),
  unk460: z.coerce.number().optional(),
  thirdBurstRedLockMelee: z.coerce.number().optional(),
  thirdBurstRedLock: z.coerce.number().optional(),
  thirdBurstDamageDealtMultiplier: z.coerce.number().optional(),
  thirdBurstDamageTakenMultiplier: z.coerce.number().optional(),
  thirdBurstMobilityMultiplier: z.coerce.number().optional(),
  thirdBurstDownValueDealtMultiplier: z.coerce.number().optional(),
  thirdBurstBoostConsumptionMultiplier: z.coerce.number().optional(),
  unk492: z.coerce.number().optional(),
  unk496: z.coerce.number().optional(),
  thirdBurstDamageDealtBurstGaugeIncreaseMultiplier: z.coerce
    .number()
    .optional(),
  thirdBurstDamageTakenBurstGaugeIncreaseMultiplier: z.coerce
    .number()
    .optional(),
  unk508: z.coerce.number().optional(),
  unk512: z.coerce.number().optional(),
  unk516: z.coerce.number().optional(),
  unk520: z.coerce.number().optional(),
  unk524: z.coerce.number().optional(),
  fourthBurstRedLockMelee: z.coerce.number().optional(),
  fourthBurstRedLock: z.coerce.number().optional(),
  fourthBurstDamageDealtMultiplier: z.coerce.number().optional(),
  fourthBurstDamageTakenMultiplier: z.coerce.number().optional(),
  fourthBurstMobilityMultiplier: z.coerce.number().optional(),
  fourthBurstDownValueDealtMultiplier: z.coerce.number().optional(),
  fourthBurstBoostConsumptionMultiplier: z.coerce.number().optional(),
  unk572: z.coerce.number().optional(),
  unk576: z.coerce.number().optional(),
  fourthBurstDamageDealtBurstGaugeIncreaseMultiplier: z.coerce
    .number()
    .optional(),
  fourthBurstDamageTakenBurstGaugeIncreaseMultiplier: z.coerce
    .number()
    .optional(),
  unk588: z.coerce.number().optional(),
  unk592: z.coerce.number().optional(),
  unk596: z.coerce.number().optional(),
  unk600: z.coerce.number().optional(),
  unk604: z.coerce.number().optional(),
  unk608: z.coerce.number().optional(),
  order: z.coerce.number().optional(),
  unitId: z.union([z.coerce.number(), z.null()]).optional(),
});

export type PaginatedListOfStatDto = z.infer<typeof PaginatedListOfStatDto>;
export const PaginatedListOfStatDto = z.object({
  items: z.array(StatDto),
  pageNumber: z.coerce.number(),
  totalPages: z.union([z.coerce.number(), z.undefined()]).optional(),
  totalCount: z.coerce.number(),
  hasPreviousPage: z.union([z.boolean(), z.undefined()]).optional(),
  hasNextPage: z.union([z.boolean(), z.undefined()]).optional(),
});

export type UnitProjectileDto = z.infer<typeof UnitProjectileDto>;
export const UnitProjectileDto = z.object({
  unitId: z.coerce.number().optional(),
  projectiles: z.array(ProjectileDto).optional(),
});

export type PaginatedListOfUnitProjectileDto = z.infer<
  typeof PaginatedListOfUnitProjectileDto
>;
export const PaginatedListOfUnitProjectileDto = z.object({
  items: z.array(UnitProjectileDto),
  pageNumber: z.coerce.number(),
  totalPages: z.union([z.coerce.number(), z.undefined()]).optional(),
  totalCount: z.coerce.number(),
  hasPreviousPage: z.union([z.boolean(), z.undefined()]).optional(),
  hasNextPage: z.union([z.boolean(), z.undefined()]).optional(),
});

export type UnitAmmoSlotDto = z.infer<typeof UnitAmmoSlotDto>;
export const UnitAmmoSlotDto = z.object({
  id: z.union([z.string(), z.null()]).optional(),
  slotOrder: z.coerce.number().optional(),
  ammoHash: z.coerce.number().optional(),
});

export type UnitStatDto = z.infer<typeof UnitStatDto>;
export const UnitStatDto = z.object({
  id: z.union([z.string(), z.null()]).optional(),
  unitId: z.coerce.number().optional(),
  ammoSlots: z.array(UnitAmmoSlotDto).optional(),
});

export type PaginatedListOfUnitStatDto = z.infer<
  typeof PaginatedListOfUnitStatDto
>;
export const PaginatedListOfUnitStatDto = z.object({
  items: z.array(UnitStatDto),
  pageNumber: z.coerce.number(),
  totalPages: z.union([z.coerce.number(), z.undefined()]).optional(),
  totalCount: z.coerce.number(),
  hasPreviousPage: z.union([z.boolean(), z.undefined()]).optional(),
  hasNextPage: z.union([z.boolean(), z.undefined()]).optional(),
});

export type TblFileInfoMetadata = z.infer<typeof TblFileInfoMetadata>;
export const TblFileInfoMetadata = z.union([
  z.object({
    cumulativeIndex: z.coerce.number(),
    patchNumber: z.coerce.number(),
    size1: z.coerce.number(),
    size2: z.coerce.number(),
    size3: z.coerce.number(),
    size4: z.coerce.number(),
    hashName: z.coerce.number(),
  }),
  z.null(),
]);

export type PatchFileMetadataDto = z.infer<typeof PatchFileMetadataDto>;
export const PatchFileMetadataDto = z.object({
  path: z.union([z.string(), z.null()]).optional(),
  fileInfoMetadata: TblFileInfoMetadata.optional(),
});

export type PlayableCharacterDto = z.infer<typeof PlayableCharacterDto>;
export const PlayableCharacterDto = z.object({
  unitId: z.coerce.number().optional(),
  unitIndex: z.coerce.number().optional(),
  seriesId: z.coerce.number().optional(),
  unk2: z.coerce.number().optional(),
  fString: z.union([z.string(), z.null()]).optional(),
  fOutString: z.union([z.string(), z.null()]).optional(),
  pString: z.union([z.string(), z.null()]).optional(),
  unitSelectOrderInSeries: z.coerce.number().optional(),
  arcadeSmallSpriteIndex: z.coerce.number().optional(),
  arcadeUnitNameSpriteIndex: z.coerce.number().optional(),
  unk27: z.coerce.number().optional(),
  unk112: z.coerce.number().optional(),
  figurineSpriteIndex: z.coerce.number().optional(),
  unk114: z.coerce.number().optional(),
  unk124: z.coerce.number().optional(),
  unk128: z.coerce.number().optional(),
  catalogStorePilotCostume2TString: z.union([z.string(), z.null()]).optional(),
  catalogStorePilotCostume2String: z.union([z.string(), z.null()]).optional(),
  catalogStorePilotCostume3TString: z.union([z.string(), z.null()]).optional(),
  catalogStorePilotCostume3String: z.union([z.string(), z.null()]).optional(),
  unk156: z.coerce.number().optional(),
  arcadeSelectionCostume1SpriteAssetHash: z.coerce.number().optional(),
  arcadeSelectionCostume2SpriteAssetHash: z.coerce.number().optional(),
  arcadeSelectionCostume3SpriteAssetHash: z.coerce.number().optional(),
  loadingLeftCostume1SpriteAssetHash: z.coerce.number().optional(),
  loadingLeftCostume2SpriteAssetHash: z.coerce.number().optional(),
  loadingLeftCostume3SpriteAssetHash: z.coerce.number().optional(),
  loadingRightCostume1SpriteAssetHash: z.coerce.number().optional(),
  loadingRightCostume2SpriteAssetHash: z.coerce.number().optional(),
  loadingRightCostume3SpriteAssetHash: z.coerce.number().optional(),
  genericSelectionCostume1SpriteAssetHash: z.coerce.number().optional(),
  genericSelectionCostume2SpriteAssetHash: z.coerce.number().optional(),
  genericSelectionCostume3SpriteAssetHash: z.coerce.number().optional(),
  loadingTargetUnitSpriteAssetHash: z.coerce.number().optional(),
  loadingTargetPilotCostume1SpriteAssetHash: z.coerce.number().optional(),
  loadingTargetPilotCostume2SpriteAssetHash: z.coerce.number().optional(),
  loadingTargetPilotCostume3SpriteAssetHash: z.coerce.number().optional(),
  inGameSortieAndAwakeningPilotCostume1SpriteAssetHash: z.coerce
    .number()
    .optional(),
  inGameSortieAndAwakeningPilotCostume2SpriteAssetHash: z.coerce
    .number()
    .optional(),
  inGameSortieAndAwakeningPilotCostume3SpriteAssetHash: z.coerce
    .number()
    .optional(),
  spriteFramesAssetHash: z.coerce.number().optional(),
  resultSmallUnitSpriteAssetHash: z.coerce.number().optional(),
  figurineSpriteAssetHash: z.coerce.number().optional(),
  loadingTargetUnitSmallSpriteAssetHash: z.coerce.number().optional(),
  catalogStorePilotCostume2SpriteAssetHash: z.coerce.number().optional(),
  catalogStorePilotCostume3SpriteAssetHash: z.coerce.number().optional(),
});

export type ResizePatchFileCommand = z.infer<typeof ResizePatchFileCommand>;
export const ResizePatchFileCommand = z.object({
  ids: z.union([z.array(z.string()), z.null()]).optional(),
  versions: z.union([z.array(PatchFileVersion), z.null()]).optional(),
  unitIds: z.union([z.array(z.coerce.number()), z.null()]).optional(),
  assetFileTypes: z.union([z.array(AssetFileType), z.null()]).optional(),
});

export type SerializeTbl = z.infer<typeof SerializeTbl>;
export const SerializeTbl = z.object({
  cumulativeFileInfoCount: z.coerce.number(),
  fileMetadata: z.array(PatchFileMetadataDto),
  pathOrder: z.union([z.array(z.string()), z.null(), z.undefined()]).optional(),
});

export type TblDto = z.infer<typeof TblDto>;
export const TblDto = z.object({
  cumulativeFileInfoCount: z.coerce.number(),
  fileMetadata: z.array(PatchFileMetadataDto),
  pathOrder: z.union([z.array(z.string()), z.null(), z.undefined()]).optional(),
});

export type TblVm = z.infer<typeof TblVm>;
export const TblVm = z.object({
  id: z.string(),
  cumulativeAssetIndex: z.union([z.coerce.number(), z.undefined()]).optional(),
});

export type UnpackFhmAssetCommand = z.infer<typeof UnpackFhmAssetCommand>;
export const UnpackFhmAssetCommand = z.object({
  assetFileHashes: z.union([z.array(z.coerce.number()), z.null()]).optional(),
  assetFileTypes: z.union([z.array(AssetFileType), z.null()]).optional(),
  unitIds: z.union([z.array(z.coerce.number()), z.null()]).optional(),
  patchFileVersions: z.union([z.array(PatchFileVersion), z.null()]).optional(),
  replaceWorking: z.boolean().optional(),
});

export type UnpackPsarcByPatchFilesCommand = z.infer<
  typeof UnpackPsarcByPatchFilesCommand
>;
export const UnpackPsarcByPatchFilesCommand = z.object({
  patchFileVersions: z.union([z.array(PatchFileVersion), z.null()]).optional(),
});

export type UnpackPsarcByPathCommand = z.infer<typeof UnpackPsarcByPathCommand>;
export const UnpackPsarcByPathCommand = z.object({
  sourceFilePath: z.string(),
  outputDirectoryPath: z.string(),
});

export type UpdateAmmoCommand = z.infer<typeof UpdateAmmoCommand>;
export const UpdateAmmoCommand = z.object({
  hash: z.coerce.number(),
  ammoType: z.union([z.coerce.number(), z.undefined()]).optional(),
  maxAmmo: z.union([z.coerce.number(), z.undefined()]).optional(),
  initialAmmo: z.union([z.coerce.number(), z.undefined()]).optional(),
  timedDurationFrame: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk16: z.union([z.coerce.number(), z.undefined()]).optional(),
  reloadType: z.union([z.coerce.number(), z.undefined()]).optional(),
  cooldownDurationFrame: z.union([z.coerce.number(), z.undefined()]).optional(),
  reloadDurationFrame: z.union([z.coerce.number(), z.undefined()]).optional(),
  assaultBurstReloadDurationFrame: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  blastBurstReloadDurationFrame: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  unk40: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk44: z.union([z.coerce.number(), z.undefined()]).optional(),
  inactiveUnk48: z.union([z.coerce.number(), z.undefined()]).optional(),
  inactiveCooldownDurationFrame: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  inactiveReloadDurationFrame: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  inactiveAssaultBurstReloadDurationFrame: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  inactiveBlastBurstReloadDurationFrame: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  inactiveUnk68: z.union([z.coerce.number(), z.undefined()]).optional(),
  inactiveUnk72: z.union([z.coerce.number(), z.undefined()]).optional(),
  burstReplenish: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk80: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk84: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk88: z.union([z.coerce.number(), z.undefined()]).optional(),
  chargeInput: z.union([z.coerce.number(), z.undefined()]).optional(),
  chargeDurationFrame: z.union([z.coerce.number(), z.undefined()]).optional(),
  assaultBurstChargeDurationFrame: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  blastBurstChargeDurationFrame: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  unk108: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk112: z.union([z.coerce.number(), z.undefined()]).optional(),
  releaseChargeLingerDurationFrame: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  maxChargeLevel: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk124: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk128: z.union([z.coerce.number(), z.undefined()]).optional(),
  order: z.union([z.coerce.number(), z.undefined()]).optional(),
  unitId: z.union([z.coerce.number(), z.null(), z.undefined()]).optional(),
});

export type UpdateAssetFileByHashCommand = z.infer<
  typeof UpdateAssetFileByHashCommand
>;
export const UpdateAssetFileByHashCommand = z.object({
  hash: z.coerce.number(),
  order: z.union([z.coerce.number(), z.undefined()]).optional(),
  fileType: z.union([z.array(AssetFileType), z.undefined()]).optional(),
  gameUnitId: z.union([z.coerce.number(), z.null(), z.undefined()]).optional(),
});

export type UpdateHitboxCommand = z.infer<typeof UpdateHitboxCommand>;
export const UpdateHitboxCommand = z.object({
  hash: z.coerce.number(),
  hitboxType: z.union([z.coerce.number(), z.undefined()]).optional(),
  damage: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk8: z.union([z.coerce.number(), z.undefined()]).optional(),
  downValueThreshold: z.union([z.coerce.number(), z.undefined()]).optional(),
  yorukeValueThreshold: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk20: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk24: z.union([z.coerce.number(), z.undefined()]).optional(),
  damageCorrection: z.union([z.coerce.number(), z.undefined()]).optional(),
  specialEffect: z.union([z.coerce.number(), z.undefined()]).optional(),
  hitEffect: z.union([z.coerce.number(), z.undefined()]).optional(),
  flyDirection1: z.union([z.coerce.number(), z.undefined()]).optional(),
  flyDirection2: z.union([z.coerce.number(), z.undefined()]).optional(),
  flyDirection3: z.union([z.coerce.number(), z.undefined()]).optional(),
  enemyCameraShakeMultiplier: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  playerCameraShakeMultiplier: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  unk56: z.union([z.coerce.number(), z.undefined()]).optional(),
  knockUpAngle: z.union([z.coerce.number(), z.undefined()]).optional(),
  knockUpRange: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk68: z.union([z.coerce.number(), z.undefined()]).optional(),
  multipleHitIntervalFrame: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  multipleHitCount: z.union([z.coerce.number(), z.undefined()]).optional(),
  enemyStunDuration: z.union([z.coerce.number(), z.undefined()]).optional(),
  playerStunDuration: z.union([z.coerce.number(), z.undefined()]).optional(),
  hitVisualEffect: z.union([z.coerce.number(), z.undefined()]).optional(),
  hitVisualEffectSizeMultiplier: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  hitSoundEffectHash: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk100: z.union([z.coerce.number(), z.undefined()]).optional(),
  friendlyDamageFlag: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk108: z.union([z.coerce.number(), z.undefined()]).optional(),
  hitboxGroupHash: z.union([z.coerce.number(), z.undefined()]).optional(),
});

export type UpdateHitboxGroupCommand = z.infer<typeof UpdateHitboxGroupCommand>;
export const UpdateHitboxGroupCommand = z.object({
  hash: z.coerce.number(),
  unitIds: z
    .union([z.array(z.coerce.number()), z.null(), z.undefined()])
    .optional(),
});

export type UpdatePatchFileByIdCommand = z.infer<
  typeof UpdatePatchFileByIdCommand
>;
export const UpdatePatchFileByIdCommand = z.object({
  id: z.string(),
  tblId: PatchFileVersion,
  pathInfo: z.union([PathInfoDto, z.undefined()]).optional(),
  fileInfo: z.union([FileInfoDto, z.undefined()]).optional(),
  assetFileHash: z
    .union([z.coerce.number(), z.null(), z.undefined()])
    .optional(),
});

export type UpdateProjectileByIdCommand = z.infer<
  typeof UpdateProjectileByIdCommand
>;
export const UpdateProjectileByIdCommand = z.object({
  hash: z.coerce.number(),
  projectileType: z.union([z.coerce.number(), z.undefined()]).optional(),
  hitboxHash: z.union([z.coerce.number(), z.null(), z.undefined()]).optional(),
  modelHash: z.union([z.coerce.number(), z.undefined()]).optional(),
  skeletonIndex: z.union([z.coerce.number(), z.undefined()]).optional(),
  aimType: z.union([z.coerce.number(), z.undefined()]).optional(),
  translateY: z.union([z.coerce.number(), z.undefined()]).optional(),
  translateZ: z.union([z.coerce.number(), z.undefined()]).optional(),
  translateX: z.union([z.coerce.number(), z.undefined()]).optional(),
  rotateX: z.union([z.coerce.number(), z.undefined()]).optional(),
  rotateZ: z.union([z.coerce.number(), z.undefined()]).optional(),
  cosmeticHash: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk44: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk48: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk52: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk56: z.union([z.coerce.number(), z.undefined()]).optional(),
  ammoConsumption: z.union([z.coerce.number(), z.undefined()]).optional(),
  durationFrame: z.union([z.coerce.number(), z.undefined()]).optional(),
  maxTravelDistance: z.union([z.coerce.number(), z.undefined()]).optional(),
  initialSpeed: z.union([z.coerce.number(), z.undefined()]).optional(),
  acceleration: z.union([z.coerce.number(), z.undefined()]).optional(),
  accelerationStartFrame: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  unk84: z.union([z.coerce.number(), z.undefined()]).optional(),
  maxSpeed: z.union([z.coerce.number(), z.undefined()]).optional(),
  reserved92: z.union([z.coerce.number(), z.undefined()]).optional(),
  reserved96: z.union([z.coerce.number(), z.undefined()]).optional(),
  reserved100: z.union([z.coerce.number(), z.undefined()]).optional(),
  reserved104: z.union([z.coerce.number(), z.undefined()]).optional(),
  reserved108: z.union([z.coerce.number(), z.undefined()]).optional(),
  reserved112: z.union([z.coerce.number(), z.undefined()]).optional(),
  reserved116: z.union([z.coerce.number(), z.undefined()]).optional(),
  horizontalGuidance: z.union([z.coerce.number(), z.undefined()]).optional(),
  horizontalGuidanceAngle: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  verticalGuidance: z.union([z.coerce.number(), z.undefined()]).optional(),
  verticalGuidanceAngle: z.union([z.coerce.number(), z.undefined()]).optional(),
  reserved136: z.union([z.coerce.number(), z.undefined()]).optional(),
  reserved140: z.union([z.coerce.number(), z.undefined()]).optional(),
  reserved144: z.union([z.coerce.number(), z.undefined()]).optional(),
  reserved148: z.union([z.coerce.number(), z.undefined()]).optional(),
  reserved152: z.union([z.coerce.number(), z.undefined()]).optional(),
  reserved156: z.union([z.coerce.number(), z.undefined()]).optional(),
  reserved160: z.union([z.coerce.number(), z.undefined()]).optional(),
  reserved164: z.union([z.coerce.number(), z.undefined()]).optional(),
  reserved168: z.union([z.coerce.number(), z.undefined()]).optional(),
  reserved172: z.union([z.coerce.number(), z.undefined()]).optional(),
  size: z.union([z.coerce.number(), z.undefined()]).optional(),
  reserved180: z.union([z.coerce.number(), z.undefined()]).optional(),
  reserved184: z.union([z.coerce.number(), z.undefined()]).optional(),
  soundEffectHash: z.union([z.coerce.number(), z.undefined()]).optional(),
  reserved192: z.union([z.coerce.number(), z.undefined()]).optional(),
  reserved196: z.union([z.coerce.number(), z.undefined()]).optional(),
  chainedProjectileHash: z.union([z.coerce.number(), z.undefined()]).optional(),
  reserved204: z.union([z.coerce.number(), z.undefined()]).optional(),
  reserved208: z.union([z.coerce.number(), z.undefined()]).optional(),
  reserved212: z.union([z.coerce.number(), z.undefined()]).optional(),
  reserved216: z.union([z.coerce.number(), z.undefined()]).optional(),
  reserved220: z.union([z.coerce.number(), z.undefined()]).optional(),
  reserved224: z.union([z.coerce.number(), z.undefined()]).optional(),
  reserved228: z.union([z.coerce.number(), z.undefined()]).optional(),
  reserved232: z.union([z.coerce.number(), z.undefined()]).optional(),
  reserved236: z.union([z.coerce.number(), z.undefined()]).optional(),
  reserved240: z.union([z.coerce.number(), z.undefined()]).optional(),
  reserved244: z.union([z.coerce.number(), z.undefined()]).optional(),
  reserved248: z.union([z.coerce.number(), z.undefined()]).optional(),
  reserved252: z.union([z.coerce.number(), z.undefined()]).optional(),
  reserved256: z.union([z.coerce.number(), z.undefined()]).optional(),
  reserved260: z.union([z.coerce.number(), z.undefined()]).optional(),
  reserved264: z.union([z.coerce.number(), z.undefined()]).optional(),
  reserved268: z.union([z.coerce.number(), z.undefined()]).optional(),
  reserved272: z.union([z.coerce.number(), z.undefined()]).optional(),
  reserved276: z.union([z.coerce.number(), z.undefined()]).optional(),
  unitId: z.union([z.coerce.number(), z.null(), z.undefined()]).optional(),
});

export type UpdateStatCommand = z.infer<typeof UpdateStatCommand>;
export const UpdateStatCommand = z.object({
  id: z.string(),
  unitCost: z.union([z.coerce.number(), z.undefined()]).optional(),
  unitCost2: z.union([z.coerce.number(), z.undefined()]).optional(),
  maxHp: z.union([z.coerce.number(), z.undefined()]).optional(),
  downValueThreshold: z.union([z.coerce.number(), z.undefined()]).optional(),
  yorukeValueThreshold: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk20: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk24: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk28: z.union([z.coerce.number(), z.undefined()]).optional(),
  maxBoost: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk36: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk40: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk44: z.union([z.coerce.number(), z.undefined()]).optional(),
  gravityMultiplierAir: z.union([z.coerce.number(), z.undefined()]).optional(),
  gravityMultiplierLand: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk56: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk60: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk64: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk68: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk72: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk76: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk80: z.union([z.coerce.number(), z.undefined()]).optional(),
  cameraZoomMultiplier: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk88: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk92: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk96: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk100: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk104: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk108: z.union([z.coerce.number(), z.undefined()]).optional(),
  sizeMultiplier: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk116: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk120: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk124: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk128: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk132: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk136: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk140: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk144: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk148: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk152: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk156: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk160: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk164: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk168: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk172: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk176: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk180: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk184: z.union([z.coerce.number(), z.undefined()]).optional(),
  redLockRangeMelee: z.union([z.coerce.number(), z.undefined()]).optional(),
  redLockRange: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk196: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk200: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk204: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk208: z.union([z.coerce.number(), z.undefined()]).optional(),
  boostReplenish: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk216: z.union([z.coerce.number(), z.undefined()]).optional(),
  boostInitialConsumption: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  boostFuwaInitialConsumption: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  boostFlyConsumption: z.union([z.coerce.number(), z.undefined()]).optional(),
  boostGroundStepInitialConsumption: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  boostGroundStepConsumption: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  boostAirStepInitialConsumption: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  boostAirStepConsumption: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  boostBdInitialConsumption: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  boostBdConsumption: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk256: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk260: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk264: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk268: z.union([z.coerce.number(), z.undefined()]).optional(),
  boostTransformInitialConsumption: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  boostTransformConsumption: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  boostNonVernierActionConsumption: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  boostPostActionConsumption: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  boostRainbowStepInitialConsumption: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  unk292: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk296: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk300: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk304: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk308: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk312: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk316: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk320: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk324: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk328: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk332: z.union([z.coerce.number(), z.undefined()]).optional(),
  assaultBurstRedLockMelee: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  assaultBurstRedLock: z.union([z.coerce.number(), z.undefined()]).optional(),
  assaultBurstDamageDealtMultiplier: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  assaultBurstDamageTakenMultiplier: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  assaultBurstMobilityMultiplier: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  assaultBurstDownValueDealtMultiplier: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  assaultBurstBoostConsumptionMultiplier: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  unk364: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk368: z.union([z.coerce.number(), z.undefined()]).optional(),
  assaultBurstDamageDealtBurstGaugeIncreaseMultiplier: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  assaultBurstDamageTakenBurstGaugeIncreaseMultiplier: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  unk380: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk384: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk388: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk392: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk396: z.union([z.coerce.number(), z.undefined()]).optional(),
  blastBurstRedLockMelee: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  blastBurstRedLock: z.union([z.coerce.number(), z.undefined()]).optional(),
  blastBurstDamageDealtMultiplier: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  blastBurstDamageTakenMultiplier: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  blastBurstMobilityMultiplier: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  blastBurstDownValueDealtMultiplier: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  blastBurstBoostConsumptionMultiplier: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  unk428: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk432: z.union([z.coerce.number(), z.undefined()]).optional(),
  blastBurstDamageDealtBurstGaugeIncreaseMultiplier: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  blastBurstDamageTakenBurstGaugeIncreaseMultiplier: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  unk444: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk448: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk452: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk456: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk460: z.union([z.coerce.number(), z.undefined()]).optional(),
  thirdBurstRedLockMelee: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  thirdBurstRedLock: z.union([z.coerce.number(), z.undefined()]).optional(),
  thirdBurstDamageDealtMultiplier: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  thirdBurstDamageTakenMultiplier: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  thirdBurstMobilityMultiplier: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  thirdBurstDownValueDealtMultiplier: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  thirdBurstBoostConsumptionMultiplier: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  unk492: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk496: z.union([z.coerce.number(), z.undefined()]).optional(),
  thirdBurstDamageDealtBurstGaugeIncreaseMultiplier: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  thirdBurstDamageTakenBurstGaugeIncreaseMultiplier: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  unk508: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk512: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk516: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk520: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk524: z.union([z.coerce.number(), z.undefined()]).optional(),
  fourthBurstRedLockMelee: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  fourthBurstRedLock: z.union([z.coerce.number(), z.undefined()]).optional(),
  fourthBurstDamageDealtMultiplier: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  fourthBurstDamageTakenMultiplier: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  fourthBurstMobilityMultiplier: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  fourthBurstDownValueDealtMultiplier: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  fourthBurstBoostConsumptionMultiplier: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  unk572: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk576: z.union([z.coerce.number(), z.undefined()]).optional(),
  fourthBurstDamageDealtBurstGaugeIncreaseMultiplier: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  fourthBurstDamageTakenBurstGaugeIncreaseMultiplier: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  unk588: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk592: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk596: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk600: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk604: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk608: z.union([z.coerce.number(), z.undefined()]).optional(),
  order: z.union([z.coerce.number(), z.undefined()]).optional(),
  unitId: z.union([z.coerce.number(), z.null(), z.undefined()]).optional(),
});

export type UpdateUnitAmmoSlotCommand = z.infer<
  typeof UpdateUnitAmmoSlotCommand
>;
export const UpdateUnitAmmoSlotCommand = z.object({
  id: z.string(),
  unitId: z.coerce.number(),
  slotOrder: z.union([z.coerce.number(), z.null(), z.undefined()]).optional(),
  ammoHash: z.union([z.coerce.number(), z.null(), z.undefined()]).optional(),
});

export type UpdateUnitCommand = z.infer<typeof UpdateUnitCommand>;
export const UpdateUnitCommand = z.object({
  unitId: z.coerce.number().optional(),
  slugName: z.union([z.string(), z.null()]).optional(),
  nameEnglish: z.union([z.string(), z.null()]).optional(),
  nameJapanese: z.union([z.string(), z.null()]).optional(),
  nameChinese: z.union([z.string(), z.null()]).optional(),
  seriesId: z.union([z.coerce.number(), z.null()]).optional(),
});

export type UpsertConfigCommand = z.infer<typeof UpsertConfigCommand>;
export const UpsertConfigCommand = z.object({
  key: z.string(),
  value: z.string(),
});

export type UpsertPlayableCharactersCommand = z.infer<
  typeof UpsertPlayableCharactersCommand
>;
export const UpsertPlayableCharactersCommand = z.object({
  unitId: z.coerce.number(),
  unitIndex: z.union([z.coerce.number(), z.undefined()]).optional(),
  seriesId: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk2: z.union([z.coerce.number(), z.undefined()]).optional(),
  fString: z.union([z.string(), z.null(), z.undefined()]).optional(),
  fOutString: z.union([z.string(), z.null(), z.undefined()]).optional(),
  pString: z.union([z.string(), z.null(), z.undefined()]).optional(),
  unitSelectOrderInSeries: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  arcadeSmallSpriteIndex: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  arcadeUnitNameSpriteIndex: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  unk27: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk112: z.union([z.coerce.number(), z.undefined()]).optional(),
  figurineSpriteIndex: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk114: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk124: z.union([z.coerce.number(), z.undefined()]).optional(),
  unk128: z.union([z.coerce.number(), z.undefined()]).optional(),
  catalogStorePilotCostume2TString: z
    .union([z.string(), z.null(), z.undefined()])
    .optional(),
  catalogStorePilotCostume2String: z
    .union([z.string(), z.null(), z.undefined()])
    .optional(),
  catalogStorePilotCostume3TString: z
    .union([z.string(), z.null(), z.undefined()])
    .optional(),
  catalogStorePilotCostume3String: z
    .union([z.string(), z.null(), z.undefined()])
    .optional(),
  unk156: z.union([z.coerce.number(), z.undefined()]).optional(),
  arcadeSelectionCostume1SpriteAssetHash: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  arcadeSelectionCostume2SpriteAssetHash: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  arcadeSelectionCostume3SpriteAssetHash: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  loadingLeftCostume1SpriteAssetHash: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  loadingLeftCostume2SpriteAssetHash: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  loadingLeftCostume3SpriteAssetHash: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  loadingRightCostume1SpriteAssetHash: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  loadingRightCostume2SpriteAssetHash: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  loadingRightCostume3SpriteAssetHash: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  genericSelectionCostume1SpriteAssetHash: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  genericSelectionCostume2SpriteAssetHash: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  genericSelectionCostume3SpriteAssetHash: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  loadingTargetUnitSpriteAssetHash: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  loadingTargetPilotCostume1SpriteAssetHash: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  loadingTargetPilotCostume2SpriteAssetHash: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  loadingTargetPilotCostume3SpriteAssetHash: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  inGameSortieAndAwakeningPilotCostume1SpriteAssetHash: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  inGameSortieAndAwakeningPilotCostume2SpriteAssetHash: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  inGameSortieAndAwakeningPilotCostume3SpriteAssetHash: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  spriteFramesAssetHash: z.union([z.coerce.number(), z.undefined()]).optional(),
  resultSmallUnitSpriteAssetHash: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  figurineSpriteAssetHash: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  loadingTargetUnitSmallSpriteAssetHash: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  catalogStorePilotCostume2SpriteAssetHash: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
  catalogStorePilotCostume3SpriteAssetHash: z
    .union([z.coerce.number(), z.undefined()])
    .optional(),
});

export type get_Get__api__ammo = typeof get_Get__api__ammo;
export const get_Get__api__ammo = {
  method: z.literal("GET"),
  path: z.literal("/api/ammo"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    query: z.object({
      Page: z.coerce.number().optional(),
      PerPage: z.coerce.number().optional(),
      Hash: z.array(z.coerce.number()).optional(),
      UnitIds: z.array(z.coerce.number()).optional(),
      Search: z.string().optional(),
    }),
  }),
  response: PaginatedListOfAmmoDto,
};

export type post_Post__api__ammo = typeof post_Post__api__ammo;
export const post_Post__api__ammo = {
  method: z.literal("POST"),
  path: z.literal("/api/ammo"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    body: CreateAmmoCommand,
  }),
  response: z.unknown(),
};

export type get_Get__api__ammo__by__hash = typeof get_Get__api__ammo__by__hash;
export const get_Get__api__ammo__by__hash = {
  method: z.literal("GET"),
  path: z.literal("/api/ammo/{hash}"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    path: z.object({
      hash: z.coerce.number(),
    }),
  }),
  response: AmmoDto,
};

export type post_Post__api__ammo__by__hash =
  typeof post_Post__api__ammo__by__hash;
export const post_Post__api__ammo__by__hash = {
  method: z.literal("POST"),
  path: z.literal("/api/ammo/{hash}"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    path: z.object({
      hash: z.coerce.number(),
    }),
    body: UpdateAmmoCommand,
  }),
  response: z.unknown(),
};

export type delete_Delete__api__ammo__by__hash =
  typeof delete_Delete__api__ammo__by__hash;
export const delete_Delete__api__ammo__by__hash = {
  method: z.literal("DELETE"),
  path: z.literal("/api/ammo/{hash}"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    path: z.object({
      hash: z.coerce.number(),
    }),
  }),
  response: z.unknown(),
};

export type get_Get__api__ammo__options = typeof get_Get__api__ammo__options;
export const get_Get__api__ammo__options = {
  method: z.literal("GET"),
  path: z.literal("/api/ammo/options"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    query: z.object({
      UnitIds: z.array(z.coerce.number()).optional(),
    }),
  }),
  response: z.array(z.coerce.number()),
};

export type post_Post__api__ammo__import = typeof post_Post__api__ammo__import;
export const post_Post__api__ammo__import = {
  method: z.literal("POST"),
  path: z.literal("/api/ammo/import"),
  requestFormat: z.literal("form-data"),
  parameters: z.object({
    body: z.object({
      formFile: IFormFile,
    }),
  }),
  response: z.unknown(),
};

export type post_Post__api__ammo__export = typeof post_Post__api__ammo__export;
export const post_Post__api__ammo__export = {
  method: z.literal("POST"),
  path: z.literal("/api/ammo/export"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    body: ExportAmmoCommand,
  }),
  response: z.unknown(),
};

export type post_Post__api__ammo__export__path =
  typeof post_Post__api__ammo__export__path;
export const post_Post__api__ammo__export__path = {
  method: z.literal("POST"),
  path: z.literal("/api/ammo/export/path"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    body: ExportAmmoByPathCommand,
  }),
  response: z.unknown(),
};

export type get_Get__api__configs = typeof get_Get__api__configs;
export const get_Get__api__configs = {
  method: z.literal("GET"),
  path: z.literal("/api/configs"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    query: z.object({
      Keys: z.array(z.string()),
    }),
  }),
  response: z.array(ConfigDto),
};

export type post_Post__api__configs = typeof post_Post__api__configs;
export const post_Post__api__configs = {
  method: z.literal("POST"),
  path: z.literal("/api/configs"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    body: UpsertConfigCommand,
  }),
  response: z.unknown(),
};

export type get_Get__api__configs__by__key =
  typeof get_Get__api__configs__by__key;
export const get_Get__api__configs__by__key = {
  method: z.literal("GET"),
  path: z.literal("/api/configs/{key}"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    path: z.object({
      key: z.string(),
    }),
  }),
  response: z.unknown(),
};

export type delete_Delete__api__configs__by__key =
  typeof delete_Delete__api__configs__by__key;
export const delete_Delete__api__configs__by__key = {
  method: z.literal("DELETE"),
  path: z.literal("/api/configs/{key}"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    path: z.object({
      key: z.string(),
    }),
  }),
  response: z.unknown(),
};

export type post_Post__api__debug = typeof post_Post__api__debug;
export const post_Post__api__debug = {
  method: z.literal("POST"),
  path: z.literal("/api/debug"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    body: DebugCommand,
  }),
  response: z.unknown(),
};

export type get_Get__api__fhm__pack__path =
  typeof get_Get__api__fhm__pack__path;
export const get_Get__api__fhm__pack__path = {
  method: z.literal("GET"),
  path: z.literal("/api/fhm/pack-path"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    query: z.object({
      SourcePath: z.string(),
      DestinationPath: z.string(),
      FileName: z.union([z.string(), z.undefined()]),
    }),
  }),
  response: z.unknown(),
};

export type get_Get__api__fhm__unpack__path =
  typeof get_Get__api__fhm__unpack__path;
export const get_Get__api__fhm__unpack__path = {
  method: z.literal("GET"),
  path: z.literal("/api/fhm/unpack-path"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    query: z.object({
      SourceFilePath: z.string(),
      OutputDirectoryPath: z.string(),
      MultipleFiles: z.union([z.boolean(), z.undefined()]),
    }),
  }),
  response: z.unknown(),
};

export type post_Post__api__fhm__pack = typeof post_Post__api__fhm__pack;
export const post_Post__api__fhm__pack = {
  method: z.literal("POST"),
  path: z.literal("/api/fhm/pack"),
  requestFormat: z.literal("form-data"),
  parameters: z.object({
    body: z.object({
      file: IFormFile,
    }),
  }),
  response: z.unknown(),
};

export type post_Post__api__fhm__unpack = typeof post_Post__api__fhm__unpack;
export const post_Post__api__fhm__unpack = {
  method: z.literal("POST"),
  path: z.literal("/api/fhm/unpack"),
  requestFormat: z.literal("form-data"),
  parameters: z.object({
    body: z.object({
      file: IFormFile,
    }),
  }),
  response: z.unknown(),
};

export type post_Post__api__fhm__pack__asset =
  typeof post_Post__api__fhm__pack__asset;
export const post_Post__api__fhm__pack__asset = {
  method: z.literal("POST"),
  path: z.literal("/api/fhm/pack/asset"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    body: PackFhmAssetCommand,
  }),
  response: z.unknown(),
};

export type post_Post__api__fhm__unpack__asset =
  typeof post_Post__api__fhm__unpack__asset;
export const post_Post__api__fhm__unpack__asset = {
  method: z.literal("POST"),
  path: z.literal("/api/fhm/unpack/asset"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    body: UnpackFhmAssetCommand,
  }),
  response: z.unknown(),
};

export type post_Post__api__psarc__pack__path =
  typeof post_Post__api__psarc__pack__path;
export const post_Post__api__psarc__pack__path = {
  method: z.literal("POST"),
  path: z.literal("/api/psarc/pack/path"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    body: PackPsarcByPathCommand,
  }),
  response: z.unknown(),
};

export type post_Post__api__psarc__unpack__path =
  typeof post_Post__api__psarc__unpack__path;
export const post_Post__api__psarc__unpack__path = {
  method: z.literal("POST"),
  path: z.literal("/api/psarc/unpack/path"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    body: UnpackPsarcByPathCommand,
  }),
  response: z.unknown(),
};

export type post_Post__api__psarc__pack__patch__files =
  typeof post_Post__api__psarc__pack__patch__files;
export const post_Post__api__psarc__pack__patch__files = {
  method: z.literal("POST"),
  path: z.literal("/api/psarc/pack/patch-files"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    body: PackPsarcByPatchFilesCommand,
  }),
  response: z.unknown(),
};

export type post_Post__api__psarc__unpack__patch__files =
  typeof post_Post__api__psarc__unpack__patch__files;
export const post_Post__api__psarc__unpack__patch__files = {
  method: z.literal("POST"),
  path: z.literal("/api/psarc/unpack/patch-files"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    body: UnpackPsarcByPatchFilesCommand,
  }),
  response: z.unknown(),
};

export type get_Get__api__scex__decompiled__by__unitId =
  typeof get_Get__api__scex__decompiled__by__unitId;
export const get_Get__api__scex__decompiled__by__unitId = {
  method: z.literal("GET"),
  path: z.literal("/api/scex/decompiled/{unitId}"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    path: z.object({
      unitId: z.coerce.number(),
    }),
  }),
  response: z.unknown(),
};

export type post_Post__api__scex__compile__path =
  typeof post_Post__api__scex__compile__path;
export const post_Post__api__scex__compile__path = {
  method: z.literal("POST"),
  path: z.literal("/api/scex/compile/path"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    body: CompileScexByPathCommand,
  }),
  response: z.unknown(),
};

export type post_Post__api__scex__decompile__path =
  typeof post_Post__api__scex__decompile__path;
export const post_Post__api__scex__decompile__path = {
  method: z.literal("POST"),
  path: z.literal("/api/scex/decompile/path"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    body: DecompileScexByPathCommand,
  }),
  response: z.unknown(),
};

export type post_Post__api__scex__hot__reload__path =
  typeof post_Post__api__scex__hot__reload__path;
export const post_Post__api__scex__hot__reload__path = {
  method: z.literal("POST"),
  path: z.literal("/api/scex/hot-reload/path"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    body: HotReloadScex,
  }),
  response: z.unknown(),
};

export type post_Post__api__scex__compile__units =
  typeof post_Post__api__scex__compile__units;
export const post_Post__api__scex__compile__units = {
  method: z.literal("POST"),
  path: z.literal("/api/scex/compile/units"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    body: CompileScexByUnitsCommand,
  }),
  response: z.unknown(),
};

export type post_Post__api__scex__decompile__units =
  typeof post_Post__api__scex__decompile__units;
export const post_Post__api__scex__decompile__units = {
  method: z.literal("POST"),
  path: z.literal("/api/scex/decompile/units"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    body: DecompileScexByUnitsCommand,
  }),
  response: z.unknown(),
};

export type get_Get__api__units = typeof get_Get__api__units;
export const get_Get__api__units = {
  method: z.literal("GET"),
  path: z.literal("/api/units"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    query: z.object({
      Search: z.string().optional(),
      UnitIds: z.array(z.coerce.number()).optional(),
      Languages: z.array(z.unknown()).optional(),
    }),
  }),
  response: z.array(UnitSummaryVm),
};

export type post_Post__api__units = typeof post_Post__api__units;
export const post_Post__api__units = {
  method: z.literal("POST"),
  path: z.literal("/api/units"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    body: CreateUnitCommand,
  }),
  response: z.unknown(),
};

export type get_Get__api__units__by__unitId =
  typeof get_Get__api__units__by__unitId;
export const get_Get__api__units__by__unitId = {
  method: z.literal("GET"),
  path: z.literal("/api/units/{unitId}"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    path: z.object({
      unitId: z.coerce.number(),
    }),
  }),
  response: UnitSummaryVm,
};

export type post_Post__api__units__by__unitId =
  typeof post_Post__api__units__by__unitId;
export const post_Post__api__units__by__unitId = {
  method: z.literal("POST"),
  path: z.literal("/api/units/{unitId}"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    path: z.object({
      unitId: z.coerce.number(),
    }),
    body: UpdateUnitCommand,
  }),
  response: z.unknown(),
};

export type get_Get__api__units__by__unitId__playable__characters =
  typeof get_Get__api__units__by__unitId__playable__characters;
export const get_Get__api__units__by__unitId__playable__characters = {
  method: z.literal("GET"),
  path: z.literal("/api/units/{unitId}/playable-characters"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    path: z.object({
      unitId: z.coerce.number(),
    }),
  }),
  response: PlayableCharacterDto,
};

export type post_Post__api__units__by__unitId__playable__characters =
  typeof post_Post__api__units__by__unitId__playable__characters;
export const post_Post__api__units__by__unitId__playable__characters = {
  method: z.literal("POST"),
  path: z.literal("/api/units/{unitId}/playable-characters"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    path: z.object({
      unitId: z.coerce.number(),
    }),
    body: UpsertPlayableCharactersCommand,
  }),
  response: z.unknown(),
};

export type post_Post__api__units__bulk = typeof post_Post__api__units__bulk;
export const post_Post__api__units__bulk = {
  method: z.literal("POST"),
  path: z.literal("/api/units/bulk"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    body: BulkCreateUnitCommand,
  }),
  response: z.unknown(),
};

export type post_Post__api__units__playable__characters__import =
  typeof post_Post__api__units__playable__characters__import;
export const post_Post__api__units__playable__characters__import = {
  method: z.literal("POST"),
  path: z.literal("/api/units/playable-characters/import"),
  requestFormat: z.literal("form-data"),
  parameters: z.object({
    body: z.object({
      file: IFormFile,
    }),
  }),
  response: z.unknown(),
};

export type post_Post__api__units__playable__characters__export =
  typeof post_Post__api__units__playable__characters__export;
export const post_Post__api__units__playable__characters__export = {
  method: z.literal("POST"),
  path: z.literal("/api/units/playable-characters/export"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    body: ExportPlayableCharactersCommand,
  }),
  response: z.unknown(),
};

export type post_Post__api__patch__files = typeof post_Post__api__patch__files;
export const post_Post__api__patch__files = {
  method: z.literal("POST"),
  path: z.literal("/api/patch-files"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    body: CreatePatchFileCommand,
  }),
  response: z.unknown(),
};

export type get_Get__api__patch__files = typeof get_Get__api__patch__files;
export const get_Get__api__patch__files = {
  method: z.literal("GET"),
  path: z.literal("/api/patch-files"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    query: z.object({
      Page: z.coerce.number().optional(),
      PerPage: z.coerce.number().optional(),
      Search: z.string().optional(),
      Versions: z.array(PatchFileVersion).optional(),
      UnitIds: z.array(z.coerce.number()).optional(),
    }),
  }),
  response: PaginatedListOfPatchFileVm,
};

export type get_Get__api__patch__files__summary =
  typeof get_Get__api__patch__files__summary;
export const get_Get__api__patch__files__summary = {
  method: z.literal("GET"),
  path: z.literal("/api/patch-files/summary"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    query: z.object({
      Page: z.coerce.number().optional(),
      PerPage: z.coerce.number().optional(),
      Versions: z.array(PatchFileVersion).optional(),
      UnitIds: z.array(z.coerce.number()).optional(),
      AssetFileHashes: z.array(z.coerce.number()).optional(),
      AssetFileTypes: z.array(AssetFileType).optional(),
    }),
  }),
  response: PaginatedListOfPatchFileSummaryVm,
};

export type get_Get__api__patch__files__by__id =
  typeof get_Get__api__patch__files__by__id;
export const get_Get__api__patch__files__by__id = {
  method: z.literal("GET"),
  path: z.literal("/api/patch-files/{id}"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    path: z.object({
      id: z.string(),
    }),
  }),
  response: PatchFileVm,
};

export type post_Post__api__patch__files__by__id =
  typeof post_Post__api__patch__files__by__id;
export const post_Post__api__patch__files__by__id = {
  method: z.literal("POST"),
  path: z.literal("/api/patch-files/{id}"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    path: z.object({
      id: z.string(),
    }),
    body: UpdatePatchFileByIdCommand,
  }),
  response: z.unknown(),
};

export type delete_Delete__api__patch__files__by__id =
  typeof delete_Delete__api__patch__files__by__id;
export const delete_Delete__api__patch__files__by__id = {
  method: z.literal("DELETE"),
  path: z.literal("/api/patch-files/{id}"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    path: z.object({
      id: z.string(),
    }),
  }),
  response: z.unknown(),
};

export type post_Post__api__patch__files__resize =
  typeof post_Post__api__patch__files__resize;
export const post_Post__api__patch__files__resize = {
  method: z.literal("POST"),
  path: z.literal("/api/patch-files/resize"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    body: ResizePatchFileCommand,
  }),
  response: z.unknown(),
};

export type get_Get__api__tbl__deserialize__path =
  typeof get_Get__api__tbl__deserialize__path;
export const get_Get__api__tbl__deserialize__path = {
  method: z.literal("GET"),
  path: z.literal("/api/tbl/deserialize-path"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    query: z.object({
      filePath: z.string(),
    }),
  }),
  response: TblDto,
};

export type post_Post__api__tbl__deserialize =
  typeof post_Post__api__tbl__deserialize;
export const post_Post__api__tbl__deserialize = {
  method: z.literal("POST"),
  path: z.literal("/api/tbl/deserialize"),
  requestFormat: z.literal("form-data"),
  parameters: z.object({
    body: z.object({
      file: IFormFile,
    }),
  }),
  response: TblDto,
};

export type get_Get__api__tbl__serialize__path =
  typeof get_Get__api__tbl__serialize__path;
export const get_Get__api__tbl__serialize__path = {
  method: z.literal("GET"),
  path: z.literal("/api/tbl/serialize-path"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    query: z.object({
      filePath: z.string(),
    }),
  }),
  response: z.unknown(),
};

export type post_Post__api__tbl__serialize =
  typeof post_Post__api__tbl__serialize;
export const post_Post__api__tbl__serialize = {
  method: z.literal("POST"),
  path: z.literal("/api/tbl/serialize"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    body: SerializeTbl,
  }),
  response: z.unknown(),
};

export type get_Get__api__tbl__by__id = typeof get_Get__api__tbl__by__id;
export const get_Get__api__tbl__by__id = {
  method: z.literal("GET"),
  path: z.literal("/api/tbl/{id}"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    path: z.object({
      id: z.unknown(),
    }),
  }),
  response: TblVm,
};

export type post_Post__api__tbl__import = typeof post_Post__api__tbl__import;
export const post_Post__api__tbl__import = {
  method: z.literal("POST"),
  path: z.literal("/api/tbl/import"),
  requestFormat: z.literal("form-data"),
  parameters: z.object({
    body: z.object({
      files: IFormFileCollection,
    }),
  }),
  response: z.unknown(),
};

export type post_Post__api__tbl__export = typeof post_Post__api__tbl__export;
export const post_Post__api__tbl__export = {
  method: z.literal("POST"),
  path: z.literal("/api/tbl/export"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    body: ExportTblCommand,
  }),
  response: z.unknown(),
};

export type get_Get__api__stats = typeof get_Get__api__stats;
export const get_Get__api__stats = {
  method: z.literal("GET"),
  path: z.literal("/api/stats"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    query: z.object({
      Page: z.coerce.number().optional(),
      PerPage: z.coerce.number().optional(),
      Ids: z.array(z.string()).optional(),
      UnitIds: z.array(z.coerce.number()).optional(),
    }),
  }),
  response: PaginatedListOfStatDto,
};

export type post_Post__api__stats = typeof post_Post__api__stats;
export const post_Post__api__stats = {
  method: z.literal("POST"),
  path: z.literal("/api/stats"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    body: CreateStatCommand,
  }),
  response: z.unknown(),
};

export type get_Get__api__stats__by__id = typeof get_Get__api__stats__by__id;
export const get_Get__api__stats__by__id = {
  method: z.literal("GET"),
  path: z.literal("/api/stats/{id}"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    path: z.object({
      id: z.string(),
    }),
  }),
  response: StatDto,
};

export type post_Post__api__stats__by__id =
  typeof post_Post__api__stats__by__id;
export const post_Post__api__stats__by__id = {
  method: z.literal("POST"),
  path: z.literal("/api/stats/{id}"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    path: z.object({
      id: z.string(),
    }),
    body: UpdateStatCommand,
  }),
  response: z.unknown(),
};

export type delete_Delete__api__stats__by__id =
  typeof delete_Delete__api__stats__by__id;
export const delete_Delete__api__stats__by__id = {
  method: z.literal("DELETE"),
  path: z.literal("/api/stats/{id}"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    path: z.object({
      id: z.string(),
    }),
  }),
  response: z.unknown(),
};

export type get_Get__api__unit__stats = typeof get_Get__api__unit__stats;
export const get_Get__api__unit__stats = {
  method: z.literal("GET"),
  path: z.literal("/api/unit-stats"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    query: z.object({
      Page: z.coerce.number().optional(),
      PerPage: z.coerce.number().optional(),
      UnitIds: z.array(z.coerce.number()).optional(),
    }),
  }),
  response: PaginatedListOfUnitStatDto,
};

export type get_Get__api__unit__stats__by__unitId =
  typeof get_Get__api__unit__stats__by__unitId;
export const get_Get__api__unit__stats__by__unitId = {
  method: z.literal("GET"),
  path: z.literal("/api/unit-stats/{unitId}"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    path: z.object({
      unitId: z.coerce.number(),
    }),
  }),
  response: UnitStatDto,
};

export type post_Post__api__unit__stats__import =
  typeof post_Post__api__unit__stats__import;
export const post_Post__api__unit__stats__import = {
  method: z.literal("POST"),
  path: z.literal("/api/unit-stats/import"),
  requestFormat: z.literal("form-data"),
  parameters: z.object({
    body: z.object({
      files: IFormFileCollection,
    }),
  }),
  response: z.unknown(),
};

export type post_Post__api__unit__stats__export =
  typeof post_Post__api__unit__stats__export;
export const post_Post__api__unit__stats__export = {
  method: z.literal("POST"),
  path: z.literal("/api/unit-stats/export"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    body: ExportUnitStatCommand,
  }),
  response: z.unknown(),
};

export type post_Post__api__unit__stats__export__path =
  typeof post_Post__api__unit__stats__export__path;
export const post_Post__api__unit__stats__export__path = {
  method: z.literal("POST"),
  path: z.literal("/api/unit-stats/export/path"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    body: ExportUnitStatByPathCommand,
  }),
  response: z.unknown(),
};

export type get_Get__api__unit__stats__ammo__slot__by__unitId =
  typeof get_Get__api__unit__stats__ammo__slot__by__unitId;
export const get_Get__api__unit__stats__ammo__slot__by__unitId = {
  method: z.literal("GET"),
  path: z.literal("/api/unit-stats/ammo-slot/{unitId}"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    path: z.object({
      unitId: z.coerce.number(),
    }),
  }),
  response: z.array(UnitAmmoSlotDto),
};

export type post_Post__api__unit__stats__ammo__slot =
  typeof post_Post__api__unit__stats__ammo__slot;
export const post_Post__api__unit__stats__ammo__slot = {
  method: z.literal("POST"),
  path: z.literal("/api/unit-stats/ammo-slot"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    body: CreateUnitAmmoSlotCommand,
  }),
  response: z.string(),
};

export type post_Post__api__unit__stats__ammo__slot__by__id =
  typeof post_Post__api__unit__stats__ammo__slot__by__id;
export const post_Post__api__unit__stats__ammo__slot__by__id = {
  method: z.literal("POST"),
  path: z.literal("/api/unit-stats/ammo-slot/{id}"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    path: z.object({
      id: z.string(),
    }),
    body: UpdateUnitAmmoSlotCommand,
  }),
  response: z.unknown(),
};

export type delete_Delete__api__unit__stats__ammo__slot__by__id =
  typeof delete_Delete__api__unit__stats__ammo__slot__by__id;
export const delete_Delete__api__unit__stats__ammo__slot__by__id = {
  method: z.literal("DELETE"),
  path: z.literal("/api/unit-stats/ammo-slot/{id}"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    path: z.object({
      id: z.string(),
    }),
  }),
  response: z.unknown(),
};

export type get_Get__api__series = typeof get_Get__api__series;
export const get_Get__api__series = {
  method: z.literal("GET"),
  path: z.literal("/api/series"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    query: z.object({
      Page: z.coerce.number().optional(),
      PerPage: z.coerce.number().optional(),
      Search: z.array(z.string()).optional(),
    }),
  }),
  response: PaginatedListOfSeriesDto,
};

export type post_Post__api__series = typeof post_Post__api__series;
export const post_Post__api__series = {
  method: z.literal("POST"),
  path: z.literal("/api/series"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    body: CreateSeriesCommand,
  }),
  response: z.unknown(),
};

export type get_Get__api__series__units = typeof get_Get__api__series__units;
export const get_Get__api__series__units = {
  method: z.literal("GET"),
  path: z.literal("/api/series/units"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    query: z.object({
      Page: z.coerce.number().optional(),
      PerPage: z.coerce.number().optional(),
      UnitIds: z.array(z.coerce.number()).optional(),
      ListAll: z.boolean().optional(),
    }),
  }),
  response: PaginatedListOfSeriesUnitsVm,
};

export type post_Post__api__series__import =
  typeof post_Post__api__series__import;
export const post_Post__api__series__import = {
  method: z.literal("POST"),
  path: z.literal("/api/series/import"),
  requestFormat: z.literal("form-data"),
  parameters: z.object({
    body: z.object({
      file: IFormFile,
    }),
  }),
  response: z.unknown(),
};

export type post_Post__api__series__export =
  typeof post_Post__api__series__export;
export const post_Post__api__series__export = {
  method: z.literal("POST"),
  path: z.literal("/api/series/export"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    body: ExportPlayableSeriesCommand,
  }),
  response: z.unknown(),
};

export type get_Get__api__projectiles = typeof get_Get__api__projectiles;
export const get_Get__api__projectiles = {
  method: z.literal("GET"),
  path: z.literal("/api/projectiles"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    query: z.object({
      Page: z.coerce.number().optional(),
      PerPage: z.coerce.number().optional(),
      Hashes: z.array(z.coerce.number()).optional(),
      UnitIds: z.array(z.coerce.number()).optional(),
      Search: z.string().optional(),
    }),
  }),
  response: PaginatedListOfProjectileDto,
};

export type post_Post__api__projectiles = typeof post_Post__api__projectiles;
export const post_Post__api__projectiles = {
  method: z.literal("POST"),
  path: z.literal("/api/projectiles"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    body: CreateProjectileCommand,
  }),
  response: z.unknown(),
};

export type get_Get__api__projectiles__by__hash =
  typeof get_Get__api__projectiles__by__hash;
export const get_Get__api__projectiles__by__hash = {
  method: z.literal("GET"),
  path: z.literal("/api/projectiles/{hash}"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    path: z.object({
      hash: z.coerce.number(),
    }),
  }),
  response: ProjectileDto,
};

export type post_Post__api__projectiles__by__hash =
  typeof post_Post__api__projectiles__by__hash;
export const post_Post__api__projectiles__by__hash = {
  method: z.literal("POST"),
  path: z.literal("/api/projectiles/{hash}"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    path: z.object({
      hash: z.coerce.number(),
    }),
    body: UpdateProjectileByIdCommand,
  }),
  response: z.unknown(),
};

export type delete_Delete__api__projectiles__by__hash =
  typeof delete_Delete__api__projectiles__by__hash;
export const delete_Delete__api__projectiles__by__hash = {
  method: z.literal("DELETE"),
  path: z.literal("/api/projectiles/{hash}"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    path: z.object({
      hash: z.coerce.number(),
    }),
  }),
  response: z.unknown(),
};

export type get_Get__api__unit__projectiles =
  typeof get_Get__api__unit__projectiles;
export const get_Get__api__unit__projectiles = {
  method: z.literal("GET"),
  path: z.literal("/api/unit-projectiles"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    query: z.object({
      Page: z.coerce.number().optional(),
      PerPage: z.coerce.number().optional(),
      UnitIds: z.array(z.coerce.number()).optional(),
      Search: z.string().optional(),
    }),
  }),
  response: PaginatedListOfUnitProjectileDto,
};

export type get_Get__api__unit__projectiles__by__unitId =
  typeof get_Get__api__unit__projectiles__by__unitId;
export const get_Get__api__unit__projectiles__by__unitId = {
  method: z.literal("GET"),
  path: z.literal("/api/unit-projectiles/{unitId}"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    path: z.object({
      unitId: z.coerce.number(),
    }),
  }),
  response: UnitProjectileDto,
};

export type post_Post__api__unit__projectiles__import =
  typeof post_Post__api__unit__projectiles__import;
export const post_Post__api__unit__projectiles__import = {
  method: z.literal("POST"),
  path: z.literal("/api/unit-projectiles/import"),
  requestFormat: z.literal("form-data"),
  parameters: z.object({
    body: z.object({
      files: IFormFileCollection,
    }),
  }),
  response: z.unknown(),
};

export type post_Post__api__unit__projectiles__import__path =
  typeof post_Post__api__unit__projectiles__import__path;
export const post_Post__api__unit__projectiles__import__path = {
  method: z.literal("POST"),
  path: z.literal("/api/unit-projectiles/import/path"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    query: z.object({
      directoryPath: z.string(),
    }),
  }),
  response: z.unknown(),
};

export type post_Post__api__unit__projectiles__export =
  typeof post_Post__api__unit__projectiles__export;
export const post_Post__api__unit__projectiles__export = {
  method: z.literal("POST"),
  path: z.literal("/api/unit-projectiles/export"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    body: ExportUnitProjectileCommand,
  }),
  response: z.unknown(),
};

export type post_Post__api__unit__projectiles__export__path =
  typeof post_Post__api__unit__projectiles__export__path;
export const post_Post__api__unit__projectiles__export__path = {
  method: z.literal("POST"),
  path: z.literal("/api/unit-projectiles/export/path"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    body: ExportUnitProjectileByPathCommand,
  }),
  response: z.unknown(),
};

export type get_Get__api__hitboxes = typeof get_Get__api__hitboxes;
export const get_Get__api__hitboxes = {
  method: z.literal("GET"),
  path: z.literal("/api/hitboxes"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    query: z.object({
      Page: z.coerce.number().optional(),
      PerPage: z.coerce.number().optional(),
      Hashes: z.array(z.coerce.number()).optional(),
      UnitIds: z.array(z.coerce.number()).optional(),
      Search: z.string().optional(),
    }),
  }),
  response: PaginatedListOfHitboxDto,
};

export type post_Post__api__hitboxes = typeof post_Post__api__hitboxes;
export const post_Post__api__hitboxes = {
  method: z.literal("POST"),
  path: z.literal("/api/hitboxes"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    body: CreateHitboxCommand,
  }),
  response: z.unknown(),
};

export type get_Get__api__hitboxes__by__hash =
  typeof get_Get__api__hitboxes__by__hash;
export const get_Get__api__hitboxes__by__hash = {
  method: z.literal("GET"),
  path: z.literal("/api/hitboxes/{hash}"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    path: z.object({
      hash: z.coerce.number(),
    }),
  }),
  response: HitboxDto,
};

export type post_Post__api__hitboxes__by__hash =
  typeof post_Post__api__hitboxes__by__hash;
export const post_Post__api__hitboxes__by__hash = {
  method: z.literal("POST"),
  path: z.literal("/api/hitboxes/{hash}"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    path: z.object({
      hash: z.coerce.number(),
    }),
    body: UpdateHitboxCommand,
  }),
  response: z.unknown(),
};

export type delete_Delete__api__hitboxes__by__hash =
  typeof delete_Delete__api__hitboxes__by__hash;
export const delete_Delete__api__hitboxes__by__hash = {
  method: z.literal("DELETE"),
  path: z.literal("/api/hitboxes/{hash}"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    path: z.object({
      hash: z.coerce.number(),
    }),
  }),
  response: z.unknown(),
};

export type get_Get__api__hitbox__groups = typeof get_Get__api__hitbox__groups;
export const get_Get__api__hitbox__groups = {
  method: z.literal("GET"),
  path: z.literal("/api/hitbox-groups"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    query: z.object({
      Page: z.coerce.number().optional(),
      PerPage: z.coerce.number().optional(),
      Hashes: z.array(z.coerce.number()).optional(),
      UnitIds: z.array(z.coerce.number()).optional(),
    }),
  }),
  response: PaginatedListOfHitboxGroupDto,
};

export type post_Post__api__hitbox__groups =
  typeof post_Post__api__hitbox__groups;
export const post_Post__api__hitbox__groups = {
  method: z.literal("POST"),
  path: z.literal("/api/hitbox-groups"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    body: CreateHitboxGroupCommand,
  }),
  response: z.unknown(),
};

export type get_Get__api__hitbox__groups__hash__by__hash =
  typeof get_Get__api__hitbox__groups__hash__by__hash;
export const get_Get__api__hitbox__groups__hash__by__hash = {
  method: z.literal("GET"),
  path: z.literal("/api/hitbox-groups/hash/{hash}"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    path: z.object({
      hash: z.coerce.number(),
    }),
  }),
  response: HitboxGroupDto,
};

export type get_Get__api__hitbox__groups__unitId__by__unitId =
  typeof get_Get__api__hitbox__groups__unitId__by__unitId;
export const get_Get__api__hitbox__groups__unitId__by__unitId = {
  method: z.literal("GET"),
  path: z.literal("/api/hitbox-groups/unitId/{unitId}"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    path: z.object({
      unitId: z.coerce.number(),
    }),
  }),
  response: HitboxGroupDto,
};

export type post_Post__api__hitbox__groups__by__hash =
  typeof post_Post__api__hitbox__groups__by__hash;
export const post_Post__api__hitbox__groups__by__hash = {
  method: z.literal("POST"),
  path: z.literal("/api/hitbox-groups/{hash}"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    path: z.object({
      hash: z.coerce.number(),
    }),
    body: UpdateHitboxGroupCommand,
  }),
  response: z.unknown(),
};

export type post_Post__api__hitbox__groups__import =
  typeof post_Post__api__hitbox__groups__import;
export const post_Post__api__hitbox__groups__import = {
  method: z.literal("POST"),
  path: z.literal("/api/hitbox-groups/import"),
  requestFormat: z.literal("form-data"),
  parameters: z.object({
    query: z.object({
      unitId: z.coerce.number().optional(),
    }),
    body: z.object({
      file: IFormFile,
    }),
  }),
  response: z.unknown(),
};

export type post_Post__api__hitbox__groups__import__path =
  typeof post_Post__api__hitbox__groups__import__path;
export const post_Post__api__hitbox__groups__import__path = {
  method: z.literal("POST"),
  path: z.literal("/api/hitbox-groups/import/path"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    query: z.object({
      directoryPath: z.string(),
    }),
  }),
  response: z.unknown(),
};

export type post_Post__api__hitbox__groups__export =
  typeof post_Post__api__hitbox__groups__export;
export const post_Post__api__hitbox__groups__export = {
  method: z.literal("POST"),
  path: z.literal("/api/hitbox-groups/export"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    body: ExportHitboxGroupCommand,
  }),
  response: z.unknown(),
};

export type post_Post__api__hitbox__groups__export__path =
  typeof post_Post__api__hitbox__groups__export__path;
export const post_Post__api__hitbox__groups__export__path = {
  method: z.literal("POST"),
  path: z.literal("/api/hitbox-groups/export/path"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    body: ExportHitboxGroupByPathCommand,
  }),
  response: z.unknown(),
};

export type post_Post__api__assets = typeof post_Post__api__assets;
export const post_Post__api__assets = {
  method: z.literal("POST"),
  path: z.literal("/api/assets"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    body: CreateAssetFileCommand,
  }),
  response: z.unknown(),
};

export type get_Get__api__assets = typeof get_Get__api__assets;
export const get_Get__api__assets = {
  method: z.literal("GET"),
  path: z.literal("/api/assets"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    query: z.object({
      Page: z.coerce.number().optional(),
      PerPage: z.coerce.number().optional(),
      UnitIds: z.array(z.coerce.number()).optional(),
      AssetFileTypes: z.array(AssetFileType).optional(),
    }),
  }),
  response: PaginatedListOfAssetFileVm,
};

export type get_Get__api__assets__by__hash =
  typeof get_Get__api__assets__by__hash;
export const get_Get__api__assets__by__hash = {
  method: z.literal("GET"),
  path: z.literal("/api/assets/{hash}"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    path: z.object({
      hash: z.coerce.number(),
    }),
  }),
  response: AssetFileVm,
};

export type post_Post__api__assets__by__hash =
  typeof post_Post__api__assets__by__hash;
export const post_Post__api__assets__by__hash = {
  method: z.literal("POST"),
  path: z.literal("/api/assets/{hash}"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    path: z.object({
      hash: z.coerce.number(),
    }),
    body: UpdateAssetFileByHashCommand,
  }),
  response: z.unknown(),
};

export type delete_Delete__api__assets__by__hash =
  typeof delete_Delete__api__assets__by__hash;
export const delete_Delete__api__assets__by__hash = {
  method: z.literal("DELETE"),
  path: z.literal("/api/assets/{hash}"),
  requestFormat: z.literal("json"),
  parameters: z.object({
    path: z.object({
      hash: z.coerce.number(),
    }),
  }),
  response: z.unknown(),
};

export type post_Post__api__assets__import =
  typeof post_Post__api__assets__import;
export const post_Post__api__assets__import = {
  method: z.literal("POST"),
  path: z.literal("/api/assets/import"),
  requestFormat: z.literal("form-data"),
  parameters: z.object({
    body: z.object({
      files: IFormFileCollection,
    }),
  }),
  response: z.unknown(),
};

// <EndpointByMethod>
export const EndpointByMethod = {
  get: {
    "/api/ammo": get_Get__api__ammo,
    "/api/ammo/{hash}": get_Get__api__ammo__by__hash,
    "/api/ammo/options": get_Get__api__ammo__options,
    "/api/configs": get_Get__api__configs,
    "/api/configs/{key}": get_Get__api__configs__by__key,
    "/api/fhm/pack-path": get_Get__api__fhm__pack__path,
    "/api/fhm/unpack-path": get_Get__api__fhm__unpack__path,
    "/api/scex/decompiled/{unitId}": get_Get__api__scex__decompiled__by__unitId,
    "/api/units": get_Get__api__units,
    "/api/units/{unitId}": get_Get__api__units__by__unitId,
    "/api/units/{unitId}/playable-characters":
      get_Get__api__units__by__unitId__playable__characters,
    "/api/patch-files": get_Get__api__patch__files,
    "/api/patch-files/summary": get_Get__api__patch__files__summary,
    "/api/patch-files/{id}": get_Get__api__patch__files__by__id,
    "/api/tbl/deserialize-path": get_Get__api__tbl__deserialize__path,
    "/api/tbl/serialize-path": get_Get__api__tbl__serialize__path,
    "/api/tbl/{id}": get_Get__api__tbl__by__id,
    "/api/stats": get_Get__api__stats,
    "/api/stats/{id}": get_Get__api__stats__by__id,
    "/api/unit-stats": get_Get__api__unit__stats,
    "/api/unit-stats/{unitId}": get_Get__api__unit__stats__by__unitId,
    "/api/unit-stats/ammo-slot/{unitId}":
      get_Get__api__unit__stats__ammo__slot__by__unitId,
    "/api/series": get_Get__api__series,
    "/api/series/units": get_Get__api__series__units,
    "/api/projectiles": get_Get__api__projectiles,
    "/api/projectiles/{hash}": get_Get__api__projectiles__by__hash,
    "/api/unit-projectiles": get_Get__api__unit__projectiles,
    "/api/unit-projectiles/{unitId}":
      get_Get__api__unit__projectiles__by__unitId,
    "/api/hitboxes": get_Get__api__hitboxes,
    "/api/hitboxes/{hash}": get_Get__api__hitboxes__by__hash,
    "/api/hitbox-groups": get_Get__api__hitbox__groups,
    "/api/hitbox-groups/hash/{hash}":
      get_Get__api__hitbox__groups__hash__by__hash,
    "/api/hitbox-groups/unitId/{unitId}":
      get_Get__api__hitbox__groups__unitId__by__unitId,
    "/api/assets": get_Get__api__assets,
    "/api/assets/{hash}": get_Get__api__assets__by__hash,
  },
  post: {
    "/api/ammo": post_Post__api__ammo,
    "/api/ammo/{hash}": post_Post__api__ammo__by__hash,
    "/api/ammo/import": post_Post__api__ammo__import,
    "/api/ammo/export": post_Post__api__ammo__export,
    "/api/ammo/export/path": post_Post__api__ammo__export__path,
    "/api/configs": post_Post__api__configs,
    "/api/debug": post_Post__api__debug,
    "/api/fhm/pack": post_Post__api__fhm__pack,
    "/api/fhm/unpack": post_Post__api__fhm__unpack,
    "/api/fhm/pack/asset": post_Post__api__fhm__pack__asset,
    "/api/fhm/unpack/asset": post_Post__api__fhm__unpack__asset,
    "/api/psarc/pack/path": post_Post__api__psarc__pack__path,
    "/api/psarc/unpack/path": post_Post__api__psarc__unpack__path,
    "/api/psarc/pack/patch-files": post_Post__api__psarc__pack__patch__files,
    "/api/psarc/unpack/patch-files":
      post_Post__api__psarc__unpack__patch__files,
    "/api/scex/compile/path": post_Post__api__scex__compile__path,
    "/api/scex/decompile/path": post_Post__api__scex__decompile__path,
    "/api/scex/hot-reload/path": post_Post__api__scex__hot__reload__path,
    "/api/scex/compile/units": post_Post__api__scex__compile__units,
    "/api/scex/decompile/units": post_Post__api__scex__decompile__units,
    "/api/units": post_Post__api__units,
    "/api/units/{unitId}": post_Post__api__units__by__unitId,
    "/api/units/{unitId}/playable-characters":
      post_Post__api__units__by__unitId__playable__characters,
    "/api/units/bulk": post_Post__api__units__bulk,
    "/api/units/playable-characters/import":
      post_Post__api__units__playable__characters__import,
    "/api/units/playable-characters/export":
      post_Post__api__units__playable__characters__export,
    "/api/patch-files": post_Post__api__patch__files,
    "/api/patch-files/{id}": post_Post__api__patch__files__by__id,
    "/api/patch-files/resize": post_Post__api__patch__files__resize,
    "/api/tbl/deserialize": post_Post__api__tbl__deserialize,
    "/api/tbl/serialize": post_Post__api__tbl__serialize,
    "/api/tbl/import": post_Post__api__tbl__import,
    "/api/tbl/export": post_Post__api__tbl__export,
    "/api/stats": post_Post__api__stats,
    "/api/stats/{id}": post_Post__api__stats__by__id,
    "/api/unit-stats/import": post_Post__api__unit__stats__import,
    "/api/unit-stats/export": post_Post__api__unit__stats__export,
    "/api/unit-stats/export/path": post_Post__api__unit__stats__export__path,
    "/api/unit-stats/ammo-slot": post_Post__api__unit__stats__ammo__slot,
    "/api/unit-stats/ammo-slot/{id}":
      post_Post__api__unit__stats__ammo__slot__by__id,
    "/api/series": post_Post__api__series,
    "/api/series/import": post_Post__api__series__import,
    "/api/series/export": post_Post__api__series__export,
    "/api/projectiles": post_Post__api__projectiles,
    "/api/projectiles/{hash}": post_Post__api__projectiles__by__hash,
    "/api/unit-projectiles/import": post_Post__api__unit__projectiles__import,
    "/api/unit-projectiles/import/path":
      post_Post__api__unit__projectiles__import__path,
    "/api/unit-projectiles/export": post_Post__api__unit__projectiles__export,
    "/api/unit-projectiles/export/path":
      post_Post__api__unit__projectiles__export__path,
    "/api/hitboxes": post_Post__api__hitboxes,
    "/api/hitboxes/{hash}": post_Post__api__hitboxes__by__hash,
    "/api/hitbox-groups": post_Post__api__hitbox__groups,
    "/api/hitbox-groups/{hash}": post_Post__api__hitbox__groups__by__hash,
    "/api/hitbox-groups/import": post_Post__api__hitbox__groups__import,
    "/api/hitbox-groups/import/path":
      post_Post__api__hitbox__groups__import__path,
    "/api/hitbox-groups/export": post_Post__api__hitbox__groups__export,
    "/api/hitbox-groups/export/path":
      post_Post__api__hitbox__groups__export__path,
    "/api/assets": post_Post__api__assets,
    "/api/assets/{hash}": post_Post__api__assets__by__hash,
    "/api/assets/import": post_Post__api__assets__import,
  },
  delete: {
    "/api/ammo/{hash}": delete_Delete__api__ammo__by__hash,
    "/api/configs/{key}": delete_Delete__api__configs__by__key,
    "/api/patch-files/{id}": delete_Delete__api__patch__files__by__id,
    "/api/stats/{id}": delete_Delete__api__stats__by__id,
    "/api/unit-stats/ammo-slot/{id}":
      delete_Delete__api__unit__stats__ammo__slot__by__id,
    "/api/projectiles/{hash}": delete_Delete__api__projectiles__by__hash,
    "/api/hitboxes/{hash}": delete_Delete__api__hitboxes__by__hash,
    "/api/assets/{hash}": delete_Delete__api__assets__by__hash,
  },
};
export type EndpointByMethod = typeof EndpointByMethod;
// </EndpointByMethod>

// <EndpointByMethod.Shorthands>
export type GetEndpoints = EndpointByMethod["get"];
export type PostEndpoints = EndpointByMethod["post"];
export type DeleteEndpoints = EndpointByMethod["delete"];
export type AllEndpoints = EndpointByMethod[keyof EndpointByMethod];
// </EndpointByMethod.Shorthands>

// <ApiClientTypes>
export type EndpointParameters = {
  body?: unknown;
  query?: Record<string, unknown>;
  header?: Record<string, unknown>;
  path?: Record<string, unknown>;
};

export type MutationMethod = "post" | "put" | "patch" | "delete";
export type Method = "get" | "head" | "options" | MutationMethod;

type RequestFormat = "json" | "form-data" | "form-url" | "binary" | "text";

export type DefaultEndpoint = {
  parameters?: EndpointParameters | undefined;
  response: unknown;
};

export type Endpoint<TConfig extends DefaultEndpoint = DefaultEndpoint> = {
  operationId: string;
  method: Method;
  path: string;
  requestFormat: RequestFormat;
  parameters?: TConfig["parameters"];
  meta: {
    alias: string;
    hasParameters: boolean;
    areParametersRequired: boolean;
  };
  response: TConfig["response"];
};

type Fetcher = (
  method: Method,
  url: string,
  parameters?: EndpointParameters | undefined,
) => Promise<Endpoint["response"]>;

type RequiredKeys<T> = {
  [P in keyof T]-?: undefined extends T[P] ? never : P;
}[keyof T];

type MaybeOptionalArg<T> =
  RequiredKeys<T> extends never ? [config?: T] : [config: T];

// </ApiClientTypes>

// <ApiClient>
export class ApiClient {
  baseUrl: string = "";

  constructor(public fetcher: Fetcher) {}

  setBaseUrl(baseUrl: string) {
    this.baseUrl = baseUrl;
    return this;
  }

  // <ApiClient.get>
  get<Path extends keyof GetEndpoints, TEndpoint extends GetEndpoints[Path]>(
    path: Path,
    ...params: MaybeOptionalArg<z.infer<TEndpoint["parameters"]>>
  ): Promise<z.infer<TEndpoint["response"]>> {
    return this.fetcher("get", this.baseUrl + path, params[0]) as Promise<
      z.infer<TEndpoint["response"]>
    >;
  }
  // </ApiClient.get>

  // <ApiClient.post>
  post<Path extends keyof PostEndpoints, TEndpoint extends PostEndpoints[Path]>(
    path: Path,
    ...params: MaybeOptionalArg<z.infer<TEndpoint["parameters"]>>
  ): Promise<z.infer<TEndpoint["response"]>> {
    return this.fetcher("post", this.baseUrl + path, params[0]) as Promise<
      z.infer<TEndpoint["response"]>
    >;
  }
  // </ApiClient.post>

  // <ApiClient.delete>
  delete<
    Path extends keyof DeleteEndpoints,
    TEndpoint extends DeleteEndpoints[Path],
  >(
    path: Path,
    ...params: MaybeOptionalArg<z.infer<TEndpoint["parameters"]>>
  ): Promise<z.infer<TEndpoint["response"]>> {
    return this.fetcher("delete", this.baseUrl + path, params[0]) as Promise<
      z.infer<TEndpoint["response"]>
    >;
  }
  // </ApiClient.delete>
}

export function createApiClient(fetcher: Fetcher, baseUrl?: string) {
  return new ApiClient(fetcher).setBaseUrl(baseUrl ?? "");
}

/**
 Example usage:
 const api = createApiClient((method, url, params) =>
   fetch(url, { method, body: JSON.stringify(params) }).then((res) => res.json()),
 );
 api.get("/users").then((users) => console.log(users));
 api.post("/users", { body: { name: "John" } }).then((user) => console.log(user));
 api.put("/users/:id", { path: { id: 1 }, body: { name: "John" } }).then((user) => console.log(user));
*/

// </ApiClient
