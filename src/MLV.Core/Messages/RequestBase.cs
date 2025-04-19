using FluentValidation.Results;
using MediatR;
using System.Text.Json.Serialization;

namespace MLV.Core.Messages;
public class RequestBase
{
    [JsonIgnore]
    public DateTime Timestamp { get; private set; }

    [JsonIgnore]
    public ValidationResult ValidationResult { get; set; }

    protected Command()
    {
        Timestamp = DateTime.Now;
        ValidationResult = new ValidationResult();
    }

    public virtual bool IsValid()
    {
        throw new NotImplementedException();
    }
}
