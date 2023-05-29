namespace UniBay.Result;

/// <summary>
/// Represents a void type, since <see cref="System.Void"/> is not a valid return type in C#.
/// </summary>
public readonly struct Nothing
{
    private static readonly Nothing value = new();
    public static ref readonly Nothing Value => ref Nothing.value;

    public static Task<Nothing> Task { get; } = System.Threading.Tasks.Task.FromResult(Nothing.value);
    public int CompareTo(Nothing other) => 0;
    public override int GetHashCode() => 0;
    public bool Equals(Nothing other) => true;
    public override bool Equals(object obj) => obj is Nothing;
    public static bool operator ==(Nothing first, Nothing second) => true;
    public static bool operator !=(Nothing first, Nothing second) => false;
    public override string ToString() => "()";
}