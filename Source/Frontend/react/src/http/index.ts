export interface Page<T = any> {
  page: number
  pageCount: number
  dataCount: number
  pageSize: number
  data: T[]
}
