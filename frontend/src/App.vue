<script setup lang="ts">
import Navigation from './components/Navigation.vue';
import './variables.css'
</script>



<template>
  <Navigation/>
  <hr/>
  <main>
    <RouterView/>
  </main>
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