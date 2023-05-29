using System.Collections;
using UniBay.Result.Types;

namespace UniBay.Result.Exceptions;

public class ConflictException : ResultException
{
    public readonly AppResource Resource;

    public ConflictException(AppResource resource)
            : base(code: ResultCode.Conflict, 
                   messageTemplate: "Resource {0} with id {1} already exists", resource.Name, resource.Name)
    {
        this.Resource = resource;
    }

    public static ConflictException Create<TResource>(string id) => new(AppResource.Create<TResource>(id));
}