using BurnSystems.Collections;

#nullable enable

namespace DatenMeister.Validators
{
    public enum ValidatorState
    {
        NotValidated,
        Ok,
        Recommendation,
        Error
    }

    public class ValidatorResult : IChainNode<ValidatorResult>
    {
        public ValidatorResult(ValidatorState state, string message, string propertyName = "")
        {
            State = state;
            Message = message;
            PropertyName = propertyName;
        }

        /// <summary>
        /// Gets or sets the state of the validation
        /// </summary>
        public ValidatorState State { get; }

        /// <summary>
        /// Defines the message for the user
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// If there is one specific property associated to the validation, take this
        /// </summary>
        public string? PropertyName { get; }

        /// <summary>
        /// Gets the next validator result as a chained list
        /// </summary>
        public ValidatorResult? Next { get; set; }
    }
}