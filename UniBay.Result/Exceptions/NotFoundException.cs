using UniBay.Result.Types;

namespace UniBay.Result.Exceptions;

/// <summary>
/// Exception should be thrown when requested resource was not found.
/// </summary>
public class NotFoundException : ResultException
{
    public readonly AppResource Resource;

    public NotFoundException(AppResource resource)
            : base(code: ResultCode.NotFound,
                   messageTemplate: "Resource {0} with id {1} was not found", resource.Name, resource.Id)
    {
        this.Resource = resource;
    }

    public static NotFoundException Create<TResource>(Guid id) => Create<TResource>(id.ToString());
    public static NotFoundException Create<TResource>(string id) => new(AppResource.Create<TResource>(id));
}