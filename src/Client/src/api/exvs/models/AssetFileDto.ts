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
import type { UnitSummaryVm } from './UnitSummaryVm';
import {
    UnitSummaryVmFromJSON,
    UnitSummaryVmFromJSONTyped,
    UnitSummaryVmToJSON,
} from './UnitSummaryVm';
import type { AssetFileType } from './AssetFileType';
import {
    AssetFileTypeFromJSON,
    AssetFileTypeFromJSONTyped,
    AssetFileTypeToJSON,
} from './AssetFileType';

/**
 * 
 * @export
 * @interface AssetFileDto
 */
export interface AssetFileDto {
    /**
     * 
     * @type {number}
     * @memberof AssetFileDto
     */
    hash: number;
    /**
     * 
     * @type {number}
     * @memberof AssetFileDto
     */
    order: number;
    /**
     * 
     * @type {Array<AssetFileType>}
     * @memberof AssetFileDto
     */
    fileType: Array<AssetFileType>;
    /**
     * 
     * @type {Array<UnitSummaryVm>}
     * @memberof AssetFileDto
     */
    units: Array<UnitSummaryVm>;
}

/**
 * Check if a given object implements the AssetFileDto interface.
 */
export function instanceOfAssetFileDto(value: object): value is AssetFileDto {
    if (!('hash' in value) || value['hash'] === undefined) return false;
    if (!('order' in value) || value['order'] === undefined) return false;
    if (!('fileType' in value) || value['fileType'] === undefined) return false;
    if (!('units' in value) || value['units'] === undefined) return false;
    return true;
}

export function AssetFileDtoFromJSON(json: any): AssetFileDto {
    return AssetFileDtoFromJSONTyped(json, false);
}

export function AssetFileDtoFromJSONTyped(json: any, ignoreDiscriminator: boolean): AssetFileDto {
    if (json == null) {
        return json;
    }
    return {
        
        'hash': json['hash'],
        'order': json['order'],
        'fileType': ((json['fileType'] as Array<any>).map(AssetFileTypeFromJSON)),
        'units': ((json['units'] as Array<any>).map(UnitSummaryVmFromJSON)),
    };
}

export function AssetFileDtoToJSON(value?: AssetFileDto | null): any {
    if (value == null) {
        return value;
    }
    return {
        
        'hash': value['hash'],
        'order': value['order'],
        'fileType': ((value['fileType'] as Array<any>).map(AssetFileTypeToJSON)),
        'units': ((value['units'] as Array<any>).map(UnitSummaryVmToJSON)),
    };
}

