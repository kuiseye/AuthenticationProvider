<script lang="ts" setup>
import userContext from '../userContext';

    const {user} = userContext
</script>

<template>
    <div class="page">
        <h1 v-if="user == undefined">Home... or is it?</h1>
        <h1 v-else> Home sweet home! Welcome, {{ user.username }}!</h1>
    </div>
</template>

<style lang="css" scoped>
    h1{
        color:var(--text-color);
    }
    .page{
        height: 100%;
        
        display: flex;
        align-items: center;
        justify-content: center;
    }
</style>