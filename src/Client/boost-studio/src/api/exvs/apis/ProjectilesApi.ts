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
  GetApiProjectiles200Response,
  GetApiProjectilesByHash200Response,
  PostApiProjectilesByHashRequest,
  PostApiProjectilesRequest,
} from '../models/index';
import {
    GetApiProjectiles200ResponseFromJSON,
    GetApiProjectiles200ResponseToJSON,
    GetApiProjectilesByHash200ResponseFromJSON,
    GetApiProjectilesByHash200ResponseToJSON,
    PostApiProjectilesByHashRequestFromJSON,
    PostApiProjectilesByHashRequestToJSON,
    PostApiProjectilesRequestFromJSON,
    PostApiProjectilesRequestToJSON,
} from '../models/index';

export interface DeleteApiProjectilesByHashRequest {
    hash: number;
}

export interface GetApiProjectilesRequest {
    page?: number;
    perPage?: number;
    hashes?: Array<number> | null;
    unitIds?: Array<number> | null;
    search?: string | null;
}

export interface GetApiProjectilesByHashRequest {
    hash: number;
}

export interface PostApiProjectilesOperationRequest {
    postApiProjectilesRequest: PostApiProjectilesRequest;
}

export interface PostApiProjectilesByHashOperationRequest {
    hash: number;
    postApiProjectilesByHashRequest: PostApiProjectilesByHashRequest;
}

/**
 * 
 */
export class ProjectilesApi extends runtime.BaseAPI {

    /**
     */
    async deleteApiProjectilesByHashRaw(requestParameters: DeleteApiProjectilesByHashRequest, initOverrides?: RequestInit | runtime.InitOverrideFunction): Promise<runtime.ApiResponse<void>> {
        if (requestParameters['hash'] == null) {
            throw new runtime.RequiredError(
                'hash',
                'Required parameter "hash" was null or undefined when calling deleteApiProjectilesByHash().'
            );
        }

        const queryParameters: any = {};

        const headerParameters: runtime.HTTPHeaders = {};

        const response = await this.request({
            path: `/api/projectiles/{hash}`.replace(`{${"hash"}}`, encodeURIComponent(String(requestParameters['hash']))),
            method: 'DELETE',
            headers: headerParameters,
            query: queryParameters,
        }, initOverrides);

        return new runtime.VoidApiResponse(response);
    }

    /**
     */
    async deleteApiProjectilesByHash(requestParameters: DeleteApiProjectilesByHashRequest, initOverrides?: RequestInit | runtime.InitOverrideFunction): Promise<void> {
        await this.deleteApiProjectilesByHashRaw(requestParameters, initOverrides);
    }

    /**
     */
    async getApiProjectilesRaw(requestParameters: GetApiProjectilesRequest, initOverrides?: RequestInit | runtime.InitOverrideFunction): Promise<runtime.ApiResponse<GetApiProjectiles200Response>> {
        const queryParameters: any = {};

        if (requestParameters['page'] != null) {
            queryParameters['Page'] = requestParameters['page'];
        }

        if (requestParameters['perPage'] != null) {
            queryParameters['PerPage'] = requestParameters['perPage'];
        }

        if (requestParameters['hashes'] != null) {
            queryParameters['Hashes'] = requestParameters['hashes'];
        }

        if (requestParameters['unitIds'] != null) {
            queryParameters['UnitIds'] = requestParameters['unitIds'];
        }

        if (requestParameters['search'] != null) {
            queryParameters['Search'] = requestParameters['search'];
        }

        const headerParameters: runtime.HTTPHeaders = {};

        const response = await this.request({
            path: `/api/projectiles`,
            method: 'GET',
            headers: headerParameters,
            query: queryParameters,
        }, initOverrides);

        return new runtime.JSONApiResponse(response, (jsonValue) => GetApiProjectiles200ResponseFromJSON(jsonValue));
    }

    /**
     */
    async getApiProjectiles(requestParameters: GetApiProjectilesRequest = {}, initOverrides?: RequestInit | runtime.InitOverrideFunction): Promise<GetApiProjectiles200Response> {
        const response = await this.getApiProjectilesRaw(requestParameters, initOverrides);
        return await response.value();
    }

    /**
     */
    async getApiProjectilesByHashRaw(requestParameters: GetApiProjectilesByHashRequest, initOverrides?: RequestInit | runtime.InitOverrideFunction): Promise<runtime.ApiResponse<GetApiProjectilesByHash200Response>> {
        if (requestParameters['hash'] == null) {
            throw new runtime.RequiredError(
                'hash',
                'Required parameter "hash" was null or undefined when calling getApiProjectilesByHash().'
            );
        }

        const queryParameters: any = {};

        const headerParameters: runtime.HTTPHeaders = {};

        const response = await this.request({
            path: `/api/projectiles/{hash}`.replace(`{${"hash"}}`, encodeURIComponent(String(requestParameters['hash']))),
            method: 'GET',
            headers: headerParameters,
            query: queryParameters,
        }, initOverrides);

        return new runtime.JSONApiResponse(response, (jsonValue) => GetApiProjectilesByHash200ResponseFromJSON(jsonValue));
    }

    /**
     */
    async getApiProjectilesByHash(requestParameters: GetApiProjectilesByHashRequest, initOverrides?: RequestInit | runtime.InitOverrideFunction): Promise<GetApiProjectilesByHash200Response> {
        const response = await this.getApiProjectilesByHashRaw(requestParameters, initOverrides);
        return await response.value();
    }

    /**
     */
    async postApiProjectilesRaw(requestParameters: PostApiProjectilesOperationRequest, initOverrides?: RequestInit | runtime.InitOverrideFunction): Promise<runtime.ApiResponse<void>> {
        if (requestParameters['postApiProjectilesRequest'] == null) {
            throw new runtime.RequiredError(
                'postApiProjectilesRequest',
                'Required parameter "postApiProjectilesRequest" was null or undefined when calling postApiProjectiles().'
            );
        }

        const queryParameters: any = {};

        const headerParameters: runtime.HTTPHeaders = {};

        headerParameters['Content-Type'] = 'application/json';

        const response = await this.request({
            path: `/api/projectiles`,
            method: 'POST',
            headers: headerParameters,
            query: queryParameters,
            body: PostApiProjectilesRequestToJSON(requestParameters['postApiProjectilesRequest']),
        }, initOverrides);

        return new runtime.VoidApiResponse(response);
    }

    /**
     */
    async postApiProjectiles(requestParameters: PostApiProjectilesOperationRequest, initOverrides?: RequestInit | runtime.InitOverrideFunction): Promise<void> {
        await this.postApiProjectilesRaw(requestParameters, initOverrides);
    }

    /**
     */
    async postApiProjectilesByHashRaw(requestParameters: PostApiProjectilesByHashOperationRequest, initOverrides?: RequestInit | runtime.InitOverrideFunction): Promise<runtime.ApiResponse<void>> {
        if (requestParameters['hash'] == null) {
            throw new runtime.RequiredError(
                'hash',
                'Required parameter "hash" was null or undefined when calling postApiProjectilesByHash().'
            );
        }

        if (requestParameters['postApiProjectilesByHashRequest'] == null) {
            throw new runtime.RequiredError(
                'postApiProjectilesByHashRequest',
                'Required parameter "postApiProjectilesByHashRequest" was null or undefined when calling postApiProjectilesByHash().'
            );
        }

        const queryParameters: any = {};

        const headerParameters: runtime.HTTPHeaders = {};

        headerParameters['Content-Type'] = 'application/json';

        const response = await this.request({
            path: `/api/projectiles/{hash}`.replace(`{${"hash"}}`, encodeURIComponent(String(requestParameters['hash']))),
            method: 'POST',
            headers: headerParameters,
            query: queryParameters,
            body: PostApiProjectilesByHashRequestToJSON(requestParameters['postApiProjectilesByHashRequest']),
        }, initOverrides);

        return new runtime.VoidApiResponse(response);
    }

    /**
     */
    async postApiProjectilesByHash(requestParameters: PostApiProjectilesByHashOperationRequest, initOverrides?: RequestInit | runtime.InitOverrideFunction): Promise<void> {
        await this.postApiProjectilesByHashRaw(requestParameters, initOverrides);
    }

}
