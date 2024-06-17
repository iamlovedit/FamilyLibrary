import type { HttpResponse, Page } from '@/utils/request/helper'
import request from '@/utils/request'
import { RequestEnum } from '@/enums/httpEnum'

export interface FamilyBasic {
  id: string
  name: string
  catgegory: FamilyCategoryBasic
  stars: number
  imageUrl: string
  downloads: number
  createdDate: string
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

export function getFamilyCatgories(): Promise<HttpResponse<FamilyCategory[]>> {
  return request({
    url: 'family/v1/categories',
    method: RequestEnum.GET
  })
}
