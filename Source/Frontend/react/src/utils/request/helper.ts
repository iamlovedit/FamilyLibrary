export interface HttpMessage<T> {
  data: T
  succeed: boolean
  code: number
  message: string
}

export interface RequestOptions {
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
  private baseUrl: string
  constructor(url?: string) {
    this.baseUrl = url ?? import.meta.env.VITE_APP_API_BASE_URL
  }

  get<T>(url: string, requireToken: boolean = true): Promise<HttpMessage<T>> {
    return this.request({
      url,
      requireToken
    })
  }

  send<T>(url: string, body?: any, requireToken: boolean = true): Promise<HttpMessage<T>> {
    return this.request({
      url,
      method: 'POST',
      body,
      requireToken
    })
  }

  private async request<T>(requestOptions: RequestOptions): Promise<HttpMessage<T>> {
    try {
      const headers: HeadersInit = {
        'Content-Type': 'application/json'
      }
      if (requestOptions.requireToken) {
        const token = localStorage.getItem('auth_token')
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

      const response: Response = await fetch(url, options)
      if (response.ok) {
        const data: HttpMessage<T> = await response.json()
        return data
      }
      throw new Error(`网络请求错误! status: ${response.status}`)
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
}

export function useHttpClient(): HttpClient {
  return new HttpClient()
}
