using System;

namespace Marcus.Validation
{
    public class InvalidObjectException : Exception
    {
        public InvalidObjectException(ValidationResult validationResult) : base(validationResult.ToString())
        {
            ValidationResult = validationResult;
        }

        public ValidationResult ValidationResult { get; set; }
    }
}