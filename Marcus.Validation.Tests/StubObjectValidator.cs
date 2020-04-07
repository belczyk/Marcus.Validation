namespace Marcus.Validation.Tests
{
    public class StubObjectValidator : Validator<StubObject>
    {
        protected override void ValidateObject(StubObject obj)
        {
            NotNullOrWhiteSpace(obj.StringProperty, nameof(obj.StringProperty));
            NotDefault(obj.IntProperty, nameof(obj.IntProperty));
            NullOrNotEmptyGuid(obj.GuidProperty, nameof(obj.GuidProperty));
        }
    }
}
