using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Marcus.Validation.Tests
{
    public class ValidatorBaseTests : Validator
    {
        private const string PropertyName = "property";
        private StubObjectWithValidation StubObjectWithValidation { get; set; }
        private IList<StubObjectWithValidation> StubObjecstWithValidations { get; set; }


        [Fact]
        public void ValueIn_throws_when_allowed_values_is_null()
        {
            Assert.Throws<Exception>(() => ValueIn(1, null, PropertyName));
        }

        [Fact]
        public void ValueIn_adds_broken_rule_when_value_not_in_the_allowed_set()
        {
            ValueIn(5, new[] { 1, 2, 3, 4 }, PropertyName);

            Assert.False(ValidationResult.IsValid);
            Assert.True(ValidationResult.IsNotValid);
            Assert.Equal(1, ValidationResult.BrokenRules.Count);
            Assert.Equal(PropertyName, ValidationResult.BrokenRules[0].Name);
        }

        [Fact]
        public void ValueIn_doesnt_add_broken_rule_when_value_in_the_allowed_set()
        {
            ValueIn(1, new[] { 1, 2, 3, 4 }, PropertyName);

            Assert.True(ValidationResult.IsValid);
        }

        [Fact]
        public void ValuesIn_throws_when_allowed_values_is_null()
        {
            Assert.Throws<Exception>(() => ValuesIn(new[] { 1, 2, 3 }, null, PropertyName));
        }

        [Fact]
        public void ValuesIn_adds_broken_rule_when_any_of_values_not_in_the_allowed_set()
        {
            // when
            ValuesIn(new[] { 1, 2, 5, 3 }, new[] { 1, 2, 3, 4 }, PropertyName);

            Assert.False(ValidationResult.IsValid);
            Assert.True(ValidationResult.IsNotValid);
            Assert.Equal(1, ValidationResult.BrokenRules.Count);
            Assert.Equal(PropertyName, ValidationResult.BrokenRules[0].Name);
        }

        [Fact]
        public void ValuesIn_doesnt_add_broken_rule_when_values_in_the_allowed_set()
        {
            // when
            ValuesIn(new[] { 1, 2 }, new[] { 1, 2, 3, 4 }, PropertyName);

            Assert.True(ValidationResult.IsValid);
        }

        [Fact]
        public void NotNull_adds_broken_rule_when_object_is_null()
        {
            NotNull(null, PropertyName);

            Assert.False(ValidationResult.IsValid);
            Assert.Equal(1, ValidationResult.BrokenRules.Count);
            Assert.Equal(PropertyName, ValidationResult.BrokenRules[0].Name);
        }

        [Fact]
        public void NotNull_doesnt_add_broken_rule_when_object_is_not_all()
        {
            NotNull("val", PropertyName);

            Assert.True(ValidationResult.IsValid);
        }

        [Fact]
        public void IsTrue_adds_broken_rule_when_value_is_false()
        {
            IsTrue(false, PropertyName);

            Assert.False(ValidationResult.IsValid);
            Assert.Equal(1, ValidationResult.BrokenRules.Count);
            Assert.Equal(PropertyName, ValidationResult.BrokenRules[0].Name);
        }


        [Fact]
        public void IsTrue_doesnt_add_broken_rule_when_value_is_true()
        {
            IsTrue(true, PropertyName);

            Assert.True(ValidationResult.IsValid);
        }


        [Fact]
        public void IsFalse_adds_broken_rule_when_value_is_true()
        {
            IsFalse(true, PropertyName);

            Assert.False(ValidationResult.IsValid);
            Assert.Equal(1, ValidationResult.BrokenRules.Count);
            Assert.Equal(PropertyName, ValidationResult.BrokenRules[0].Name);
        }

        [Fact]
        public void IsFalse_doesnt_add_broken_rule_when_value_is_false()
        {
            IsFalse(false, PropertyName);

            Assert.True(ValidationResult.IsValid);
        }

        [Fact]
        public void AreNotDefault_Guid_adds_broken_rule_when_one_of_the_guids_has_default_value()
        {
            AreNotDefault(new[] { Guid.NewGuid(), Guid.NewGuid(), new Guid(), Guid.NewGuid() }, PropertyName);

            Assert.False(ValidationResult.IsValid);
            Assert.Equal(1, ValidationResult.BrokenRules.Count);
            Assert.Equal(PropertyName, ValidationResult.BrokenRules[0].Name);
        }


        [Fact]
        public void AreNotDefault_Guid_doesnt_add_broken_rule_when_none_of_the_guids_has_default_value()
        {
            AreNotDefault(new[] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() }, PropertyName);

            Assert.True(ValidationResult.IsValid);
        }
        [Fact]
        public void AreNotDefault_Guid_doesnt_add_broken_rule_when_guid_list_is_null()
        {
            AreNotDefault(null, PropertyName);

            Assert.True(ValidationResult.IsValid);
        }

        [Fact]
        public void AreNotDefault_Guid_doesnt_add_broken_rule_when_guid_list_emptyl()
        {
            AreNotDefault(new Guid[0], PropertyName);

            Assert.True(ValidationResult.IsValid);
        }

        [Fact]
        public void PositiveNumber_adds_broken_rule_when_number_is_less_than_zero()
        {
            PositiveNumber(-1.0m, PropertyName);

            Assert.False(ValidationResult.IsValid);
            Assert.Equal(1, ValidationResult.BrokenRules.Count);
            Assert.Equal(PropertyName, ValidationResult.BrokenRules[0].Name);
        }

        [Fact]
        public void PositiveNumber_doesnt_add_broken_rule_when_number_is_more_than_zero()
        {
            PositiveNumber(1.0m, PropertyName);

            Assert.True(ValidationResult.IsValid);
        }

        [Fact]
        public void PositiveNumber_doesnt_add_broken_rule_when_number_is_zero()
        {
            PositiveNumber(0.0m, PropertyName);

            Assert.True(ValidationResult.IsValid);
        }

        [Fact]
        public void LessThanZero_adds_broken_rule_when_number_is_more_than_zero()
        {
            LessThanZero(10.0m, PropertyName);

            Assert.False(ValidationResult.IsValid);
            Assert.Equal(1, ValidationResult.BrokenRules.Count);
            Assert.Equal(PropertyName, ValidationResult.BrokenRules[0].Name);
        }
        [Fact]
        public void LessThanZero_adds_broken_rule_when_number_is_zero()
        {
            LessThanZero(0.0m, PropertyName);

            Assert.False(ValidationResult.IsValid);
            Assert.Equal(1, ValidationResult.BrokenRules.Count);
            Assert.Equal(PropertyName, ValidationResult.BrokenRules[0].Name);
        }
        [Fact]
        public void LessThanZero_doesnt_add_broken_rule_when_number_is_less_than_zero()
        {
            LessThanZero(-11.0m, PropertyName);

            Assert.True(ValidationResult.IsValid);
        }

        [Fact]
        public void NotZero_doesnt_add_broken_rule_when_number_is_more_than_zero()
        {
            NotZero(10.0m, PropertyName);

            Assert.True(ValidationResult.IsValid);
        }

        [Fact]
        public void NotZero_doesnt_add_broken_rule_when_number_is_less_then_zero()
        {
            NotZero(-10.0m, PropertyName);

            Assert.True(ValidationResult.IsValid);
        }

        [Fact]
        public void NotZero_adds_broken_rule_when_number_is_zero()
        {
            NotZero(0m, PropertyName);

            Assert.False(ValidationResult.IsValid);
            Assert.Equal(1, ValidationResult.BrokenRules.Count);
            Assert.Equal(PropertyName, ValidationResult.BrokenRules[0].Name);
        }

        [Fact]
        public void GreaterThanZero_adds_broken_rule_when_number_is_less_than_zero()
        {
            GreaterThanZero(-10.0m, PropertyName);

            Assert.False(ValidationResult.IsValid);
            Assert.Equal(1, ValidationResult.BrokenRules.Count);
            Assert.Equal(PropertyName, ValidationResult.BrokenRules[0].Name);
        }

        [Fact]
        public void GreaterThanZero_adds_broken_rule_when_number_is_zero()
        {
            GreaterThanZero(0.0m, PropertyName);

            Assert.False(ValidationResult.IsValid);
            Assert.Equal(1, ValidationResult.BrokenRules.Count);
            Assert.Equal(PropertyName, ValidationResult.BrokenRules[0].Name);
        }

        [Fact]
        public void GreaterThanZero_doesnt_add_broken_rule_when_number_is_less_than_zero()
        {
            GreaterThanZero(11.0m, PropertyName);

            Assert.True(ValidationResult.IsValid);
        }

        [Fact]
        public void NullOrNotDefault_decimal_adds_broken_rule_when_value_is_default_value()
        {
            NullOrNotDefault(0.0m, PropertyName);

            Assert.False(ValidationResult.IsValid);
            Assert.Equal(1, ValidationResult.BrokenRules.Count);
            Assert.Equal(PropertyName, ValidationResult.BrokenRules[0].Name);
        }


        [Fact]
        public void NullOrNotDefault_decimal_doesnt_add_broken_rule_when_value_is_null()
        {
            NullOrNotDefault((decimal?)null, PropertyName);

            Assert.True(ValidationResult.IsValid);
        }


        [Fact]
        public void NullOrNotDefault_decimal_doesnt_add_broken_rule_when_value_is_not_null_or_default()
        {
            NullOrNotDefault(11m, PropertyName);

            Assert.True(ValidationResult.IsValid);
        }


        [Fact]
        public void NullOrNotDefault_long_adds_broken_rule_when_value_is_default_value()
        {
            NullOrNotDefault((long?)0, PropertyName);

            Assert.False(ValidationResult.IsValid);
            Assert.Equal(1, ValidationResult.BrokenRules.Count);
            Assert.Equal(PropertyName, ValidationResult.BrokenRules[0].Name);
        }


        [Fact]
        public void NullOrNotDefault_long_doesnt_add_broken_rule_when_value_is_null()
        {
            NullOrNotDefault((long?)null, PropertyName);

            Assert.True(ValidationResult.IsValid);
        }


        [Fact]
        public void NullOrNotDefault_long_doesnt_add_broken_rule_when_value_is_not_null_or_default()
        {
            NullOrNotDefault((long)11000, PropertyName);

            Assert.True(ValidationResult.IsValid);
        }

        [Fact]
        public void NullOrNotDefault_int_adds_broken_rule_when_value_is_default_value()
        {
            NullOrNotDefault((int?)0, PropertyName);

            Assert.False(ValidationResult.IsValid);
            Assert.Equal(1, ValidationResult.BrokenRules.Count);
            Assert.Equal(PropertyName, ValidationResult.BrokenRules[0].Name);
        }


        [Fact]
        public void NullOrNotDefault_int_doesnt_add_broken_rule_when_value_is_null()
        {
            NullOrNotDefault((int?)null, PropertyName);

            Assert.True(ValidationResult.IsValid);
        }


        [Fact]
        public void NullOrNotDefault_int_doesnt_add_broken_rule_when_value_is_not_null_or_default()
        {
            NullOrNotDefault((int)11000, PropertyName);

            Assert.True(ValidationResult.IsValid);
        }


        [Fact]
        public void NullOrNotDefault_DateTime_adds_broken_rule_when_value_is_default_value()
        {
            NullOrNotDefault(new DateTime(), PropertyName);

            Assert.False(ValidationResult.IsValid);
            Assert.Equal(1, ValidationResult.BrokenRules.Count);
            Assert.Equal(PropertyName, ValidationResult.BrokenRules[0].Name);
        }


        [Fact]
        public void NullOrNotDefault_DateTime_doesnt_add_broken_rule_when_value_is_null()
        {
            NullOrNotDefault((DateTime?)null, PropertyName);

            Assert.True(ValidationResult.IsValid);
        }


        [Fact]
        public void NullOrNotDefault_DateTime_doesnt_add_broken_rule_when_value_is_not_null_or_default()
        {
            NullOrNotDefault(DateTime.Now, PropertyName);

            Assert.True(ValidationResult.IsValid);
        }

        [Fact]
        public void NotNullOrWhiteSpace_adds_broken_rule_when_string_is_null()
        {
            NotNullOrWhiteSpace(null, PropertyName);

            Assert.False(ValidationResult.IsValid);
            Assert.Equal(1, ValidationResult.BrokenRules.Count);
            Assert.Equal(PropertyName, ValidationResult.BrokenRules[0].Name);
        }

        [Fact]
        public void NotNullOrWhiteSpace_adds_broken_rule_when_string_is_empty_string()
        {
            NotNullOrWhiteSpace(String.Empty, PropertyName);

            Assert.False(ValidationResult.IsValid);
            Assert.Equal(1, ValidationResult.BrokenRules.Count);
            Assert.Equal(PropertyName, ValidationResult.BrokenRules[0].Name);
        }

        [Fact]
        public void NotNullOrWhiteSpace_adds_broken_rule_when_string_are_white_spaces()
        {
            NotNullOrWhiteSpace("    \n \t", PropertyName);

            Assert.False(ValidationResult.IsValid);
            Assert.Equal(1, ValidationResult.BrokenRules.Count);
            Assert.Equal(PropertyName, ValidationResult.BrokenRules[0].Name);
        }

        [Fact]
        public void NotNullOrWhiteSpace_doesnt_add_broken_rule_when_string_is_not_null_or_white_spaces()
        {
            NotNullOrWhiteSpace("abc", PropertyName);

            Assert.True(ValidationResult.IsValid);
        }

        [Fact]
        public void NotNullOrEmpty_doesnt_add_broken_rule_when_string_is_not_null_or_white_spaces()
        {
            NotNullOrEmpty("abc", PropertyName);

            Assert.True(ValidationResult.IsValid);
        }

        [Fact]
        public void NotNullOrEmpty_doesnt_add_broken_rule_when_string_are_white_spaces()
        {
            NotNullOrEmpty(" \n \t  ", PropertyName);

            Assert.True(ValidationResult.IsValid);
        }

        [Fact]
        public void NotNullOrEmpty_adds_broken_rule_when_string_is_null()
        {
            NotNullOrEmpty(null, PropertyName);

            Assert.False(ValidationResult.IsValid);
            Assert.Equal(1, ValidationResult.BrokenRules.Count);
            Assert.Equal(PropertyName, ValidationResult.BrokenRules[0].Name);
        }

        [Fact]
        public void NotNullOrEmpty_adds_broken_rule_when_string_is_empty_string()
        {
            NotNullOrEmpty(string.Empty, PropertyName);

            Assert.False(ValidationResult.IsValid);
            Assert.Equal(1, ValidationResult.BrokenRules.Count);
            Assert.Equal(PropertyName, ValidationResult.BrokenRules[0].Name);
        }

        [Fact]
        public void ValidCurrency_adds_broken_rule_if_string_is_not_three_caracters_long()
        {
            ValidCurrency("ABCD", PropertyName + "1");
            ValidCurrency("AB", PropertyName + "2");
            ValidCurrency("A", PropertyName + "3");

            Assert.False(ValidationResult.IsValid);
            Assert.Equal(3, ValidationResult.BrokenRules.Count);
            Assert.Equal(1, ValidationResult.BrokenRules.Count(x => x.Name == PropertyName + "1"));
            Assert.Equal(1, ValidationResult.BrokenRules.Count(x => x.Name == PropertyName + "2"));
            Assert.Equal(1, ValidationResult.BrokenRules.Count(x => x.Name == PropertyName + "3"));
        }

        [Fact]
        public void ValidCurrency_adds_broken_rule_if_string_is_three_caracters_long_but_contains_white_spaces()
        {
            ValidCurrency(" B ", PropertyName);

            Assert.False(ValidationResult.IsValid);
            Assert.Equal(1, ValidationResult.BrokenRules.Count);
            Assert.Equal(PropertyName, ValidationResult.BrokenRules[0].Name);
        }

        [Fact]
        public void ValidCurrency_adds_broken_rule_if_string_is_null_or_empty_string()
        {
            ValidCurrency(null, PropertyName + "1");
            ValidCurrency(String.Empty, PropertyName + "2");

            Assert.False(ValidationResult.IsValid);
            Assert.Equal(2, ValidationResult.BrokenRules.Count);
            Assert.Equal(1, ValidationResult.BrokenRules.Count(x => x.Name == PropertyName + "1"));
            Assert.Equal(1, ValidationResult.BrokenRules.Count(x => x.Name == PropertyName + "2"));
        }

        [Fact]
        public void ValidCurrency_doesnt_add_broken_rule_if_string_is_three_characters_long_without_white_spaces()
        {
            ValidCurrency("ABC", PropertyName);

            Assert.True(ValidationResult.IsValid);
        }

        [Fact]
        public void ValidMonth_adds_broker_rule_when_number_outside_of_1_12_range()
        {
            ValidMonth(-12, PropertyName + "1");
            ValidMonth(0, PropertyName + "2");
            ValidMonth(13, PropertyName + "3");

            Assert.False(ValidationResult.IsValid);
            Assert.Equal(3, ValidationResult.BrokenRules.Count);
            Assert.Equal(1, ValidationResult.BrokenRules.Count(x => x.Name == PropertyName + "1"));
            Assert.Equal(1, ValidationResult.BrokenRules.Count(x => x.Name == PropertyName + "2"));
            Assert.Equal(1, ValidationResult.BrokenRules.Count(x => x.Name == PropertyName + "3"));
        }

        [Fact]
        public void ValidMonth_doesnt_add_broker_rule_when_number_in_of_1_12_range()
        {
            ValidMonth(1, PropertyName + "1");
            ValidMonth(5, PropertyName + "2");
            ValidMonth(12, PropertyName + "3");

            Assert.True(ValidationResult.IsValid);
        }


        [Fact]
        public void ValidYear_adds_broker_rule_when_number_is_less_or_equal_zero()
        {
            ValidYear(-12, PropertyName + "1");
            ValidYear(0, PropertyName + "2");

            Assert.False(ValidationResult.IsValid);
            Assert.Equal(2, ValidationResult.BrokenRules.Count);
            Assert.Equal(1, ValidationResult.BrokenRules.Count(x => x.Name == PropertyName + "1"));
            Assert.Equal(1, ValidationResult.BrokenRules.Count(x => x.Name == PropertyName + "2"));
        }

        [Fact]
        public void ValidYear_doesnt_add_broker_rule_when_number_bigger_than_0()
        {
            ValidYear(1, PropertyName + "1");
            ValidYear(2001, PropertyName + "2");
            ValidYear(5000, PropertyName + "3");

            Assert.True(ValidationResult.IsValid);
        }

        [Fact]
        public void NotNullOrDefault_adds_broken_rule_if_guid_is_null_or_default_value()
        {
            NotNullOrDefault(null, PropertyName + "1");
            NotNullOrDefault(new Guid(), PropertyName + "2");

            Assert.False(ValidationResult.IsValid);
            Assert.Equal(2, ValidationResult.BrokenRules.Count);
            Assert.Equal(1, ValidationResult.BrokenRules.Count(x => x.Name == PropertyName + "1"));
            Assert.Equal(1, ValidationResult.BrokenRules.Count(x => x.Name == PropertyName + "2"));
        }


        [Fact]
        public void NotNullOrDefault_doesnt_add_broken_rule_if_guid_has_not_default_value()
        {
            NotNullOrDefault(Guid.NewGuid(), PropertyName);

            Assert.True(ValidationResult.IsValid);
        }

        [Fact]
        public void NullOrNotEmptyGuid_doesnt_add_broken_rule_if_guid_has_not_default_value()
        {
            NullOrNotEmptyGuid(Guid.NewGuid(), PropertyName);

            Assert.True(ValidationResult.IsValid);
        }

        [Fact]
        public void NullOrNotEmptyGuid_doesnt_add_broken_rule_if_guid_is_null()
        {
            NullOrNotEmptyGuid(null, PropertyName);

            Assert.True(ValidationResult.IsValid);
        }


        [Fact]
        public void NullOrNotEmptyGuid_adds_broken_rule_if_guid_has_default_value()
        {
            NullOrNotEmptyGuid(new Guid(), PropertyName);

            Assert.False(ValidationResult.IsValid);
            Assert.Equal(1, ValidationResult.BrokenRules.Count);
            Assert.Equal(PropertyName, ValidationResult.BrokenRules[0].Name);
        }

        [Fact]
        public void IsValid_external_validator_adds_broken_rule_with_nested_broken_rules_if_obj_is_not_valid()
        {
            var obj = new StubObject();
            IsValid<StubObject, StubObjectValidator>(obj, PropertyName);


            Assert.False(ValidationResult.IsValid);
            Assert.Equal(1, ValidationResult.BrokenRules.Count);
            Assert.Equal(PropertyName, ValidationResult.BrokenRules[0].Name);
            Assert.Equal(2, ValidationResult.BrokenRules[0].BrokenRules.Count);
            Assert.Equal(1, ValidationResult.BrokenRules[0].BrokenRules.Count(x => x.Name == nameof(obj.StringProperty)));
            Assert.Equal(1, ValidationResult.BrokenRules[0].BrokenRules.Count(x => x.Name == nameof(obj.IntProperty)));
        }

        [Fact]
        public void IsValid_external_validator_adds_broken_rule_when_object_is_null()
        {
            IsValid<StubObject, StubObjectValidator>(null, PropertyName);

            Assert.False(ValidationResult.IsValid);
            Assert.Equal(1, ValidationResult.BrokenRules.Count);
            Assert.Equal(PropertyName, ValidationResult.BrokenRules[0].Name);
        }

        [Fact]
        public void IsValid_external_validator_doesnt_add_broken_rule_when_object_is_valid()
        {
            var obj = new StubObject
            {
                IntProperty = 2,
                StringProperty = "abc",
                GuidProperty = Guid.NewGuid()
            };
            IsValid<StubObject, StubObjectValidator>(obj, PropertyName);

            Assert.True(ValidationResult.IsValid);
        }

        [Fact]
        public void IsValid_adds_broken_rule_with_nested_broken_rules_if_obj_is_not_valid()
        {
            StubObjectWithValidation = new StubObjectWithValidation();
            IsValid(StubObjectWithValidation, nameof(StubObjectWithValidation));

            Assert.False(ValidationResult.IsValid);
            Assert.Equal(1, ValidationResult.BrokenRules.Count);
            Assert.Equal(nameof(StubObjectWithValidation), ValidationResult.BrokenRules[0].Name);
            Assert.Equal(2, ValidationResult.BrokenRules[0].BrokenRules.Count);
            Assert.Equal(1, ValidationResult.BrokenRules[0].BrokenRules.Count(x => x.Name == nameof(StubObjectWithValidation.StringProperty)));
            Assert.Equal(1, ValidationResult.BrokenRules[0].BrokenRules.Count(x => x.Name == nameof(StubObjectWithValidation.IntProperty)));
        }

        [Fact]
        public void IsValid_adds_broken_rule_when_object_is_null()
        {
            IsValid(null, PropertyName);

            Assert.False(ValidationResult.IsValid);
            Assert.Equal(1, ValidationResult.BrokenRules.Count);
            Assert.Equal(PropertyName, ValidationResult.BrokenRules[0].Name);
        }

        [Fact]
        public void IsValid_doesnt_add_broken_rule_when_object_is_valid()
        {
            StubObjectWithValidation = new StubObjectWithValidation
            {
                IntProperty = 2,
                StringProperty = "abc",
                GuidProperty = Guid.NewGuid()
            };
            IsValid(StubObjectWithValidation, nameof(StubObjectWithValidation));

            Assert.True(ValidationResult.IsValid);
        }

        [Fact]
        public void AreValid_adds_broken_rule_with_nested_broken_rules_if_any_obj_is_not_valid()
        {
            StubObjecstWithValidations = new List<StubObjectWithValidation>
            {
                new StubObjectWithValidation(),
                new StubObjectWithValidation()
            };
            AreValid(StubObjecstWithValidations, nameof(StubObjecstWithValidations));

            Assert.False(ValidationResult.IsValid);
            Assert.Equal(1, ValidationResult.BrokenRules.Count);
            Assert.Equal(nameof(StubObjecstWithValidations), ValidationResult.BrokenRules[0].Name);
            Assert.Equal(2, ValidationResult.BrokenRules[0].BrokenRules.Count);
        }

        [Fact]
        public void AreValid_doesnt_add_broken_rule_when_list_is_null()
        {
            AreValid((IList<StubObjectWithValidation>)null, PropertyName);

            Assert.True(ValidationResult.IsValid);
        }

        [Fact]
        public void AreValid_doesnt_add_broken_rule_when_all_objects_are_valid()
        {
            StubObjecstWithValidations = new List<StubObjectWithValidation>
            {
                    new StubObjectWithValidation
                    {
                        IntProperty = 2,
                        StringProperty = "abc",
                        GuidProperty = Guid.NewGuid()
                    },
                    new StubObjectWithValidation
                    {
                        IntProperty = 2,
                        StringProperty = "abc",
                        GuidProperty = Guid.NewGuid()
                    }
            };

            AreValid(StubObjecstWithValidations, nameof(StubObjectWithValidation));

            Assert.True(ValidationResult.IsValid);
        }


        [Fact]
        public void AreValid_external_validator_adds_broken_rule_with_nested_broken_rules_if_any_obj_is_not_valid()
        {
            var objs = new List<StubObject>
            {
                new StubObject(),
                new StubObject()
            };
            AreValid<StubObject, StubObjectValidator>(objs, PropertyName);

            Assert.False(ValidationResult.IsValid);
            Assert.Equal(1, ValidationResult.BrokenRules.Count);
            Assert.Equal(PropertyName, ValidationResult.BrokenRules[0].Name);
            Assert.Equal(2, ValidationResult.BrokenRules[0].BrokenRules.Count);
        }

        [Fact]
        public void AreValid_external_validator_doesnt_add_broken_rule_when_list_is_null()
        {
            AreValid<StubObject, StubObjectValidator>(null, PropertyName);

            Assert.True(ValidationResult.IsValid);
        }

        [Fact]
        public void AreValid_external_validator_doesnt_add_broken_rule_when_all_objects_are_valid()
        {
            var objs = new List<StubObject>
            {
                new StubObject
                {
                    IntProperty = 2,
                    StringProperty = "abc",
                    GuidProperty = Guid.NewGuid()
                },
                new StubObject
                {
                    IntProperty = 2,
                    StringProperty = "abc",
                    GuidProperty = Guid.NewGuid()
                }
            };

            AreValid<StubObject, StubObjectValidator>(objs, nameof(PropertyName));

            Assert.True(ValidationResult.IsValid);
        }

        [Fact]
        public void NotEmpty_adds_broken_rule_when_list_is_null()
        {
            NotEmpty((IList<int>)null, PropertyName);


            Assert.False(ValidationResult.IsValid);
            Assert.Equal(1, ValidationResult.BrokenRules.Count);
            Assert.Equal(PropertyName, ValidationResult.BrokenRules[0].Name);
        }


        [Fact]
        public void NotEmpty_adds_broken_rule_when_list_is_empty()
        {
            NotEmpty(new List<int>(), PropertyName);


            Assert.False(ValidationResult.IsValid);
            Assert.Equal(1, ValidationResult.BrokenRules.Count);
            Assert.Equal(PropertyName, ValidationResult.BrokenRules[0].Name);
        }

        [Fact]
        public void NotEmpty_doesnt_add_broken_rule_when_list_is_not_empty()
        {
            NotEmpty(new List<int> { 1, 2, 3 }, PropertyName);


            Assert.True(ValidationResult.IsValid);
        }

        [Fact]
        public void AllTrue_returns_true_when_all_flags_are_true()
        {
            Assert.True(AllTrue(true, true, true, true));
        }

        [Fact]
        public void AllTrue_returns_false_when_even_one_flag_is_false()
        {
            Assert.False(AllTrue(true, true, false, true, true));
        }

        [Fact]
        public void AllTrue_returns_true_when_no_flags_passed()
        {
            Assert.True(AllTrue());
        }

        [Fact]
        public void AllFalse_returns_true_when_all_flags_are_false()
        {
            Assert.True(AllFalse(false, false, false));
        }

        [Fact]
        public void AllFalse_returns_false_when_even_one_flag_is_true()
        {
            Assert.False(AllFalse(false, false, true, false));
        }

        [Fact]
        public void AllFalse_returns_true_when_no_flags_passed()
        {
            Assert.True(AllFalse());
        }
    }
}
