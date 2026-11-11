<script setup lang="ts">
import { ref } from 'vue';
import { me, register } from '../authService';
import userContext from '../userContext';
import type { AxiosError } from 'axios';
import type { AppError, AppErrorEntry } from '../types/AppError';
import groupValidationEntriesByField from '../types/AppError';

    let {setUser} = userContext

    let username = ref<string>()
    let email = ref<string>()
    let password = ref<string>()
    let verifyPassword = ref<string>()

    let validationErrors = ref<Map<string, Array<AppErrorEntry>>>(new Map());
    let globalError = ref<string|undefined>(undefined)

    let submitting = ref<boolean>(false)

    async function submit(){
        //reset values
        validationErrors.value = new Map()
        globalError.value = undefined

        if(username.value == null || email.value == null || password.value == null || verifyPassword.value == null)
            return

        await register(username.value, email.value, password.value, verifyPassword.value)
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
            <input v-model="username" name="username">
        </div>

        <div class="entry-container">
            <div v-if="validationErrors.has('Email')">
            <ul class="error-list">
                <li class="error-entry" v-for="error in validationErrors.get('Email')">
                    {{ error.message }}
                </li>
            </ul>
            </div>
            <label for="email"> Email </label>
            <input v-model="email" name="email">
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
            <input autocomplete="new-password"  type='password' v-model="password" name="password">

        </div>

        <div class="entry-container">
            <div v-if="validationErrors.has('VerifyPassword')">
            <ul class="error-list">
                <li class="error-entry" v-for="error in validationErrors.get('VerifyPassword')">
                    {{ error.message }}
                </li>
            </ul>
            </div>
            <label for="verify-password"> Confirm Password </label>
            <input type="password" v-model="verifyPassword" name="verify-password">
        </div>

        <div>
            <label class="terms-text"> <input class="terms-checkbox" type="checkbox" name="confirmation"> By checking this checkbox I confirm that I read and agreed to terms and conditions of useage</label>
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

    .terms-text{
        font-size: 0.8rem;

        display: flex;
        align-items: center;

        margin-bottom: 10px;
    }

    .terms-checkbox{
        width: auto;   
        height: 0.8rem;
        margin-right: 10px;
        margin-bottom: 0;

        cursor: pointer;
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