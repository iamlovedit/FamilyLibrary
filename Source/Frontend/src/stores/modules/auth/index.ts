import { ref } from 'vue'
import { defineStore } from 'pinia'
import { type LoginDTO, getLocalLoginDTO, getLocalRemember, setLocalRemember, setLocalLoginDTO } from './helper'


export const useAuthStore = defineStore('auth', () => {
    const remember = ref<boolean>(getLocalRemember())
    const loginDTO = ref<LoginDTO>(getLocalLoginDTO())

    function setRemember(rememberValue: boolean) {
        remember.value = rememberValue
        setLocalRemember(rememberValue)
    }

    function setLoginDTO(loginDTOValue: LoginDTO) {
        loginDTO.value = loginDTOValue
        setLocalLoginDTO(loginDTOValue)
    }

    function clearLoginDTO() {
        setLocalLoginDTO(null);
        loginDTO.value = getLocalLoginDTO();
    }

    return {
        remember,
        setRemember,
        loginDTO,
        setLoginDTO,
        clearLoginDTO
    }
})