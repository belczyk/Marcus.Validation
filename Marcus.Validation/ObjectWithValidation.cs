namespace Marcus.Validation
{
    public abstract class ObjectWithValidation : Validator
    {
        public abstract ValidationResult Validate();
    }
}