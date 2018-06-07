using System;

namespace Marcus.Validation
{
    public class CantValidateNullObjectException<TEntity> : Exception
    {
        public CantValidateNullObjectException(Type validatorType) : base(
            $"Can't validate null object. Validator {validatorType.Name} expected instance of {typeof(TEntity).Name}")
        {
        }
    }
}