import type { HttpResponse, Page } from '@/utils/request/helper'
import request from '@/utils/request'
import { RequestEnum } from '@/enums/httpEnum'
import type { User } from '@/stores/modules/user/helper'

export interface FamilyBasic {
  id: string
  name: string
  category: FamilyCategoryBasic
  stars: number
  imageUrl: string
  downloads: number
  createdDate: string
  uploader: User
}

export interface FamilySymbol {
  name: string
}

export interface FamilyDetail extends FamilyBasic {
  fileId: string
  versions: number[]
  symbols: FamilySymbol[]
}

export interface FamilyCategoryBasic {
  id: string
  name: string
}

export interface FamilyCategory extends FamilyCategoryBasic {
  parentId: string
  parent: FamilyCategory
  children: FamilyCategory[]
}

export function getFamilyCategories(): Promise<HttpResponse<FamilyCategory[]>> {
  return request({
    url: 'family/v1/categories',
    method: RequestEnum.GET
  })
}

export function getFamiliesPagePromise(
  keyword?: string,
  categoryId?: string,
  pageIndex: number = 1,
  pageSize: number = 30,
  orderBy?: string
): Promise<HttpResponse<Page<FamilyBasic>>> {
  const params = {
    ...(keyword && { keyword }),
    ...(categoryId && { categoryId }),
    pageIndex,
    pageSize,
    ...(orderBy !== 'default' && { orderBy })
  }
  return request({
    url: 'family/v1/all',
    params,
    method: RequestEnum.GET
  })
}

export function getFamilyDetailPromise(id: string): Promise<HttpResponse<FamilyDetail>> {
  return request({
    url: `family/v1/details/${id}`,
    method: RequestEnum.GET
  })
}
