using System.Text.RegularExpressions;
using Xunit;

namespace Marcus.Validation.Tests
{
    public class ValidationResultTests
    {
        [Fact]
        public void ToString_returns_ObjectIsValid_when_presenting_result_for_valid_object()
        {
            var result = new ValidationResult();
             
            Assert.Equal("Object is valid", result.ToString());
        }

        [Fact]
        public void ToString_returns_information_about_nested_broken_rules_of_complex_object()
        {
            var result = new StubComplexObjectWithValidation().Validate();
            var resultStr = result.ToString();


            Assert.Equal(8, Regex.Matches(resultStr, "IntProperty").Count);
            Assert.Equal(8, Regex.Matches(resultStr, "StringProperty").Count);
        }

        [Fact]
        public void HasBrokenRule_returns_true_when_broken_rule_with_given_name_exists()
        {
            var result = new StubComplexObjectWithValidation().Validate();

            Assert.True(result.HasBrokenRule("StubObject"));
            Assert.True(result.HasBrokenRule("StubObjects"));
        }

        [Fact]
        public void HasBrokenRule_returns_true_when_broken_rule_with_given_name_exists_even_if_it_is_nested()
        {
            var result = new StubComplexObjectWithValidation().Validate();

            Assert.True(result.HasBrokenRule("IntProperty"));
            Assert.True(result.HasBrokenRule("StringProperty"));
        }

    }
}
