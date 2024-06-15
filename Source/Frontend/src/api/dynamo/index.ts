import type { HttpResponse, Page } from '@/utils/request/helper'
import request from '@/utils/request'
import { RequestEnum } from '@/enums/httpEnum'


export interface PackageDTO {
    id: string
    name: string
    createdDate: string,
    updatedDate: string
    downloads: number,
    votes: number,
    description: string
}

export interface PackageVersionDTO {
    version: string
    createdDate: string,
}

export function getPackagePagesAsync(keyword?: string, pageIndex: number = 1, pageSize: number = 30, orderBy?: string): Promise<HttpResponse<Page<PackageDTO>>> {
    const params = {
        ...keyword && { keyword },
        pageIndex,
        pageSize,
        ...orderBy && { orderBy }
    }
    return request<Page<PackageDTO>>({
        url: 'package/v1/packages',
        method: RequestEnum.GET,
        params
    })
}

export function getPackageVersionPageAsync(id: string): Promise<HttpResponse<Page<PackageVersionDTO>>> {
    return request<Page<PackageVersionDTO>>({
        url: `package/v1/${id}`,
        method: RequestEnum.GET
    })
}