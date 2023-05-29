using MediatR;
using UniBay.Result.ExampleApi.Domain;
using UniBay.Result.Exceptions;
using UniBay.Result.Types;

namespace UniBay.Result.ExampleApi.Application.Commands;

public record AddBookCommand(int Id, string Author, string Title) : IRequest<Result<Book>>
{
    public class AddBookCommandHandler : IRequestHandler<AddBookCommand, Result<Book>>
    {
        private readonly IBooksRepository booksRepository;

        public AddBookCommandHandler(IBooksRepository booksRepository)
        {
            this.booksRepository = booksRepository;
        }
        
        public async Task<Result<Book>> Handle(AddBookCommand command, CancellationToken cancellationToken)
        {
            var bookResult = Book.Create(command.Id, command.Author, command.Title);
            if (bookResult.IsFailed) return bookResult;

            var book = await this.booksRepository.GetById(bookResult.Value.Id);
            if (book != null) return new ConflictException(AppResource.Create<Book>(command.Id));

            await this.booksRepository.AddBook(bookResult);
            return bookResult.AsCreated();
        }
    }
}