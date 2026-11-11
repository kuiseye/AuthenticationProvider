<script setup lang="ts">
import Navigation from './components/Navigation.vue';
import userContext from './userContext.ts';
import './variables.css'

let {user} = userContext
</script>



<template>
  <div v-show="user !== undefined">
      <Navigation/>
    <hr/>
    <main>
        <RouterView/>
    </main>
  </div>
</template>

<style lang="css"  scoped>
  hr{
    border: none;
    height: var(--nav-border-height);
    background-color: white;
  }

  main{
    height: calc(100vh - var(--nav-height) - var(--nav-border-height));
  }
</style>