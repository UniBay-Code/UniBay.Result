using MediatR;
using UniBay.Result.ExampleApi.Domain;
using UniBay.Result.Exceptions;
using UniBay.Result.Types;

namespace UniBay.Result.ExampleApi.Application.Commands;

public record GetBookQuery(int Id, string Role, bool ServiceError = false, bool UnknownError = false) : IRequest<Result<Book>>
{
    public class GetBookQueryHandler : IRequestHandler<GetBookQuery, Result<Book>>
    {
        private readonly IBooksRepository booksRepository;

        public GetBookQueryHandler(IBooksRepository booksRepository)
        {
            this.booksRepository = booksRepository;
        }
        
        public async Task<Result<Book>> Handle(GetBookQuery request, CancellationToken cancellationToken)
        {
            if (request.UnknownError)
            {
                return new Exception("Unhandled error");
            }
            if (request.Id < 0)
            {
                // Api returns 421
                var validationFailure = new ValidationFailure(nameof(request.Id),"Id must be greater than zero.");
                return new ValidationException(validationFailure);
            }
            
            if (request.Role != "user")
            {
                // Api returns 403
                return new AuthorizationException("'user' Role is required.");
            }

            if (request.ServiceError)
            {
                return new ServiceException("Sql error occured");
            }
            var book = await this.booksRepository.GetById(request.Id);
            if (book is null)
            {
                // Api returns 404
                return new NotFoundException(AppResource.Create<Book>(request.Id));
            }

            return book;
        }
    }
}