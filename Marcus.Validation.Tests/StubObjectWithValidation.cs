using System;

namespace Marcus.Validation.Tests
{
    public class StubObjectWithValidation : ObjectWithValidation
    {
        public int IntProperty { get; set; }
        public string StringProperty { get; set; }
        public Guid? GuidProperty { get; set; }

        public override ValidationResult Validate()
        {
            NotNullOrWhiteSpace(StringProperty, nameof(StringProperty));
            NotDefault(IntProperty, nameof(IntProperty));
            NullOrNotEmptyGuid(GuidProperty, nameof(GuidProperty));

            return ValidationResult;
        }
    }
}
