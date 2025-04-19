using FluentValidation.Results;
using System.Text.Json.Serialization;

namespace MLV.Business.Commands;
public class RequestBase
{
    [JsonIgnore]
    public ValidationResult ValidationResult { get; set; }

    protected RequestBase()
    {
        ValidationResult = new ValidationResult();
    }

    public virtual bool IsValid()
    {
        throw new NotImplementedException();
    }
}
