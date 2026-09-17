<script setup lang="ts">
    import { ref } from "vue";
    import type {IBookResponseBody, IResponseOperationResult} from "@/models/interfaces.ts";
    import {BookManagerRequests} from "@/services/httpRequests.ts";
    
    type SearchType = "FullText" | "Regex"
    const selectedSearchType = ref<SearchType>("FullText")
    const searchQuery = ref<string>("")
    
    const bookList = defineModel<IBookResponseBody[]>("bookList", {required: true})
    const errorMessage = defineModel<string>("errorMessage")
    
    async function selectSearchQuery(type: SearchType): Promise<IResponseOperationResult<IBookResponseBody[]>> {
        let searchResponse: IResponseOperationResult<IBookResponseBody[]>;
        switch (type) {
            case "FullText":
                searchResponse = await BookManagerRequests.searchBooksByFullText(searchQuery.value)
                break;
            case "Regex":
                searchResponse = await BookManagerRequests.searchBooksByRegex(searchQuery.value)
                break;
        }
        return searchResponse
    }
    
    async function handleBookSearch() {
        const searchResponse = await selectSearchQuery(selectedSearchType.value)
      
        const responseError = searchResponse.operation.errorMessage
        const responseData = searchResponse.responseData  
      
        if (searchResponse.operation.isValid && responseData) {
            bookList.value = responseData
        } else if (responseError) {
            errorMessage.value = responseError
        }
        console.log(responseData)
    }
    
</script>

<template>
    <form class="book-searching-form" @submit.prevent>
        <h1 class="is-size-4">Выберите вид поиска</h1>
        <div class="select">
            <select v-model="selectedSearchType">
                <option value="FullText">Полнотекстовый поиск</option>
                <option value="Regex">Регулярные выражения</option>
            </select>
        </div>
        <input v-model="searchQuery" type="text" class="input" placeholder="Введите запрос">
        <button class="button is-success" @click="handleBookSearch()">Найти</button>
      </form>
</template>

<style scoped>
    .book-searching-form {
        display: flex;
        flex-direction: column;
        gap: 10px;
    }
</style>