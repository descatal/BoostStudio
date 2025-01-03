/* tslint:disable */
/* eslint-disable */
/**
 * BoostStudio.Web | exvs
 * No description provided (generated by Openapi Generator https://github.com/openapitools/openapi-generator)
 *
 * The version of the OpenAPI document: 1.0.0
 * 
 *
 * NOTE: This class is auto generated by OpenAPI Generator (https://openapi-generator.tech).
 * https://openapi-generator.tech
 * Do not edit the class manually.
 */


/**
 * 
 * @export
 */
export const AssetFileType = {
    Unknown: 'Unknown',
    Dummy: 'Dummy',
    Animations: 'Animations',
    ModelAndTexture: 'ModelAndTexture',
    Data: 'Data',
    Effects: 'Effects',
    SoundEffects: 'SoundEffects',
    InGamePilotVoiceLines: 'InGamePilotVoiceLines',
    WeaponSprites: 'WeaponSprites',
    InGameCutInSprites: 'InGameCutInSprites',
    SpriteFrames: 'SpriteFrames',
    VoiceLinesMetadata: 'VoiceLinesMetadata',
    PilotVoiceLines: 'PilotVoiceLines',
    Hitboxes: 'Hitboxes',
    Projectiles: 'Projectiles',
    Ammo: 'Ammo',
    ListInfo: 'ListInfo',
    UnitCostInfo: 'UnitCostInfo',
    FigurineSprites: 'FigurineSprites',
    MapSelectSprites: 'MapSelectSprites',
    ArcadeSelectSmallSprites: 'ArcadeSelectSmallSprites',
    ArcadeSelectUnitNameSprites: 'ArcadeSelectUnitNameSprites',
    CameraConfigs: 'CameraConfigs',
    CommonEffects: 'CommonEffects',
    CommonEffectParticles: 'CommonEffectParticles',
    CosmeticInfo: 'CosmeticInfo',
    TextStrings: 'TextStrings',
    SeriesLogoSprites: 'SeriesLogoSprites',
    SeriesLogoSprites2: 'SeriesLogoSprites2'
} as const;
export type AssetFileType = typeof AssetFileType[keyof typeof AssetFileType];


export function instanceOfAssetFileType(value: any): boolean {
    for (const key in AssetFileType) {
        if (Object.prototype.hasOwnProperty.call(AssetFileType, key)) {
            if ((AssetFileType as Record<string, AssetFileType>)[key] === value) {
                return true;
            }
        }
    }
    return false;
}

export function AssetFileTypeFromJSON(json: any): AssetFileType {
    return AssetFileTypeFromJSONTyped(json, false);
}

export function AssetFileTypeFromJSONTyped(json: any, ignoreDiscriminator: boolean): AssetFileType {
    return json as AssetFileType;
}

export function AssetFileTypeToJSON(value?: AssetFileType | null): any {
    return value as any;
}

