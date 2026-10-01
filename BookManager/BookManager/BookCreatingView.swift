import SwiftUI

struct BookStructure {
    var name: String
    var authors: [String]
    var annotation: String
}


struct TextFieldModifer : ViewModifier {
    func body(content: Content) -> some View {
        content
            .textFieldStyle(.plain)
            .font(.system(size: 16))
            .padding(.vertical, 15)
            .padding(.horizontal, 10)
            .glassEffect(.regular.interactive(), in: .rect(cornerRadius: 10))
    }
}


extension View {
    func textFieldModifer() -> some View {
        modifier(TextFieldModifer())
    }
}

struct BookCreatingView: View {
    @State private var book = BookStructure(name: "", authors: [], annotation: "")
    
    var body: some View {
        NavigationStack {
            ScrollView {
                VStack() {
                    VStack(spacing: 20) {
                        TextField("\(Image(systemName: "book")) Название", text: $book.name)
                            .textFieldModifer()
                        TextField("\(Image(systemName: "book.pages")) Аннотация", text: $book.annotation, axis:.vertical)
                            .textFieldModifer()
                            .lineLimit(10)
                        ScrollView {
                            ForEach(0..<book.authors.count, id:\.self) {index in
                                GlassEffectContainer(spacing: 0) {
                                        HStack(spacing: 5) {
                                            TextField("\(Image(systemName: "person")) Имя автора \(index + 1)", text: $book.authors[index])
                                                .textFieldModifer()
                                            Button("Delete", systemImage: "trash") {
                                                book.authors.remove(at: index)
                                            }
                                            .buttonStyle(.plain)
                                            .padding(8)
                                            .glassEffect(.regular.tint(.red).interactive(), in: .circle)
                                            .labelStyle(.iconOnly)
                                        }
                                    }
                                }
                            }
                            .frame(height: 300)
                            .cornerRadius(15)
                        Button("Добавить автора") {
                            if (book.authors.count <= 10) {
                                book.authors.append("")
                            }
                        }
                        .buttonStyle(.plain)
                        .padding(12)
                        .glassEffect(.regular.tint(.cyan).interactive())
                    }
                }
                .frame(maxWidth: 600)
                .padding(13)
                .navigationTitle("Добавить книгу")
            }
            .scrollDismissesKeyboard(.immediately)
        }
    }
}

