<script setup lang="ts">
import RegisterForm from './RegisterForm.vue';

</script>

<template>
    <div class="page">
        <div class="Register-form-container">
            <h1>Register</h1>
            <RegisterForm/>
        </div>
        <div style="width: 100%;"></div>
    </div>
</template>


<style scoped>
    .Register-form-container{
        background-color: rgb(35,35,35);
        border-radius: 10px;
        width: 500px;
        height: fit-content;

        padding: 50px;
    }

    .page{
        width: 100%;
        height: 100%;

        display: flex;
        flex-wrap: wrap;

        align-items: center;
        justify-content: center;
    }
    h1{
        width: 100%;

        color: var(--text-color);
        font-size: 1.3rem;

        text-align: center;

        margin-bottom: 10px;
    }
</style>