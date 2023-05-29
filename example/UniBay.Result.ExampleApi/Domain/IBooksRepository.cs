namespace UniBay.Result.ExampleApi.Domain;

public interface IBooksRepository
{
    Task<Book?> GetById(int id);
    Task AddBook(Book book);
}