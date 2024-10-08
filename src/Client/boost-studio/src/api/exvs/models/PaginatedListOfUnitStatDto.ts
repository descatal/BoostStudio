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
import type { UnitStatDto } from './UnitStatDto';
import {
    UnitStatDtoFromJSON,
    UnitStatDtoFromJSONTyped,
    UnitStatDtoToJSON,
} from './UnitStatDto';

/**
 * 
 * @export
 * @interface PaginatedListOfUnitStatDto
 */
export interface PaginatedListOfUnitStatDto {
    /**
     * 
     * @type {Array<UnitStatDto>}
     * @memberof PaginatedListOfUnitStatDto
     */
    items: Array<UnitStatDto>;
    /**
     * 
     * @type {number}
     * @memberof PaginatedListOfUnitStatDto
     */
    pageNumber: number;
    /**
     * 
     * @type {number}
     * @memberof PaginatedListOfUnitStatDto
     */
    totalPages?: number;
    /**
     * 
     * @type {number}
     * @memberof PaginatedListOfUnitStatDto
     */
    totalCount: number;
    /**
     * 
     * @type {boolean}
     * @memberof PaginatedListOfUnitStatDto
     */
    hasPreviousPage?: boolean;
    /**
     * 
     * @type {boolean}
     * @memberof PaginatedListOfUnitStatDto
     */
    hasNextPage?: boolean;
}

/**
 * Check if a given object implements the PaginatedListOfUnitStatDto interface.
 */
export function instanceOfPaginatedListOfUnitStatDto(value: object): value is PaginatedListOfUnitStatDto {
    if (!('items' in value) || value['items'] === undefined) return false;
    if (!('pageNumber' in value) || value['pageNumber'] === undefined) return false;
    if (!('totalCount' in value) || value['totalCount'] === undefined) return false;
    return true;
}

export function PaginatedListOfUnitStatDtoFromJSON(json: any): PaginatedListOfUnitStatDto {
    return PaginatedListOfUnitStatDtoFromJSONTyped(json, false);
}

export function PaginatedListOfUnitStatDtoFromJSONTyped(json: any, ignoreDiscriminator: boolean): PaginatedListOfUnitStatDto {
    if (json == null) {
        return json;
    }
    return {
        
        'items': ((json['items'] as Array<any>).map(UnitStatDtoFromJSON)),
        'pageNumber': json['pageNumber'],
        'totalPages': json['totalPages'] == null ? undefined : json['totalPages'],
        'totalCount': json['totalCount'],
        'hasPreviousPage': json['hasPreviousPage'] == null ? undefined : json['hasPreviousPage'],
        'hasNextPage': json['hasNextPage'] == null ? undefined : json['hasNextPage'],
    };
}

export function PaginatedListOfUnitStatDtoToJSON(value?: PaginatedListOfUnitStatDto | null): any {
    if (value == null) {
        return value;
    }
    return {
        
        'items': ((value['items'] as Array<any>).map(UnitStatDtoToJSON)),
        'pageNumber': value['pageNumber'],
        'totalPages': value['totalPages'],
        'totalCount': value['totalCount'],
        'hasPreviousPage': value['hasPreviousPage'],
        'hasNextPage': value['hasNextPage'],
    };
}

