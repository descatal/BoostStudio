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
import type { AmmoDto } from './AmmoDto';
import {
    AmmoDtoFromJSON,
    AmmoDtoFromJSONTyped,
    AmmoDtoToJSON,
} from './AmmoDto';

/**
 * 
 * @export
 * @interface PaginatedListOfAmmoDto
 */
export interface PaginatedListOfAmmoDto {
    /**
     * 
     * @type {Array<AmmoDto>}
     * @memberof PaginatedListOfAmmoDto
     */
    items: Array<AmmoDto>;
    /**
     * 
     * @type {number}
     * @memberof PaginatedListOfAmmoDto
     */
    pageNumber: number;
    /**
     * 
     * @type {number}
     * @memberof PaginatedListOfAmmoDto
     */
    totalPages?: number;
    /**
     * 
     * @type {number}
     * @memberof PaginatedListOfAmmoDto
     */
    totalCount: number;
    /**
     * 
     * @type {boolean}
     * @memberof PaginatedListOfAmmoDto
     */
    hasPreviousPage?: boolean;
    /**
     * 
     * @type {boolean}
     * @memberof PaginatedListOfAmmoDto
     */
    hasNextPage?: boolean;
}

/**
 * Check if a given object implements the PaginatedListOfAmmoDto interface.
 */
export function instanceOfPaginatedListOfAmmoDto(value: object): value is PaginatedListOfAmmoDto {
    if (!('items' in value) || value['items'] === undefined) return false;
    if (!('pageNumber' in value) || value['pageNumber'] === undefined) return false;
    if (!('totalCount' in value) || value['totalCount'] === undefined) return false;
    return true;
}

export function PaginatedListOfAmmoDtoFromJSON(json: any): PaginatedListOfAmmoDto {
    return PaginatedListOfAmmoDtoFromJSONTyped(json, false);
}

export function PaginatedListOfAmmoDtoFromJSONTyped(json: any, ignoreDiscriminator: boolean): PaginatedListOfAmmoDto {
    if (json == null) {
        return json;
    }
    return {
        
        'items': ((json['items'] as Array<any>).map(AmmoDtoFromJSON)),
        'pageNumber': json['pageNumber'],
        'totalPages': json['totalPages'] == null ? undefined : json['totalPages'],
        'totalCount': json['totalCount'],
        'hasPreviousPage': json['hasPreviousPage'] == null ? undefined : json['hasPreviousPage'],
        'hasNextPage': json['hasNextPage'] == null ? undefined : json['hasNextPage'],
    };
}

export function PaginatedListOfAmmoDtoToJSON(value?: PaginatedListOfAmmoDto | null): any {
    if (value == null) {
        return value;
    }
    return {
        
        'items': ((value['items'] as Array<any>).map(AmmoDtoToJSON)),
        'pageNumber': value['pageNumber'],
        'totalPages': value['totalPages'],
        'totalCount': value['totalCount'],
        'hasPreviousPage': value['hasPreviousPage'],
        'hasNextPage': value['hasNextPage'],
    };
}

