import { ss } from '@/utils/storage'


export interface LoginDTO {
    username: string
    password: string
}

const LOGINKEY: string = "key_login";
const REMEBERKEY: string = "key_remember";

function getDefaultLoginDTO(): LoginDTO {
    return {
        username: '',
        password: '',
    }
}

export function getLocalLoginDTO(): LoginDTO {
    return ss.get(LOGINKEY) || getDefaultLoginDTO()
}

export function setLocalLoginDTO(loginDTO: LoginDTO | null) {
    ss.set(LOGINKEY, loginDTO || getDefaultLoginDTO())
}

export function getLocalRemember(): boolean {
    return ss.get(REMEBERKEY) || false
}

export function setLocalRemember(remember: boolean) {
    ss.set(REMEBERKEY, remember)
}