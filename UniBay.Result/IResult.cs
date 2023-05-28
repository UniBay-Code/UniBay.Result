namespace UniBay.Result;
public interface IResult<out TValue> : IResult
{
    public TValue Value { get; }
  
}
public interface IResult
{
    public ResultCode Code { get; }
    public Exception Exception { get; }
    public bool IsSuccess { get; }
    public bool IsFailed { get; }
}