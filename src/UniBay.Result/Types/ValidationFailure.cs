namespace UniBay.Result.Types;

public readonly struct ValidationFailure
{
    public readonly string PropertyName;
    public readonly string Message;

    public ValidationFailure(string propertyName, string message)
    {
        this.PropertyName = propertyName.ToCamelCase();
        this.Message = message;
    }

    /// <summary>
    /// Create validation failure from parent property
    /// </summary>
    /// <param name="propertyName">Parent property name</param>
    /// <returns></returns>
    public ValidationFailure FromParent(string propertyName)
    {
        string newPropertyName = propertyName;
        if (!string.IsNullOrWhiteSpace(this.PropertyName))
            newPropertyName = $"{newPropertyName}.{this.PropertyName}";
        
        return new(newPropertyName,
            this.Message);
    }
}