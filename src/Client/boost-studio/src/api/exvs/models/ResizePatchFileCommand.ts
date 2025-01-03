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
import type { AssetFileType } from './AssetFileType';
import {
    AssetFileTypeFromJSON,
    AssetFileTypeFromJSONTyped,
    AssetFileTypeToJSON,
} from './AssetFileType';
import type { PatchFileVersion } from './PatchFileVersion';
import {
    PatchFileVersionFromJSON,
    PatchFileVersionFromJSONTyped,
    PatchFileVersionToJSON,
} from './PatchFileVersion';

/**
 * 
 * @export
 * @interface ResizePatchFileCommand
 */
export interface ResizePatchFileCommand {
    /**
     * 
     * @type {Array<string>}
     * @memberof ResizePatchFileCommand
     */
    ids?: Array<string> | null;
    /**
     * 
     * @type {Array<PatchFileVersion>}
     * @memberof ResizePatchFileCommand
     */
    versions?: Array<PatchFileVersion> | null;
    /**
     * 
     * @type {Array<number>}
     * @memberof ResizePatchFileCommand
     */
    unitIds?: Array<number> | null;
    /**
     * 
     * @type {Array<AssetFileType>}
     * @memberof ResizePatchFileCommand
     */
    assetFileTypes?: Array<AssetFileType> | null;
}

/**
 * Check if a given object implements the ResizePatchFileCommand interface.
 */
export function instanceOfResizePatchFileCommand(value: object): value is ResizePatchFileCommand {
    return true;
}

export function ResizePatchFileCommandFromJSON(json: any): ResizePatchFileCommand {
    return ResizePatchFileCommandFromJSONTyped(json, false);
}

export function ResizePatchFileCommandFromJSONTyped(json: any, ignoreDiscriminator: boolean): ResizePatchFileCommand {
    if (json == null) {
        return json;
    }
    return {
        
        'ids': json['ids'] == null ? undefined : json['ids'],
        'versions': json['versions'] == null ? undefined : ((json['versions'] as Array<any>).map(PatchFileVersionFromJSON)),
        'unitIds': json['unitIds'] == null ? undefined : json['unitIds'],
        'assetFileTypes': json['assetFileTypes'] == null ? undefined : ((json['assetFileTypes'] as Array<any>).map(AssetFileTypeFromJSON)),
    };
}

export function ResizePatchFileCommandToJSON(value?: ResizePatchFileCommand | null): any {
    if (value == null) {
        return value;
    }
    return {
        
        'ids': value['ids'],
        'versions': value['versions'] == null ? undefined : ((value['versions'] as Array<any>).map(PatchFileVersionToJSON)),
        'unitIds': value['unitIds'],
        'assetFileTypes': value['assetFileTypes'] == null ? undefined : ((value['assetFileTypes'] as Array<any>).map(AssetFileTypeToJSON)),
    };
}

