using UniBay.Result.ExampleApi.Domain;

namespace UniBay.Result.ExampleApi.Infrastructure;

public class BooksRepository : IBooksRepository
{
    private readonly List<Book> books = new()
    {
        new BookBuilder().Build(),
        new BookBuilder().Build(),
        new BookBuilder().Build(),
        new BookBuilder().Build()
    };

    public async Task<Book?> GetById(int id) => await Task.FromResult(this.books.FirstOrDefault(x => x.Id == id));

    public async Task AddBook(Book book)
    {
        this.books.Add(book);
        await Task.CompletedTask;
    }
}