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
import type { PatchFileVersion } from './PatchFileVersion';
import {
    PatchFileVersionFromJSON,
    PatchFileVersionFromJSONTyped,
    PatchFileVersionToJSON,
} from './PatchFileVersion';

/**
 * 
 * @export
 * @interface FileInfoDto
 */
export interface FileInfoDto {
    /**
     * 
     * @type {PatchFileVersion}
     * @memberof FileInfoDto
     */
    version: PatchFileVersion;
    /**
     * 
     * @type {number}
     * @memberof FileInfoDto
     */
    size1: number;
    /**
     * 
     * @type {number}
     * @memberof FileInfoDto
     */
    size2: number;
    /**
     * 
     * @type {number}
     * @memberof FileInfoDto
     */
    size3: number;
    /**
     * 
     * @type {number}
     * @memberof FileInfoDto
     */
    size4: number;
}

/**
 * Check if a given object implements the FileInfoDto interface.
 */
export function instanceOfFileInfoDto(value: object): value is FileInfoDto {
    if (!('version' in value) || value['version'] === undefined) return false;
    if (!('size1' in value) || value['size1'] === undefined) return false;
    if (!('size2' in value) || value['size2'] === undefined) return false;
    if (!('size3' in value) || value['size3'] === undefined) return false;
    if (!('size4' in value) || value['size4'] === undefined) return false;
    return true;
}

export function FileInfoDtoFromJSON(json: any): FileInfoDto {
    return FileInfoDtoFromJSONTyped(json, false);
}

export function FileInfoDtoFromJSONTyped(json: any, ignoreDiscriminator: boolean): FileInfoDto {
    if (json == null) {
        return json;
    }
    return {
        
        'version': PatchFileVersionFromJSON(json['version']),
        'size1': json['size1'],
        'size2': json['size2'],
        'size3': json['size3'],
        'size4': json['size4'],
    };
}

export function FileInfoDtoToJSON(value?: FileInfoDto | null): any {
    if (value == null) {
        return value;
    }
    return {
        
        'version': PatchFileVersionToJSON(value['version']),
        'size1': value['size1'],
        'size2': value['size2'],
        'size3': value['size3'],
        'size4': value['size4'],
    };
}

