import instance, { type HttpResponse } from './helper'
import { type AxiosRequestConfig } from 'axios'

const request = async <T = any>(config: AxiosRequestConfig): Promise<HttpResponse<T>> => {
  try {
    const { data } = await instance.request<HttpResponse<T>>(config)
    return data
  } catch (error: any) {
    const message = error.message || '请求失败'
    return {
      succeed: false,
      statusCode: error.response?.status || 500,
      message,
      response: null as any
    }
  }
}

export default request
