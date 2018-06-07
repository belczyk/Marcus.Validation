namespace Marcus.Validation
{
    public abstract class Validator<T> : ValidatorBase
    {
        public ValidationResult Validate(T obj)
        {
            if (obj == null) throw new CantValidateNullObjectException<T>(GetType());
            ValidateObject(obj);
            return ValidationResult;
        }

        public void ValidateThrowIfInvalid(T obj)
        {
            var result = Validate(obj);
            result.ThrowIfInvalid();
        }

        protected abstract void ValidateObject(T obj);
    }
}