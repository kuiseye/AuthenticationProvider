<script setup lang="ts">
import { ref } from 'vue';
import { login, me } from '../authService';
import userContext from '../userContext';
import groupValidationEntriesByField, { type AppError, type AppErrorEntry } from '../types/AppError';
import type { AxiosError } from 'axios';

    let {setUser} = userContext

    let username = ref<string>()
    let password = ref<string>()

    let validationErrors = ref<Map<string, Array<AppErrorEntry>>>(new Map())
    let globalError = ref<string|undefined>(undefined)

    let submitting = ref<boolean>(false)

    async function submit(){
        //reset values
        validationErrors.value = new Map()
        globalError.value = undefined

        if(username.value == null || password.value == null)
            return

        await login(username.value, password.value)
        .then(()=>me().then(user=>setUser(user)))
        .catch((reason:AxiosError<AppError>)=> {
            if(reason.response == null)
                return
            let error = reason.response.data 
            if(error.status == 400)
                validationErrors.value = groupValidationEntriesByField(error.validationErrorEntries)
            else if(error.status != 400)
                globalError.value = error.detail
        })
        .finally(()=>submitting.value=false)
    }
</script>

<template>
    <form @submit.prevent="submit">
        <div class="error-entry">
            {{ globalError }}
        </div>
        <div class="entry-container">
            <div v-if="validationErrors.has('Username')">
                <ul class="error-list">
                    <li class="error-entry" v-for="error in validationErrors.get('Username')">
                        {{ error.message }}
                    </li>
                </ul>
            </div>
            <label for="username"> Username </label>
            <input v-model="username" name="username" id="username">
        </div>

        <div class="entry-container">
            <div v-if="validationErrors.has('Password')">
                <ul class="error-list">
                    <li class="error-entry" v-for="error in validationErrors.get('Password')">
                        {{ error.message }}
                    </li>
                </ul>
            </div>
            <label for="password"> Password </label>
            <input autocomplete="current-password" type="password" v-model="password" name="password" id="password">
        </div>

        <button :disabled="submitting" type="submit">Submit</button>
    </form>
</template>

<style lang="css" scoped>
    form{
        width: 100%;
        height: 100%;
        display: flex;

        flex-wrap: wrap;
        justify-content: left;
    }

    .entry-container{
        width: 100%;
    }

    input{
        width: 100%;
        margin-bottom: 10px;
    }

    input:focus{
        outline: none;
    }

    button{
        width: 100%;

        margin-top: 20px;

        cursor: pointer;
    }

    label{
        color: var(--text-color)
    }

    .error-entry{
        color: red;
        font-size: 10px;
    }
    .error-list{
        margin-left: 0;
        padding-left: 0;
    }
    .error-list::marker{
        color:red;
    }
</style>