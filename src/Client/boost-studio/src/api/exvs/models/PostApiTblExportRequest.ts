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
 * @interface PostApiTblExportRequest
 */
export interface PostApiTblExportRequest {
    /**
     * 
     * @type {Array<PatchFileVersion>}
     * @memberof PostApiTblExportRequest
     */
    versions?: Array<PatchFileVersion> | null;
}

/**
 * Check if a given object implements the PostApiTblExportRequest interface.
 */
export function instanceOfPostApiTblExportRequest(value: object): value is PostApiTblExportRequest {
    return true;
}

export function PostApiTblExportRequestFromJSON(json: any): PostApiTblExportRequest {
    return PostApiTblExportRequestFromJSONTyped(json, false);
}

export function PostApiTblExportRequestFromJSONTyped(json: any, ignoreDiscriminator: boolean): PostApiTblExportRequest {
    if (json == null) {
        return json;
    }
    return {
        
        'versions': json['versions'] == null ? undefined : ((json['versions'] as Array<any>).map(PatchFileVersionFromJSON)),
    };
}

export function PostApiTblExportRequestToJSON(value?: PostApiTblExportRequest | null): any {
    if (value == null) {
        return value;
    }
    return {
        
        'versions': value['versions'] == null ? undefined : ((value['versions'] as Array<any>).map(PatchFileVersionToJSON)),
    };
}
