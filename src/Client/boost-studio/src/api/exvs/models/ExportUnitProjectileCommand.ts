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
 * @interface ExportUnitProjectileCommand
 */
export interface ExportUnitProjectileCommand {
    /**
     * 
     * @type {Array<number>}
     * @memberof ExportUnitProjectileCommand
     */
    unitIds?: Array<number> | null;
}

/**
 * Check if a given object implements the ExportUnitProjectileCommand interface.
 */
export function instanceOfExportUnitProjectileCommand(value: object): value is ExportUnitProjectileCommand {
    return true;
}

export function ExportUnitProjectileCommandFromJSON(json: any): ExportUnitProjectileCommand {
    return ExportUnitProjectileCommandFromJSONTyped(json, false);
}

export function ExportUnitProjectileCommandFromJSONTyped(json: any, ignoreDiscriminator: boolean): ExportUnitProjectileCommand {
    if (json == null) {
        return json;
    }
    return {
        
        'unitIds': json['unitIds'] == null ? undefined : json['unitIds'],
    };
}

export function ExportUnitProjectileCommandToJSON(value?: ExportUnitProjectileCommand | null): any {
    if (value == null) {
        return value;
    }
    return {
        
        'unitIds': value['unitIds'],
    };
}
