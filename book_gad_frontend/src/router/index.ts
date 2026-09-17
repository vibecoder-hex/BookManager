import { createRouter, createWebHistory } from "vue-router";
import BookCreationView from "../views/BookCreatingView.vue"
import BookListView from "../views/BookListView.vue"

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: [
        { path: '/', component: BookCreationView },
        { path: '/books', component: BookListView }
    ]
})

export default router