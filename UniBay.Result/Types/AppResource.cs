namespace UniBay.Result.Types;

public readonly struct AppResource
{
    public readonly string Id;
    public readonly string Name;

    public AppResource(string name, string id)
    {
        this.Name = name;
        this.Id = id;
    }

    public static AppResource Create<TResource>(int id) => Create<TResource>(id.ToString());
    public static AppResource Create<TResource>(long id) => Create<TResource>(id.ToString());
    public static AppResource Create<TResource>(Guid id) => Create<TResource>(id.ToString());
    public static AppResource Create<TResource>(string id)
    {
        var name = typeof(TResource).Name;
        return new AppResource(name, id);
    }
    
    public override int GetHashCode() => HashCode.Combine(this.Id, this.Name);
    public override bool Equals(object obj) => obj is AppResource other && Equals(other);

    public bool Equals(AppResource other) =>
            string.Equals(this.Id, other.Id) &&
            string.Equals(this.Name, other.Name);

    public static bool operator ==(AppResource lhs, AppResource rhs) => lhs.Equals(rhs);
    public static bool operator !=(AppResource lhs, AppResource rhs) => !(lhs == rhs);
}