<script setup lang="ts">
</script>

<template>
    <nav class="navbar" role="navigation">
        <RouterLink class="navbar-item is-size-3" to="/">BookManager</RouterLink>
        <RouterLink to="/" class="navbar-item">Создать книгу</RouterLink>
        <RouterLink to="/books" class="navbar-item">Список книг</RouterLink>
    </nav>
    <main>
       <RouterView/>
    </main>
</template>

<style scoped></style>
