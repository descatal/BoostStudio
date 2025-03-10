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
 * @interface CreateProjectileCommand
 */
export interface CreateProjectileCommand {
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    projectileType?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    hitboxHash?: number | null;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    modelHash?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    skeletonIndex?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    aimType?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    translateY?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    translateZ?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    translateX?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    rotateX?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    rotateZ?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    cosmeticHash?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    unk44?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    unk48?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    unk52?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    unk56?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    ammoConsumption?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    durationFrame?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    maxTravelDistance?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    initialSpeed?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    acceleration?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    accelerationStartFrame?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    unk84?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    maxSpeed?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    reserved92?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    reserved96?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    reserved100?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    reserved104?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    reserved108?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    reserved112?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    reserved116?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    horizontalGuidance?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    horizontalGuidanceAngle?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    verticalGuidance?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    verticalGuidanceAngle?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    reserved136?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    reserved140?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    reserved144?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    reserved148?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    reserved152?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    reserved156?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    reserved160?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    reserved164?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    reserved168?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    reserved172?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    size?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    reserved180?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    reserved184?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    soundEffectHash?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    reserved192?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    reserved196?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    chainedProjectileHash?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    reserved204?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    reserved208?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    reserved212?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    reserved216?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    reserved220?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    reserved224?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    reserved228?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    reserved232?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    reserved236?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    reserved240?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    reserved244?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    reserved248?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    reserved252?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    reserved256?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    reserved260?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    reserved264?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    reserved268?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    reserved272?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    reserved276?: number;
    /**
     * 
     * @type {number}
     * @memberof CreateProjectileCommand
     */
    unitId?: number | null;
}

/**
 * Check if a given object implements the CreateProjectileCommand interface.
 */
export function instanceOfCreateProjectileCommand(value: object): value is CreateProjectileCommand {
    return true;
}

export function CreateProjectileCommandFromJSON(json: any): CreateProjectileCommand {
    return CreateProjectileCommandFromJSONTyped(json, false);
}

export function CreateProjectileCommandFromJSONTyped(json: any, ignoreDiscriminator: boolean): CreateProjectileCommand {
    if (json == null) {
        return json;
    }
    return {
        
        'projectileType': json['projectileType'] == null ? undefined : json['projectileType'],
        'hitboxHash': json['hitboxHash'] == null ? undefined : json['hitboxHash'],
        'modelHash': json['modelHash'] == null ? undefined : json['modelHash'],
        'skeletonIndex': json['skeletonIndex'] == null ? undefined : json['skeletonIndex'],
        'aimType': json['aimType'] == null ? undefined : json['aimType'],
        'translateY': json['translateY'] == null ? undefined : json['translateY'],
        'translateZ': json['translateZ'] == null ? undefined : json['translateZ'],
        'translateX': json['translateX'] == null ? undefined : json['translateX'],
        'rotateX': json['rotateX'] == null ? undefined : json['rotateX'],
        'rotateZ': json['rotateZ'] == null ? undefined : json['rotateZ'],
        'cosmeticHash': json['cosmeticHash'] == null ? undefined : json['cosmeticHash'],
        'unk44': json['unk44'] == null ? undefined : json['unk44'],
        'unk48': json['unk48'] == null ? undefined : json['unk48'],
        'unk52': json['unk52'] == null ? undefined : json['unk52'],
        'unk56': json['unk56'] == null ? undefined : json['unk56'],
        'ammoConsumption': json['ammoConsumption'] == null ? undefined : json['ammoConsumption'],
        'durationFrame': json['durationFrame'] == null ? undefined : json['durationFrame'],
        'maxTravelDistance': json['maxTravelDistance'] == null ? undefined : json['maxTravelDistance'],
        'initialSpeed': json['initialSpeed'] == null ? undefined : json['initialSpeed'],
        'acceleration': json['acceleration'] == null ? undefined : json['acceleration'],
        'accelerationStartFrame': json['accelerationStartFrame'] == null ? undefined : json['accelerationStartFrame'],
        'unk84': json['unk84'] == null ? undefined : json['unk84'],
        'maxSpeed': json['maxSpeed'] == null ? undefined : json['maxSpeed'],
        'reserved92': json['reserved92'] == null ? undefined : json['reserved92'],
        'reserved96': json['reserved96'] == null ? undefined : json['reserved96'],
        'reserved100': json['reserved100'] == null ? undefined : json['reserved100'],
        'reserved104': json['reserved104'] == null ? undefined : json['reserved104'],
        'reserved108': json['reserved108'] == null ? undefined : json['reserved108'],
        'reserved112': json['reserved112'] == null ? undefined : json['reserved112'],
        'reserved116': json['reserved116'] == null ? undefined : json['reserved116'],
        'horizontalGuidance': json['horizontalGuidance'] == null ? undefined : json['horizontalGuidance'],
        'horizontalGuidanceAngle': json['horizontalGuidanceAngle'] == null ? undefined : json['horizontalGuidanceAngle'],
        'verticalGuidance': json['verticalGuidance'] == null ? undefined : json['verticalGuidance'],
        'verticalGuidanceAngle': json['verticalGuidanceAngle'] == null ? undefined : json['verticalGuidanceAngle'],
        'reserved136': json['reserved136'] == null ? undefined : json['reserved136'],
        'reserved140': json['reserved140'] == null ? undefined : json['reserved140'],
        'reserved144': json['reserved144'] == null ? undefined : json['reserved144'],
        'reserved148': json['reserved148'] == null ? undefined : json['reserved148'],
        'reserved152': json['reserved152'] == null ? undefined : json['reserved152'],
        'reserved156': json['reserved156'] == null ? undefined : json['reserved156'],
        'reserved160': json['reserved160'] == null ? undefined : json['reserved160'],
        'reserved164': json['reserved164'] == null ? undefined : json['reserved164'],
        'reserved168': json['reserved168'] == null ? undefined : json['reserved168'],
        'reserved172': json['reserved172'] == null ? undefined : json['reserved172'],
        'size': json['size'] == null ? undefined : json['size'],
        'reserved180': json['reserved180'] == null ? undefined : json['reserved180'],
        'reserved184': json['reserved184'] == null ? undefined : json['reserved184'],
        'soundEffectHash': json['soundEffectHash'] == null ? undefined : json['soundEffectHash'],
        'reserved192': json['reserved192'] == null ? undefined : json['reserved192'],
        'reserved196': json['reserved196'] == null ? undefined : json['reserved196'],
        'chainedProjectileHash': json['chainedProjectileHash'] == null ? undefined : json['chainedProjectileHash'],
        'reserved204': json['reserved204'] == null ? undefined : json['reserved204'],
        'reserved208': json['reserved208'] == null ? undefined : json['reserved208'],
        'reserved212': json['reserved212'] == null ? undefined : json['reserved212'],
        'reserved216': json['reserved216'] == null ? undefined : json['reserved216'],
        'reserved220': json['reserved220'] == null ? undefined : json['reserved220'],
        'reserved224': json['reserved224'] == null ? undefined : json['reserved224'],
        'reserved228': json['reserved228'] == null ? undefined : json['reserved228'],
        'reserved232': json['reserved232'] == null ? undefined : json['reserved232'],
        'reserved236': json['reserved236'] == null ? undefined : json['reserved236'],
        'reserved240': json['reserved240'] == null ? undefined : json['reserved240'],
        'reserved244': json['reserved244'] == null ? undefined : json['reserved244'],
        'reserved248': json['reserved248'] == null ? undefined : json['reserved248'],
        'reserved252': json['reserved252'] == null ? undefined : json['reserved252'],
        'reserved256': json['reserved256'] == null ? undefined : json['reserved256'],
        'reserved260': json['reserved260'] == null ? undefined : json['reserved260'],
        'reserved264': json['reserved264'] == null ? undefined : json['reserved264'],
        'reserved268': json['reserved268'] == null ? undefined : json['reserved268'],
        'reserved272': json['reserved272'] == null ? undefined : json['reserved272'],
        'reserved276': json['reserved276'] == null ? undefined : json['reserved276'],
        'unitId': json['unitId'] == null ? undefined : json['unitId'],
    };
}

export function CreateProjectileCommandToJSON(value?: CreateProjectileCommand | null): any {
    if (value == null) {
        return value;
    }
    return {
        
        'projectileType': value['projectileType'],
        'hitboxHash': value['hitboxHash'],
        'modelHash': value['modelHash'],
        'skeletonIndex': value['skeletonIndex'],
        'aimType': value['aimType'],
        'translateY': value['translateY'],
        'translateZ': value['translateZ'],
        'translateX': value['translateX'],
        'rotateX': value['rotateX'],
        'rotateZ': value['rotateZ'],
        'cosmeticHash': value['cosmeticHash'],
        'unk44': value['unk44'],
        'unk48': value['unk48'],
        'unk52': value['unk52'],
        'unk56': value['unk56'],
        'ammoConsumption': value['ammoConsumption'],
        'durationFrame': value['durationFrame'],
        'maxTravelDistance': value['maxTravelDistance'],
        'initialSpeed': value['initialSpeed'],
        'acceleration': value['acceleration'],
        'accelerationStartFrame': value['accelerationStartFrame'],
        'unk84': value['unk84'],
        'maxSpeed': value['maxSpeed'],
        'reserved92': value['reserved92'],
        'reserved96': value['reserved96'],
        'reserved100': value['reserved100'],
        'reserved104': value['reserved104'],
        'reserved108': value['reserved108'],
        'reserved112': value['reserved112'],
        'reserved116': value['reserved116'],
        'horizontalGuidance': value['horizontalGuidance'],
        'horizontalGuidanceAngle': value['horizontalGuidanceAngle'],
        'verticalGuidance': value['verticalGuidance'],
        'verticalGuidanceAngle': value['verticalGuidanceAngle'],
        'reserved136': value['reserved136'],
        'reserved140': value['reserved140'],
        'reserved144': value['reserved144'],
        'reserved148': value['reserved148'],
        'reserved152': value['reserved152'],
        'reserved156': value['reserved156'],
        'reserved160': value['reserved160'],
        'reserved164': value['reserved164'],
        'reserved168': value['reserved168'],
        'reserved172': value['reserved172'],
        'size': value['size'],
        'reserved180': value['reserved180'],
        'reserved184': value['reserved184'],
        'soundEffectHash': value['soundEffectHash'],
        'reserved192': value['reserved192'],
        'reserved196': value['reserved196'],
        'chainedProjectileHash': value['chainedProjectileHash'],
        'reserved204': value['reserved204'],
        'reserved208': value['reserved208'],
        'reserved212': value['reserved212'],
        'reserved216': value['reserved216'],
        'reserved220': value['reserved220'],
        'reserved224': value['reserved224'],
        'reserved228': value['reserved228'],
        'reserved232': value['reserved232'],
        'reserved236': value['reserved236'],
        'reserved240': value['reserved240'],
        'reserved244': value['reserved244'],
        'reserved248': value['reserved248'],
        'reserved252': value['reserved252'],
        'reserved256': value['reserved256'],
        'reserved260': value['reserved260'],
        'reserved264': value['reserved264'],
        'reserved268': value['reserved268'],
        'reserved272': value['reserved272'],
        'reserved276': value['reserved276'],
        'unitId': value['unitId'],
    };
}

