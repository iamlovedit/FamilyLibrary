import { GlobalVariables } from '@/utils'

export type HttpMessage<T> = {
  response: T
  succeed: boolean
  code: number
  message: string
}

export type RequestOptions = {
  method?: string
  url: string
  body?: any
  requireToken?: boolean
}

export class HttpError extends Error {
  code: number

  constructor(message: string, code: number) {
    super(message)
    this.name = 'HttpError'
    this.code = code
  }
}

export class HttpClient {
  private requestInterceptors: Array<(url: string, options: RequestInit) => void> = []
  private responseInterceptors: Array<(response: Response) => void> = []
  private baseUrl: string
  constructor(url?: string) {
    this.baseUrl = url ?? import.meta.env.VITE_APP_API_BASE_URL
  }

  get = <T>(url: string, requireToken: boolean = true): Promise<HttpMessage<T>> => {
    return this.request({
      url,
      requireToken
    })
  }

  post = <T>(url: string, body?: any, requireToken: boolean = true): Promise<HttpMessage<T>> => {
    return this.request({
      url,
      method: 'POST',
      body,
      requireToken
    })
  }

  put = <T>(url: string, body?: any, requireToken: boolean = true): Promise<HttpMessage<T>> => {
    return this.request({
      url,
      method: 'PUT',
      body,
      requireToken
    })
  }

  delete = <T>(url: string, body?: any, requireToken: boolean = true): Promise<HttpMessage<T>> => {
    return this.request({
      url,
      body,
      method: 'DELETE',
      requireToken
    })
  }

  addResponseInterceptor(interceptor: (response: Response) => void) {
    this.responseInterceptors.push(interceptor)
  }

  addRequestInterceptor(interceptor: (url: string, options: RequestInit) => void) {
    this.requestInterceptors.push(interceptor)
  }

  private async request<T>(requestOptions: RequestOptions): Promise<HttpMessage<T>> {
    try {
      const headers: HeadersInit = {
        'Content-Type': 'application/json'
      }
      if (requestOptions.requireToken) {
        const token = localStorage.getItem(GlobalVariables.tokenName)
        if (token) {
          headers['Authorization'] = `Bearer ${token}`
        }
      }
      const url = this.baseUrl + requestOptions.url
      const options: RequestInit = {
        method: requestOptions.method || 'GET',
        headers,
        body: requestOptions.body ? JSON.stringify(requestOptions.body) : undefined
      }

      this.executeRequestInterceptors(url, options)
      const response: Response = await fetch(url, options)
      this.executeResponseInterceptors(response)
      if (response.ok) {
        const data: HttpMessage<T> = await response.json()
        if (data.succeed) {
          return data
        }
        throw new HttpError(data.message, data.code)
      }
      throw new HttpError(response.statusText, response.status)
    } catch (error: any) {
      if (error instanceof HttpError) {
        throw new HttpError(error.message, error.code)
      }
      if (error instanceof TypeError) {
        throw new HttpError('网络请求错误', 500)
      }
      throw new HttpError(error.message, 500)
    }
  }

  private executeRequestInterceptors(url: string, options: RequestInit) {
    this.requestInterceptors.forEach((interceptor) => interceptor(url, options))
  }
  private executeResponseInterceptors(response: Response) {
    this.responseInterceptors.forEach((interceptor) => interceptor(response))
  }
}
