import { httpClient, HttpMessage } from '@/utils/request'
import { Page } from '@/http'

const { get } = httpClient
const controller = '/package'

export interface PackageBasic {
  id: string
  name: string
}
export function getPackagePage(
  page: number = 1,
  pageSize: number = 30
): Promise<HttpMessage<Page<PackageBasic>>> {
  return get<Page<PackageBasic>>(`${controller}/v2/all?page=${page}&pageSize=${pageSize}`, false)
}
