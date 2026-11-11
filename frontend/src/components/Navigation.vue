<script setup lang="ts">
import { logout } from '../authService';
import userContext from '../userContext';


    const { user, setUser } = userContext 
    console.log(user.value)

    function logoutUser(){
        logout().then(()=>{
            setUser(null)
        })
    }
</script>

<template>
    <nav>
        <ul class="text-color">
            <li  class="home-container">
                <RouterLink to="/" active-class="is-active"> Home </RouterLink>
            </li>
            <div v-if="user == undefined" class="auth-container">
                <li >
                    <RouterLink class="text-color" to="/login" active-class="is-active"> Login </RouterLink>
                </li>
                <li>
                    <RouterLink to="/register" active-class="is-active"> Register </RouterLink>
                </li>
            </div>
            <div v-else class="auth-container user-data">
                <span>{{ user.username }}</span>
                <button @click="logoutUser">
                    Logout
                </button>
            </div>
        </ul>
    </nav>
</template>

<style lang="css" scoped>

    nav{
        width: 100%;
        height: var(--nav-height);
    }

    .home-container{
        display: flex;

        align-items: center;
        justify-content: center;
    }
    .auth-container{
        height: var(--nav-height);
        display: flex;

        align-items: center;
        justify-content: space-around;
    }

    ul{
        height: var(--nav-height);

        display: flex;
        flex-wrap: nowrap;

        align-items: center;
        justify-content: space-around;
        

        list-style-type: none;
    }

    li{
        height: var(--nav-height);

        padding: 0px 10px;
        margin: 0px 10px;
    }

    a{
        display: inline-block;
        width: 100%;
        padding: 0px 10px;
        height: var(--nav-height);
        line-height: var(--nav-height);

        color: var(--text-color);
        text-decoration: none;

        text-align: center;

        transition: ease-in-out 150ms;
    }

    a:hover{
        color: red;
    }

    button{
        border-width: 0px;
        width: 100%;
        height: var(--nav-height);
        line-height: var(--nav-height);
        background-color: rgba(0, 0, 0,0);
        border-color: rgba(0, 0, 0,0);

        color: var(--text-color);
        text-decoration: none;

        text-align: center;

        transition: ease-in-out 150ms;

        padding: 0px 10px;
        margin: 0px 10px;
    }

    button:hover{
        color: red;
    }

    .user-data{
        height: 100%;
        color:var(--text-color)
    }

    a.is-active{
        background-color: rgb(66 184 131 / 14%);
    }
</style>