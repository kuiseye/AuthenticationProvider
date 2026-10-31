import globalAxios from "./axiosGlobal";

export function me() : Promise<{username: string}> {
    return globalAxios.get('me').then(response => response.data)
}

export function login(username: string, password: string) : Promise<void>{
    return globalAxios.post('login',{username:username, password:password})
}

export function register(username: string, email: string, password: string, verifyPassword: string) : Promise<void>{
    return globalAxios.post('register',{username:username, email: email, password:password, verifyPassword: verifyPassword})
}