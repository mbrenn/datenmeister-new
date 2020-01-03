#nullable enable

namespace DatenMeister.Modules.Validators
{
    public enum ValidatorState
    {
        NotValidated,
        Ok, 
        Recommendation, 
        Failed
    }
    
    public class ValidatorResult
    {
        public ValidatorResult(ValidatorState state, string message)
        {
            State = state;
            Message = message;
        }

        /// <summary>
        /// Gets or sets the state of the validation
        /// </summary>
        public ValidatorState State { get; }
        
        /// <summary>
        /// Defines the message for the user
        /// </summary>
        public string Message { get; set; }
        
        /// <summary>
        /// If there is one specific property associated to the validation, take this
        /// </summary>
        public string? PropertyName { get; set; }
        
        /// <summary>
        /// Gets the next validator result as a chained list
        /// </summary>
        public ValidatorResult? Next { get; set; }

    }
}