<script setup lang="ts">
    import { ref } from 'vue'
    import {BookManagerRequests } from "@/services/httpRequests.ts";
    import type {IBookResponseBody, IResponseOperationResult} from "@/models/interfaces.ts";
    
    const authorList = ref<string[]>([]);
    const title = ref<string>("");
    const annotation = ref<string>("");
    
    const errorMessage = ref<string | null>("");
    
    
    function deleteAuthor(index: number): void {
        if (authorList.value.length > 0) {
            authorList.value.splice(index, 1);
        }
    }
    
    async function handleAddBook(): Promise<void> {
        const bookResponse: IResponseOperationResult<IBookResponseBody> = await BookManagerRequests.addBook(title.value, annotation.value, authorList.value);

        const responseError = bookResponse.operation.errorMessage
      
        if (bookResponse.operation.isValid) {
            errorMessage.value = `Книга ${title.value} добавлена`
            authorList.value = []
            title.value = ""
            annotation.value = ""
        } else if (responseError) {
            errorMessage.value = responseError;
        }
    }
</script>

<template>
    <form class="book-creating-form" @submit.prevent>
      <h1 class="is-size-4">Добавить книгу</h1>
      <input class="input" type="text" v-model="title" placeholder="Название">
      <textarea placeholder="Артикул" v-model="annotation" class="textarea is-link" rows="5"></textarea>
      <div v-for="(author, index) in authorList" class="author-list-element">
          <input class="input" v-model="authorList[index]" placeholder="Имя автора">
          <button class="button is-danger" @click="deleteAuthor(index)">Удалить</button>
      </div>
      <button class="button is-success" @click="authorList.push('')">Добавить автора</button>
      <br><br>
      <button v-if="title && annotation && authorList.length > 0" class="button is-info" @click="handleAddBook()">Добавить книгу</button>
    </form>
    <p>{{ errorMessage }}</p>
</template>

<style scoped>
    .book-creating-form {
        display: flex;
        flex-direction: column;
        gap: 10px;
    }
    .author-list-element {
        display: flex;
        flex-direction: row;
        gap: 5px;
    }
</style>