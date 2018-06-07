using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Marcus.Validation.Tests
{
    public class ValidatorTests
    {

        [Fact]
        public void ValidateThrowIfInvalid_throws_exception_if_invalid_object_provided()
        {
            var obj = new StubObject
            {
                GuidProperty = Guid.NewGuid(),
                IntProperty = 1,
                StringProperty = "abc"
            };

            var validator = new StubObjectValidator();
            validator.ValidateThrowIfInvalid(obj);
        }


        [Fact]
        public void ValidateThrowIfInvalid_doesnt_throw_if_object_is_valid()
        {
            var obj = new StubObject();

            var validator = new StubObjectValidator();

            var ex = Record.Exception(() => validator.ValidateThrowIfInvalid(obj));

            Assert.NotNull(ex);
            Assert.IsType<InvalidObjectException>(ex);

            Assert.True(((InvalidObjectException)ex).ValidationResult.IsNotValid);
            Assert.Equal(2, ((InvalidObjectException)ex).ValidationResult.BrokenRules.Count);

        }

        [Fact]
        public void Validate_throws_exception_when_null_passed()
        {
            var validator = new StubObjectValidator();
            Assert.Throws<CantValidateNullObjectException<StubObject>>(() => validator.Validate(null));
        }
    }


}
