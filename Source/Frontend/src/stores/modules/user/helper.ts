import type { TokenInfo } from '@/api/user'
import { ss } from '@/utils/storage'

export interface User {
  id: string,
  username: string,
  name: string,
  lastLoginDate: string
  createdDate: string,
  email: string
}

const TOKENKEY: string = 'key_tokenInfo'
const USERKEY: string = 'key_user'

export function getLocalTokenInfo(): TokenInfo | null {
  return ss.get(TOKENKEY)
}

export function setLocalTokenInfo(tokenInfo: TokenInfo | null) {
  ss.set(TOKENKEY, tokenInfo)
}

export function getLocalUser(): User | null {
  return ss.get(USERKEY)
}

export function setLocalUser(user: User | null) {
  ss.set(USERKEY, user)
}