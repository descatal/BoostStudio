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
 * @interface PostApiUnitStatsAmmoSlotByIdRequest
 */
export interface PostApiUnitStatsAmmoSlotByIdRequest {
    /**
     * 
     * @type {string}
     * @memberof PostApiUnitStatsAmmoSlotByIdRequest
     */
    id: string;
    /**
     * 
     * @type {number}
     * @memberof PostApiUnitStatsAmmoSlotByIdRequest
     */
    unitId: number;
    /**
     * 
     * @type {number}
     * @memberof PostApiUnitStatsAmmoSlotByIdRequest
     */
    slotOrder?: number | null;
    /**
     * 
     * @type {number}
     * @memberof PostApiUnitStatsAmmoSlotByIdRequest
     */
    ammoHash?: number | null;
}

/**
 * Check if a given object implements the PostApiUnitStatsAmmoSlotByIdRequest interface.
 */
export function instanceOfPostApiUnitStatsAmmoSlotByIdRequest(value: object): value is PostApiUnitStatsAmmoSlotByIdRequest {
    if (!('id' in value) || value['id'] === undefined) return false;
    if (!('unitId' in value) || value['unitId'] === undefined) return false;
    return true;
}

export function PostApiUnitStatsAmmoSlotByIdRequestFromJSON(json: any): PostApiUnitStatsAmmoSlotByIdRequest {
    return PostApiUnitStatsAmmoSlotByIdRequestFromJSONTyped(json, false);
}

export function PostApiUnitStatsAmmoSlotByIdRequestFromJSONTyped(json: any, ignoreDiscriminator: boolean): PostApiUnitStatsAmmoSlotByIdRequest {
    if (json == null) {
        return json;
    }
    return {
        
        'id': json['id'],
        'unitId': json['unitId'],
        'slotOrder': json['slotOrder'] == null ? undefined : json['slotOrder'],
        'ammoHash': json['ammoHash'] == null ? undefined : json['ammoHash'],
    };
}

export function PostApiUnitStatsAmmoSlotByIdRequestToJSON(value?: PostApiUnitStatsAmmoSlotByIdRequest | null): any {
    if (value == null) {
        return value;
    }
    return {
        
        'id': value['id'],
        'unitId': value['unitId'],
        'slotOrder': value['slotOrder'],
        'ammoHash': value['ammoHash'],
    };
}
