using BurnSystems.Collections;

namespace DatenMeister.Validators;

public enum ValidatorState
{
    NotValidated,
    Ok,
    Recommendation,
    Error
}

public class ValidatorResult(ValidatorState state, string message, string propertyName = "")
    : IChainNode<ValidatorResult>
{
    /// <summary>
    /// Gets or sets the state of the validation
    /// </summary>
    public ValidatorState State { get; } = state;

    /// <summary>
    /// Defines the message for the user
    /// </summary>
    public string Message { get; } = message;

    /// <summary>
    /// If there is one specific property associated to the validation, take this
    /// </summary>
    public string? PropertyName { get; } = propertyName;

    /// <summary>
    /// Gets the next validator result as a chained list
    /// </summary>
    public ValidatorResult? Next { get; set; }
}