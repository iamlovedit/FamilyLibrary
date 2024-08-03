import axios, { AxiosError, type AxiosResponse } from 'axios'
import { useNavigate } from 'react-router-dom'

export interface HttpResponse<T = any> {
  succeed: boolean
  statusCode: number
  message: string
  response: T
}

const useAxios = () => {
  const navigate = useNavigate()
  const instance = axios.create({
    baseURL: import.meta.env.VITE_APP_API_BASE_URL
  })

  instance.interceptors.request.use(
    (config) => {
      const json = window.localStorage.getItem('key_tokenInfo')
      if (json) {
        const tokenInfo = JSON.parse(json)
        if (tokenInfo) {
          config.headers.Authorization = `${tokenInfo.tokenType} ${tokenInfo.token}`
        }
      }
      return config
    },
    (error) => {
      return Promise.reject(error.response)
    }
  )

  instance.interceptors.response.use(
    (response: AxiosResponse<HttpResponse>): AxiosResponse<HttpResponse> => {
      if (response.status === 200) {
        if (response.data.succeed) {
          return response
        }
        throw new Error(response.data.message)
      }
      throw new Error(response.status.toString())
    },
    (error: AxiosError) => {
      if (error.response) {
        let message = ''
        switch (error.response.status) {
          case 400:
            message = '请求错误(400)'
            break
          case 401:
            message = '未授权，请重新登录(401)'
            navigate('./login')
            break
          case 403:
            message = '拒绝访问(403)'
            navigate('/403')
            break
          case 404:
            message = '请求出错(404)'
            break
          case 408:
            message = '请求超时(408)'
            break
          case 500:
            message = '服务器错误(500)'
            break
          case 501:
            message = '服务未实现(501)'
            break
          case 502:
            message = '网络错误(502)'
            break
          case 503:
            message = '服务不可用(503)'
            break
          case 504:
            message = '网络超时(504)'
            break
          case 505:
            message = 'HTTP版本不受支持(505)'
            break
          default:
            message = `连接出错(${error.response.status})!`
        }
        console.error(message)
      } else {
        console.error('未知异常')
      }
      return Promise.reject(error)
    }
  )
  return { instance }
}
export default useAxios
