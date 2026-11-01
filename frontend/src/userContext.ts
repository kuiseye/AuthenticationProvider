import { readonly, ref } from "vue";
import { me } from "./authService";


let globalUser = ref<{username : string} | undefined>(undefined)

me()
.then(user => globalUser.value = user)
.catch(reason => console.log(reason))

export default {
    user: readonly(globalUser), 
    setUser: (user:{username: string}) => globalUser.value = user}