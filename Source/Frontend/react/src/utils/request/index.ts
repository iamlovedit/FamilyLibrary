import { AxiosRequestConfig } from 'axios'
import useAxios, { HttpResponse } from './helper'

const { instance } = useAxios()

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
