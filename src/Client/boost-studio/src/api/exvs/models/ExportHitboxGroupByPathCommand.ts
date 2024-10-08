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
 * @interface ExportHitboxGroupByPathCommand
 */
export interface ExportHitboxGroupByPathCommand {
    /**
     * 
     * @type {Array<number>}
     * @memberof ExportHitboxGroupByPathCommand
     */
    hashes?: Array<number> | null;
    /**
     * 
     * @type {Array<number>}
     * @memberof ExportHitboxGroupByPathCommand
     */
    unitIds?: Array<number> | null;
    /**
     * 
     * @type {string}
     * @memberof ExportHitboxGroupByPathCommand
     */
    outputPath?: string | null;
}

/**
 * Check if a given object implements the ExportHitboxGroupByPathCommand interface.
 */
export function instanceOfExportHitboxGroupByPathCommand(value: object): value is ExportHitboxGroupByPathCommand {
    return true;
}

export function ExportHitboxGroupByPathCommandFromJSON(json: any): ExportHitboxGroupByPathCommand {
    return ExportHitboxGroupByPathCommandFromJSONTyped(json, false);
}

export function ExportHitboxGroupByPathCommandFromJSONTyped(json: any, ignoreDiscriminator: boolean): ExportHitboxGroupByPathCommand {
    if (json == null) {
        return json;
    }
    return {
        
        'hashes': json['hashes'] == null ? undefined : json['hashes'],
        'unitIds': json['unitIds'] == null ? undefined : json['unitIds'],
        'outputPath': json['outputPath'] == null ? undefined : json['outputPath'],
    };
}

export function ExportHitboxGroupByPathCommandToJSON(value?: ExportHitboxGroupByPathCommand | null): any {
    if (value == null) {
        return value;
    }
    return {
        
        'hashes': value['hashes'],
        'unitIds': value['unitIds'],
        'outputPath': value['outputPath'],
    };
}

