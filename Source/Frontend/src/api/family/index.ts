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

export interface FamilyCategoryBasic {
  id: string
  name: string
}

export interface FamilyCategory extends FamilyCategoryBasic {
  parentId: string
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
  pageSize: number = 30
): Promise<HttpResponse<Page<FamilyBasic>>> {
  const params = {
    ...(keyword && { keyword }),
    ...(categoryId && { categoryId }),
    pageIndex,
    pageSize
  }
  return request({
    url: 'family/v1/all',
    params,
    method: RequestEnum.GET
  })
}
