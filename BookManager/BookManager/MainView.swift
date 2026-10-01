import SwiftUI
import Playgrounds


struct MainContentView: View {
    var body: some View {
        TabView() {
            Tab("Мои книги", systemImage: "house") {
                
            }
            Tab("Добавить книгу", systemImage: "book.pages") {
                BookCreatingView()
            }
        }
        
    }
}
