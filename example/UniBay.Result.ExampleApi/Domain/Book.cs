using UniBay.Result.AspNetCore.ResponseMapper;
using UniBay.Result.Exceptions;
using UniBay.Result.Types;

namespace UniBay.Result.ExampleApi.Domain;

public class Book
{
    public int Id { get; set; }
    public string Author { get; set; }
    public string Title { get; set; }


    public static Result<Book> Create(int id, string author, string title)
    {
        var idResult = ValidateId(id);
        var authorResult = ValidateAuthor(author);
        var titleResult = ValidateTitle(title);
        
        var aggregatedResult = ResultFactory.AggregatedResult(idResult, authorResult, titleResult);
        if (aggregatedResult.IsFailed) return aggregatedResult.Exception;
        
        return new Book { Id = id, Author = author, Title = title };
    }

    public static Result<Nothing> ValidateAuthor(string author)
    {
        if (author.Length <= 3) return new ValidationException(new ValidationFailure(nameof(author), "Author name lenght must be greater than 3"));
        return Nothing.Value;
    }
    
    public static Result<Nothing> ValidateTitle(string title)
    {
        if (title.Length <= 3) return new ValidationException(new ValidationFailure(nameof(title), "Title name lenght must be greater than 3"));
        return Nothing.Value;
    }

    public static Result<Nothing> ValidateId(int id)
    {
        if (id <= 0) return new ValidationException(new ValidationFailure(nameof(id), "Id name lenght must be greater than 0"));
        return Nothing.Value;
    }
}