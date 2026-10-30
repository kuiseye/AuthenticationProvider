<script setup lang="ts">
import { ref } from 'vue';
import { me, register } from '../authService';
import userContext from '../userContext';

    let {setUser} = userContext

    let username = ref<string>()
    let email = ref<string>()
    let password = ref<string>()
    let verifyPassword = ref<string>()

    async function submit(){
        if(username.value == null || email.value == undefined 
        || password.value == undefined || verifyPassword.value == undefined)
            return
        await register(username.value, email.value, password.value, verifyPassword.value)
        me().then(user=>setUser(user))
    }
</script>

<template>
    <form @submit.prevent="submit">
        <div class="entry-container">
            <label for="username"> Username </label>
            <input v-model="username" name="username">
        </div>

        <div class="entry-container">
            <label for="email"> Email </label>
            <input v-model="email" name="email">
        </div>

        <div class="entry-container">
            <label for="password"> Password </label>
            <input autocomplete="new-password"  type='password' v-model="password" name="password">

        </div>

        <div class="entry-container">
            <label for="verify-password"> Confirm Password </label>
            <input type="password" v-model="verifyPassword" name="verify-password">
        </div>

        <div>
            <label class="terms-text"> <input class="terms-checkbox" type="checkbox" name="confirmation"> By checking this checkbox I confirm that I read and agreed to terms and conditions of useage</label>
        </div>

        <button type="submit"> Submit</button>
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
</style>