using System.Net;

namespace UniBay.Result;

public readonly struct ResultCode
{
    public readonly string Code = nameof(ResultCode.Error);
    public readonly bool IsSuccess = false;
    public readonly bool IsFailed = true;
    public readonly HttpStatusCode HttpStatusCode = HttpStatusCode.BadGateway;

    public ResultCode(string code, bool success, HttpStatusCode httpStatusCode)
    {
        this.Code = code;
        this.IsSuccess = success;
        this.HttpStatusCode = httpStatusCode;
        this.IsFailed = !success;
    }
    
    public static readonly ResultCode Ok = new(nameof(ResultCode.Ok), true, HttpStatusCode.OK);
    public static readonly ResultCode NoContent = new(nameof(ResultCode.NoContent), true, HttpStatusCode.NoContent);
    public static readonly ResultCode Created = new(nameof(ResultCode.Created), true, HttpStatusCode.Created);

    public static readonly ResultCode Error = new(nameof(ResultCode.Error), false, HttpStatusCode.BadRequest);
    public static readonly ResultCode UnknownError = new(nameof(ResultCode.UnknownError), false, HttpStatusCode.BadRequest);
    public static readonly ResultCode MultipleErrors = new(nameof(ResultCode.MultipleErrors), false, HttpStatusCode.BadRequest);
    public static readonly ResultCode DomainError = new(nameof(ResultCode.DomainError), false, HttpStatusCode.BadRequest);
    public static readonly ResultCode ServiceError = new(nameof(ResultCode.ServiceError), false, HttpStatusCode.BadRequest);
    public static readonly ResultCode NotFound = new(nameof(ResultCode.NotFound), false, HttpStatusCode.NotFound);
    public static readonly ResultCode Conflict = new(nameof(ResultCode.Conflict), false, HttpStatusCode.Conflict);
    public static readonly ResultCode ValidationFailed = new(nameof(ResultCode.ValidationFailed), false, HttpStatusCode.UnprocessableEntity);
    public static readonly ResultCode AuthorizationFailed = new(nameof(ResultCode.AuthorizationFailed), false, HttpStatusCode.Forbidden);

    public override int GetHashCode() => HashCode.Combine(this.Code, this.IsSuccess, this.IsFailed);
    public override bool Equals(object obj) => obj is ResultCode other && Equals(other);

    public bool Equals(ResultCode other) =>
            string.Equals(this.Code, other.Code, StringComparison.OrdinalIgnoreCase) &&
            this.IsSuccess == other.IsSuccess &&
            this.IsFailed == other.IsFailed &&
            this.HttpStatusCode == other.HttpStatusCode;

    public static bool operator ==(ResultCode lhs, ResultCode rhs) => lhs.Equals(rhs);
    public static bool operator !=(ResultCode lhs, ResultCode rhs) => !(lhs == rhs);
}