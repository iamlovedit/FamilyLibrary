import { HttpClient, HttpError, type HttpMessage, type RequestOptions } from './helper'
import { GlobalVariables } from '@/utils'

const httpClient = new HttpClient()
httpClient.addResponseInterceptor((response) => {
  if (response.status === 401) {
    localStorage.removeItem(GlobalVariables.tokenName)
    window.location.href = window.location.origin + '/login'
    throw new HttpError('未验证', 401)
  }
})
export { HttpMessage, httpClient, RequestOptions }
