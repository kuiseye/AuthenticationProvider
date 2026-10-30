<script setup lang="ts">
import { ref } from 'vue';
import { login, me } from '../authService';
import userContext from '../userContext';

    let {setUser} = userContext

    let username = ref<string>()
    let password = ref<string>()

    async function submit(){
        if(username.value == null || password.value == null)
            return
        await login(username.value, password.value)
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
            <label for="password"> Password </label>
            <input autocomplete="current-password" type="password" v-model="password" name="password">
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
</style>