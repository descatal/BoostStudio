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
import type { PlayableSeriesDetailsDto } from './PlayableSeriesDetailsDto';
import {
    PlayableSeriesDetailsDtoFromJSON,
    PlayableSeriesDetailsDtoFromJSONTyped,
    PlayableSeriesDetailsDtoToJSON,
} from './PlayableSeriesDetailsDto';

/**
 * 
 * @export
 * @interface CreateSeriesCommand
 */
export interface CreateSeriesCommand {
    /**
     * 
     * @type {number}
     * @memberof CreateSeriesCommand
     */
    id?: number;
    /**
     * 
     * @type {PlayableSeriesDetailsDto}
     * @memberof CreateSeriesCommand
     */
    playableSeries?: PlayableSeriesDetailsDto | null;
    /**
     * 
     * @type {string}
     * @memberof CreateSeriesCommand
     */
    slugName?: string;
    /**
     * 
     * @type {string}
     * @memberof CreateSeriesCommand
     */
    nameEnglish?: string | null;
    /**
     * 
     * @type {string}
     * @memberof CreateSeriesCommand
     */
    nameJapanese?: string | null;
    /**
     * 
     * @type {string}
     * @memberof CreateSeriesCommand
     */
    nameChinese?: string | null;
}

/**
 * Check if a given object implements the CreateSeriesCommand interface.
 */
export function instanceOfCreateSeriesCommand(value: object): value is CreateSeriesCommand {
    return true;
}

export function CreateSeriesCommandFromJSON(json: any): CreateSeriesCommand {
    return CreateSeriesCommandFromJSONTyped(json, false);
}

export function CreateSeriesCommandFromJSONTyped(json: any, ignoreDiscriminator: boolean): CreateSeriesCommand {
    if (json == null) {
        return json;
    }
    return {
        
        'id': json['id'] == null ? undefined : json['id'],
        'playableSeries': json['playableSeries'] == null ? undefined : PlayableSeriesDetailsDtoFromJSON(json['playableSeries']),
        'slugName': json['slugName'] == null ? undefined : json['slugName'],
        'nameEnglish': json['nameEnglish'] == null ? undefined : json['nameEnglish'],
        'nameJapanese': json['nameJapanese'] == null ? undefined : json['nameJapanese'],
        'nameChinese': json['nameChinese'] == null ? undefined : json['nameChinese'],
    };
}

export function CreateSeriesCommandToJSON(value?: CreateSeriesCommand | null): any {
    if (value == null) {
        return value;
    }
    return {
        
        'id': value['id'],
        'playableSeries': PlayableSeriesDetailsDtoToJSON(value['playableSeries']),
        'slugName': value['slugName'],
        'nameEnglish': value['nameEnglish'],
        'nameJapanese': value['nameJapanese'],
        'nameChinese': value['nameChinese'],
    };
}

