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
import type { PostApiTblDeserialize200ResponseFileMetadataInner } from './PostApiTblDeserialize200ResponseFileMetadataInner';
import {
    PostApiTblDeserialize200ResponseFileMetadataInnerFromJSON,
    PostApiTblDeserialize200ResponseFileMetadataInnerFromJSONTyped,
    PostApiTblDeserialize200ResponseFileMetadataInnerToJSON,
} from './PostApiTblDeserialize200ResponseFileMetadataInner';

/**
 * 
 * @export
 * @interface TblDto2
 */
export interface TblDto2 {
    /**
     * 
     * @type {number}
     * @memberof TblDto2
     */
    cumulativeFileInfoCount: number;
    /**
     * 
     * @type {Array<PostApiTblDeserialize200ResponseFileMetadataInner>}
     * @memberof TblDto2
     */
    fileMetadata: Array<PostApiTblDeserialize200ResponseFileMetadataInner>;
    /**
     * 
     * @type {Array<string>}
     * @memberof TblDto2
     */
    pathOrder?: Array<string> | null;
}

/**
 * Check if a given object implements the TblDto2 interface.
 */
export function instanceOfTblDto2(value: object): value is TblDto2 {
    if (!('cumulativeFileInfoCount' in value) || value['cumulativeFileInfoCount'] === undefined) return false;
    if (!('fileMetadata' in value) || value['fileMetadata'] === undefined) return false;
    return true;
}

export function TblDto2FromJSON(json: any): TblDto2 {
    return TblDto2FromJSONTyped(json, false);
}

export function TblDto2FromJSONTyped(json: any, ignoreDiscriminator: boolean): TblDto2 {
    if (json == null) {
        return json;
    }
    return {
        
        'cumulativeFileInfoCount': json['cumulativeFileInfoCount'],
        'fileMetadata': ((json['fileMetadata'] as Array<any>).map(PostApiTblDeserialize200ResponseFileMetadataInnerFromJSON)),
        'pathOrder': json['pathOrder'] == null ? undefined : json['pathOrder'],
    };
}

export function TblDto2ToJSON(value?: TblDto2 | null): any {
    if (value == null) {
        return value;
    }
    return {
        
        'cumulativeFileInfoCount': value['cumulativeFileInfoCount'],
        'fileMetadata': ((value['fileMetadata'] as Array<any>).map(PostApiTblDeserialize200ResponseFileMetadataInnerToJSON)),
        'pathOrder': value['pathOrder'],
    };
}
