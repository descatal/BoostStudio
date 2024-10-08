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
 * @interface CreateUnitAmmoSlotCommand
 */
export interface CreateUnitAmmoSlotCommand {
    /**
     * 
     * @type {number}
     * @memberof CreateUnitAmmoSlotCommand
     */
    ammoHash: number;
    /**
     * 
     * @type {number}
     * @memberof CreateUnitAmmoSlotCommand
     */
    unitId: number;
    /**
     * 
     * @type {number}
     * @memberof CreateUnitAmmoSlotCommand
     */
    slotOrder: number;
}

/**
 * Check if a given object implements the CreateUnitAmmoSlotCommand interface.
 */
export function instanceOfCreateUnitAmmoSlotCommand(value: object): value is CreateUnitAmmoSlotCommand {
    if (!('ammoHash' in value) || value['ammoHash'] === undefined) return false;
    if (!('unitId' in value) || value['unitId'] === undefined) return false;
    if (!('slotOrder' in value) || value['slotOrder'] === undefined) return false;
    return true;
}

export function CreateUnitAmmoSlotCommandFromJSON(json: any): CreateUnitAmmoSlotCommand {
    return CreateUnitAmmoSlotCommandFromJSONTyped(json, false);
}

export function CreateUnitAmmoSlotCommandFromJSONTyped(json: any, ignoreDiscriminator: boolean): CreateUnitAmmoSlotCommand {
    if (json == null) {
        return json;
    }
    return {
        
        'ammoHash': json['ammoHash'],
        'unitId': json['unitId'],
        'slotOrder': json['slotOrder'],
    };
}

export function CreateUnitAmmoSlotCommandToJSON(value?: CreateUnitAmmoSlotCommand | null): any {
    if (value == null) {
        return value;
    }
    return {
        
        'ammoHash': value['ammoHash'],
        'unitId': value['unitId'],
        'slotOrder': value['slotOrder'],
    };
}

