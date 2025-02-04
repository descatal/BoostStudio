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
 * @interface ExportUnitStatCommand
 */
export interface ExportUnitStatCommand {
    /**
     * 
     * @type {Array<number>}
     * @memberof ExportUnitStatCommand
     */
    unitIds?: Array<number> | null;
    /**
     * 
     * @type {boolean}
     * @memberof ExportUnitStatCommand
     */
    replaceWorking?: boolean;
}

/**
 * Check if a given object implements the ExportUnitStatCommand interface.
 */
export function instanceOfExportUnitStatCommand(value: object): value is ExportUnitStatCommand {
    return true;
}

export function ExportUnitStatCommandFromJSON(json: any): ExportUnitStatCommand {
    return ExportUnitStatCommandFromJSONTyped(json, false);
}

export function ExportUnitStatCommandFromJSONTyped(json: any, ignoreDiscriminator: boolean): ExportUnitStatCommand {
    if (json == null) {
        return json;
    }
    return {
        
        'unitIds': json['unitIds'] == null ? undefined : json['unitIds'],
        'replaceWorking': json['replaceWorking'] == null ? undefined : json['replaceWorking'],
    };
}

export function ExportUnitStatCommandToJSON(value?: ExportUnitStatCommand | null): any {
    if (value == null) {
        return value;
    }
    return {
        
        'unitIds': value['unitIds'],
        'replaceWorking': value['replaceWorking'],
    };
}

