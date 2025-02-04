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

import { mapValues } from '../runtime';
/**
 * 
 * @export
 * @interface UpdateUnitCommand
 */
export interface UpdateUnitCommand {
    /**
     * 
     * @type {number}
     * @memberof UpdateUnitCommand
     */
    unitId?: number;
    /**
     * 
     * @type {string}
     * @memberof UpdateUnitCommand
     */
    slugName?: string | null;
    /**
     * 
     * @type {string}
     * @memberof UpdateUnitCommand
     */
    nameEnglish?: string | null;
    /**
     * 
     * @type {string}
     * @memberof UpdateUnitCommand
     */
    nameJapanese?: string | null;
    /**
     * 
     * @type {string}
     * @memberof UpdateUnitCommand
     */
    nameChinese?: string | null;
    /**
     * 
     * @type {number}
     * @memberof UpdateUnitCommand
     */
    seriesId?: number | null;
}

/**
 * Check if a given object implements the UpdateUnitCommand interface.
 */
export function instanceOfUpdateUnitCommand(value: object): value is UpdateUnitCommand {
    return true;
}

export function UpdateUnitCommandFromJSON(json: any): UpdateUnitCommand {
    return UpdateUnitCommandFromJSONTyped(json, false);
}

export function UpdateUnitCommandFromJSONTyped(json: any, ignoreDiscriminator: boolean): UpdateUnitCommand {
    if (json == null) {
        return json;
    }
    return {
        
        'unitId': json['unitId'] == null ? undefined : json['unitId'],
        'slugName': json['slugName'] == null ? undefined : json['slugName'],
        'nameEnglish': json['nameEnglish'] == null ? undefined : json['nameEnglish'],
        'nameJapanese': json['nameJapanese'] == null ? undefined : json['nameJapanese'],
        'nameChinese': json['nameChinese'] == null ? undefined : json['nameChinese'],
        'seriesId': json['seriesId'] == null ? undefined : json['seriesId'],
    };
}

export function UpdateUnitCommandToJSON(value?: UpdateUnitCommand | null): any {
    if (value == null) {
        return value;
    }
    return {
        
        'unitId': value['unitId'],
        'slugName': value['slugName'],
        'nameEnglish': value['nameEnglish'],
        'nameJapanese': value['nameJapanese'],
        'nameChinese': value['nameChinese'],
        'seriesId': value['seriesId'],
    };
}

