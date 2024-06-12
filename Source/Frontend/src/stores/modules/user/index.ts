import { ref } from 'vue'
import { store } from '@/stores'
import { defineStore } from 'pinia'
import type { User } from './helper'
import { login, getCurrentUser, type TokenInfo } from '@/api/user'
import type { HttpResponse } from '@/utils/request/helper'
import { setLocalTokenInfo, getLocalTokenInfo, setLocalUser, getLocalUser } from './helper'
import type { LoginDTO } from '../auth/helper'

export const useUserStore = defineStore('user', () => {
    const tokenInfo = ref<TokenInfo | null>(getLocalTokenInfo())
    const user = ref<User | null>(getLocalUser())
    function setUser(userValue: User) {
        user.value = userValue
        setLocalUser(userValue)
    }
    function clearUser() {
        user.value = null
        setLocalUser(null)
    }


    function setTokenInfo(tokenInfoValue: TokenInfo) {
        tokenInfo.value = tokenInfoValue
        setLocalTokenInfo(tokenInfoValue)
    }

    function clearTokenInfo() {
        tokenInfo.value = null
        setLocalTokenInfo(null)
    }

    async function loginAsync(user: LoginDTO): Promise<HttpResponse<TokenInfo>> {
        const httpResponse: HttpResponse<TokenInfo> = await login(user)
        if (httpResponse.succeed) {
            setTokenInfo(httpResponse.response)
        }
        return httpResponse;
    }

    async function getCurrentUserAsync(): Promise<HttpResponse<User>> {
        const httpResponse: HttpResponse<User> = await getCurrentUser();
        if (httpResponse.succeed) {
            setUser(httpResponse.response)
        }
        return httpResponse;
    }

    return {
        user,
        setUser,
        clearUser,
        tokenInfo,
        setTokenInfo,
        clearTokenInfo,
        loginAsync,
        getCurrentUserAsync
    }
})

export function useUser() {
    return useUserStore(store);
}