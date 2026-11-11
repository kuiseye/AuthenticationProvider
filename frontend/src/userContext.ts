import { readonly, ref } from "vue";
import { me } from "./authService";


let globalUser = ref<{username : string} | null | undefined>(undefined)

me()
.then(user => globalUser.value = user)
.catch(reason => {console.log(reason); globalUser.value == null})

export default {
    user: readonly(globalUser), 
    setUser: (user:{username: string}|null) => globalUser.value = user}