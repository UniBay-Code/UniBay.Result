using UniBay.Result.Types;

namespace UniBay.Result.AspNetCore.RequestResponses;

public readonly struct ResourceResponse
{
    /// <summary>
    /// The name of the resource
    /// <br />
    /// </summary>
    /// <example>someEntityId</example>
    public string Resource { get; }

    /// <summary>
    /// Id of the resource
    /// </summary>
    /// <example>0cd3cdd9-a73a-4e3d-8ecd-b7f175df736e</example>
    public string Id { get; }

    public ResourceResponse(AppResource resource)
    {
        this.Resource = resource.Name;
        this.Id = resource.Id;
    }
}