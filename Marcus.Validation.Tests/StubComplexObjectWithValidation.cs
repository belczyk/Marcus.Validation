using System.Collections.Generic;

namespace Marcus.Validation.Tests
{
    public class StubComplexObjectWithValidation : ObjectWithValidation
    {
        private StubObjectWithValidation StubObject { get; set;  } = new StubObjectWithValidation();
        private IList<StubObjectWithValidation> StubObjects { get; set;  } = new List<StubObjectWithValidation>
        {
            new StubObjectWithValidation(),
            new StubObjectWithValidation(),
            new StubObjectWithValidation()
        };

        public override ValidationResult Validate()
        {
            IsValid(StubObject, nameof(StubObject));
            AreValid(StubObjects, nameof(StubObjects));

            return ValidationResult;
        }
    }
}
