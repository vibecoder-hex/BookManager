<script setup lang="ts">
    import { ref } from 'vue'
    const authorList = ref<string[]>([]);
    const title = ref<string>('');
    const article = 
</script>

<template>
    <input class="input" type="text" placeholder="Название">
    <textarea placeholder="Артикул" class="textarea is-link" rows="5"></textarea>
    <div v-for="(author, index) in authorList">
        <input class="input" placeholder="Автор {{index + 1}}">
    </div>
</template>

<style scoped>

</style>