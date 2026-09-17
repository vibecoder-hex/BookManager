<script setup lang="ts">
    import { ref } from "vue";
    import type { IBookResponseBody, IResponseOperationResult } from "@/models/interfaces.ts";
    import  { BookManagerRequests } from "@/services/httpRequests.ts";
    import BookSearchingForm from "@/components/forms/BookSearchingForm.vue";
    
    const bookList = ref<IBookResponseBody[]>([])
    
    const errorMessage = ref<string>("")
    
    async function handleGetBooks(): Promise<void> {
        const response: IResponseOperationResult<IBookResponseBody[]> = await BookManagerRequests.getBooks();
        
        const responseError = response.operation.errorMessage
        const responseData = response.responseData
      
        if (response.operation.isValid && responseData) {
            bookList.value = responseData;
        } else if (responseError) {
            errorMessage.value = responseError
        }
    }
    
    async function handleDeleteBook(bookTitle: string): Promise<void> {
        const response = await BookManagerRequests.removeBook(bookTitle);
        
        const responseError = response.operation.errorMessage
        if (response.operation.isValid) {
            bookList.value = bookList.value.filter((book: IBookResponseBody) => book.name !== bookTitle)
        } else if (responseError) {
            errorMessage.value = responseError
        }
    }
    
    handleGetBooks();
</script>

<template>
  <p>{{ errorMessage }}</p>
  <BookSearchingForm v-model:bookList="bookList" v-model:errorMessage="errorMessage" />
  <div v-if="bookList.length > 0">
      <div class="card" v-for="book in bookList">
          <header class="card-header">
              <p class="card-header-title is-size-4">{{ book.name }}</p>
          </header>
          <div class="card-content">
              <p>Аннотация: {{book.annotation}}</p>
              <p>Дата публикации: {{book.publishingDate}}</p>
              <p>Авторы: {{book.authors}}</p>
          </div>
          <footer class="card-footer">
              <button class="card-footer-item" @click="handleDeleteBook(book.name)">Удалить</button>
          </footer>
      </div>
  </div>

  <div class="card" v-else>
      <header class="card-header">
        <p class="card-header-title is-size-4">Книг пока нет</p>
      </header>
      <footer class="card-footer">
          <RouterLink to="/" class="card-footer-item">Добавить книгу</RouterLink>
      </footer>
  </div>
</template>

<style scoped>

</style>