import { readonly, ref } from "vue";
import { me } from "./authService";
import type { User } from "./types/User";


let globalUser = ref<User | null | undefined>(undefined)

me()
.then(user => globalUser.value = user)
.catch(reason => {console.log(reason); globalUser.value == null})

export default {
    user: readonly(globalUser), 
    setUser: (user:User|null) => globalUser.value = user}