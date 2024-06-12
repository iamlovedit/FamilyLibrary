import { RequestEnum } from '@/enums/httpEnum'
import type { LoginDTO } from '@/stores/modules/auth/helper'
import type { User } from '@/stores/modules/user/helper'
import request from '@/utils/request'
import type { HttpResponse, Page } from '@/utils/request/helper'

export interface TokenInfo {
  token: string
  expiresIn: number,
  tokenType: string
}

export interface RoleData {
  id: string,
  name: string,
  description: string
}

export interface UserCreationDTO {
  name: string,
  username: string,
  password: string,
  email: string,
  roleId: string
}

export function login(user: LoginDTO): Promise<HttpResponse<TokenInfo>> {
  return request<TokenInfo>({
    url: '/identity/v1/auth/login',
    method: RequestEnum.POST,
    data: user
  })
}

export function getCurrentUser(): Promise<HttpResponse<User>> {
  return request<User>({
    url: '/identity/v1/user/me',
    method: RequestEnum.GET
  })
}

export function queryUsers(keyword?: string, pageIndex: number = 1, pageSize: number = 10): Promise<HttpResponse<Page<User>>> {
  const params = keyword ? {
    keyword,
    pageIndex,
    pageSize
  }
    : {
      pageIndex,
      pageSize
    }
  return request<Page<User>>({
    url: '/identity/v1/user/query',
    method: RequestEnum.GET,
    params
  })
}

export function deleteUser(userId: string): Promise<HttpResponse<boolean>> {
  return request<boolean>({
    url: '/identity/v1/user/' + userId,
    method: RequestEnum.DELETE
  })
}

export function getAllRoles(): Promise<HttpResponse<RoleData[]>> {
  return request<RoleData[]>({
    url: '/identity/v1/role/all'
  })
}

export function registerUser(userInfo: UserCreationDTO): Promise<HttpResponse<boolean>> {
  return request<boolean>({
    url: '/identity/v1/user',
    method: RequestEnum.POST,
    data: userInfo
  })
}

export function updatePassword(data: any): Promise<HttpResponse<boolean>> {
  return request<boolean>({
    url: '/identity/v1/user',
    method: RequestEnum.PUT,
    data
  })
}