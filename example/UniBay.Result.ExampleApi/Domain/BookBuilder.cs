using Bogus;

namespace UniBay.Result.ExampleApi.Domain;

public class BookBuilder
{
    private readonly Faker<Book> faker = new();
    public BookBuilder()
    {
        if (Faker.GlobalUniqueIndex <= 0)
            Faker.GlobalUniqueIndex = 1;
        
        this.faker
            .RuleFor(x => x.Id, f => f.UniqueIndex)
            .RuleFor(x => x.Author, f => f.Name.FullName())
            .RuleFor(x => x.Title, f => f.Lorem.Word());
    }

    public Book Build() => this.faker.Generate();
}