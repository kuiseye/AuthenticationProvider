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
        <li class="text-color">
            <il  class="home-container">
                <RouterLink to="/"> Home </RouterLink>
            </il>
            <div v-if="user == undefined" class="auth-container">
                <il >
                    <RouterLink class="text-color" to="/login"> Login </RouterLink>
                </il>
                <il>
                    <RouterLink to="/register"> Register </RouterLink>
                </il>
            </div>
            <div v-else class="auth-container user-data">
                <span>{{ user.username }}</span>
                <button @click="logoutUser">
                    Logout
                </button>
            </div>
        </li>
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

    li{
        width: 100%;
        height: var(--nav-height);

        display: flex;
        flex-wrap: nowrap;

        align-items: center;
        justify-content: space-around;
        

        list-style-type: none;
    }

    il{
        height: var(--nav-height);

        padding: 0px 10px;
        margin: 0px 10px;
    }

    a{
        width: 100%;
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
</style>