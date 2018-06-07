namespace Marcus.Validation
{
    public abstract class ObjectWithValidation : ValidatorBase
    {
        public abstract ValidationResult Validate();
    }
}