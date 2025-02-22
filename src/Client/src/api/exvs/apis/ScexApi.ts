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


import * as runtime from '../runtime';
import type {
  CompileScexByPathCommand,
  CompileScexByUnitsCommand,
  DecompileScexByPathCommand,
  DecompileScexByUnitsCommand,
  HotReloadScex,
} from '../models/index';
import {
    CompileScexByPathCommandFromJSON,
    CompileScexByPathCommandToJSON,
    CompileScexByUnitsCommandFromJSON,
    CompileScexByUnitsCommandToJSON,
    DecompileScexByPathCommandFromJSON,
    DecompileScexByPathCommandToJSON,
    DecompileScexByUnitsCommandFromJSON,
    DecompileScexByUnitsCommandToJSON,
    HotReloadScexFromJSON,
    HotReloadScexToJSON,
} from '../models/index';

export interface PostApiScexCompilePathRequest {
    compileScexByPathCommand: CompileScexByPathCommand;
}

export interface PostApiScexCompileUnitsRequest {
    compileScexByUnitsCommand: CompileScexByUnitsCommand;
}

export interface PostApiScexDecompilePathRequest {
    decompileScexByPathCommand: DecompileScexByPathCommand;
}

export interface PostApiScexDecompileUnitsRequest {
    decompileScexByUnitsCommand: DecompileScexByUnitsCommand;
}

export interface PostApiScexHotReloadPathRequest {
    hotReloadScex: HotReloadScex;
}

/**
 * 
 */
export class ScexApi extends runtime.BaseAPI {

    /**
     */
    async postApiScexCompilePathRaw(requestParameters: PostApiScexCompilePathRequest, initOverrides?: RequestInit | runtime.InitOverrideFunction): Promise<runtime.ApiResponse<void>> {
        if (requestParameters['compileScexByPathCommand'] == null) {
            throw new runtime.RequiredError(
                'compileScexByPathCommand',
                'Required parameter "compileScexByPathCommand" was null or undefined when calling postApiScexCompilePath().'
            );
        }

        const queryParameters: any = {};

        const headerParameters: runtime.HTTPHeaders = {};

        headerParameters['Content-Type'] = 'application/json';

        const response = await this.request({
            path: `/api/scex/compile/path`,
            method: 'POST',
            headers: headerParameters,
            query: queryParameters,
            body: CompileScexByPathCommandToJSON(requestParameters['compileScexByPathCommand']),
        }, initOverrides);

        return new runtime.VoidApiResponse(response);
    }

    /**
     */
    async postApiScexCompilePath(requestParameters: PostApiScexCompilePathRequest, initOverrides?: RequestInit | runtime.InitOverrideFunction): Promise<void> {
        await this.postApiScexCompilePathRaw(requestParameters, initOverrides);
    }

    /**
     */
    async postApiScexCompileUnitsRaw(requestParameters: PostApiScexCompileUnitsRequest, initOverrides?: RequestInit | runtime.InitOverrideFunction): Promise<runtime.ApiResponse<void>> {
        if (requestParameters['compileScexByUnitsCommand'] == null) {
            throw new runtime.RequiredError(
                'compileScexByUnitsCommand',
                'Required parameter "compileScexByUnitsCommand" was null or undefined when calling postApiScexCompileUnits().'
            );
        }

        const queryParameters: any = {};

        const headerParameters: runtime.HTTPHeaders = {};

        headerParameters['Content-Type'] = 'application/json';

        const response = await this.request({
            path: `/api/scex/compile/units`,
            method: 'POST',
            headers: headerParameters,
            query: queryParameters,
            body: CompileScexByUnitsCommandToJSON(requestParameters['compileScexByUnitsCommand']),
        }, initOverrides);

        return new runtime.VoidApiResponse(response);
    }

    /**
     */
    async postApiScexCompileUnits(requestParameters: PostApiScexCompileUnitsRequest, initOverrides?: RequestInit | runtime.InitOverrideFunction): Promise<void> {
        await this.postApiScexCompileUnitsRaw(requestParameters, initOverrides);
    }

    /**
     */
    async postApiScexDecompilePathRaw(requestParameters: PostApiScexDecompilePathRequest, initOverrides?: RequestInit | runtime.InitOverrideFunction): Promise<runtime.ApiResponse<void>> {
        if (requestParameters['decompileScexByPathCommand'] == null) {
            throw new runtime.RequiredError(
                'decompileScexByPathCommand',
                'Required parameter "decompileScexByPathCommand" was null or undefined when calling postApiScexDecompilePath().'
            );
        }

        const queryParameters: any = {};

        const headerParameters: runtime.HTTPHeaders = {};

        headerParameters['Content-Type'] = 'application/json';

        const response = await this.request({
            path: `/api/scex/decompile/path`,
            method: 'POST',
            headers: headerParameters,
            query: queryParameters,
            body: DecompileScexByPathCommandToJSON(requestParameters['decompileScexByPathCommand']),
        }, initOverrides);

        return new runtime.VoidApiResponse(response);
    }

    /**
     */
    async postApiScexDecompilePath(requestParameters: PostApiScexDecompilePathRequest, initOverrides?: RequestInit | runtime.InitOverrideFunction): Promise<void> {
        await this.postApiScexDecompilePathRaw(requestParameters, initOverrides);
    }

    /**
     */
    async postApiScexDecompileUnitsRaw(requestParameters: PostApiScexDecompileUnitsRequest, initOverrides?: RequestInit | runtime.InitOverrideFunction): Promise<runtime.ApiResponse<void>> {
        if (requestParameters['decompileScexByUnitsCommand'] == null) {
            throw new runtime.RequiredError(
                'decompileScexByUnitsCommand',
                'Required parameter "decompileScexByUnitsCommand" was null or undefined when calling postApiScexDecompileUnits().'
            );
        }

        const queryParameters: any = {};

        const headerParameters: runtime.HTTPHeaders = {};

        headerParameters['Content-Type'] = 'application/json';

        const response = await this.request({
            path: `/api/scex/decompile/units`,
            method: 'POST',
            headers: headerParameters,
            query: queryParameters,
            body: DecompileScexByUnitsCommandToJSON(requestParameters['decompileScexByUnitsCommand']),
        }, initOverrides);

        return new runtime.VoidApiResponse(response);
    }

    /**
     */
    async postApiScexDecompileUnits(requestParameters: PostApiScexDecompileUnitsRequest, initOverrides?: RequestInit | runtime.InitOverrideFunction): Promise<void> {
        await this.postApiScexDecompileUnitsRaw(requestParameters, initOverrides);
    }

    /**
     */
    async postApiScexHotReloadPathRaw(requestParameters: PostApiScexHotReloadPathRequest, initOverrides?: RequestInit | runtime.InitOverrideFunction): Promise<runtime.ApiResponse<void>> {
        if (requestParameters['hotReloadScex'] == null) {
            throw new runtime.RequiredError(
                'hotReloadScex',
                'Required parameter "hotReloadScex" was null or undefined when calling postApiScexHotReloadPath().'
            );
        }

        const queryParameters: any = {};

        const headerParameters: runtime.HTTPHeaders = {};

        headerParameters['Content-Type'] = 'application/json';

        const response = await this.request({
            path: `/api/scex/hot-reload/path`,
            method: 'POST',
            headers: headerParameters,
            query: queryParameters,
            body: HotReloadScexToJSON(requestParameters['hotReloadScex']),
        }, initOverrides);

        return new runtime.VoidApiResponse(response);
    }

    /**
     */
    async postApiScexHotReloadPath(requestParameters: PostApiScexHotReloadPathRequest, initOverrides?: RequestInit | runtime.InitOverrideFunction): Promise<void> {
        await this.postApiScexHotReloadPathRaw(requestParameters, initOverrides);
    }

}
