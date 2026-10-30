import {  createRouter, createWebHistory } from "vue-router";

import Home from "./components/Home.vue";
import LoginPage from "./components/LoginPage.vue";
import RegisterPage from "./components/RegisterPage.vue";

const routes = [
    {path:"/",component:Home},
    {path:"/login",component:LoginPage},
    {path:"/register",component:RegisterPage}
]

export default createRouter({history:createWebHistory(), routes})