using System;
using System.Collections.Generic;
using System.Linq;

namespace Marcus.Validation
{
    public abstract class ValidatorBase
    {
        protected ValidationResult ValidationResult { get; } = new ValidationResult();

        protected void ValueIn<T>(T obj, IList<T> allowedValues, string name)
        {
            if (allowedValues == null)
            {
                throw new Exception("AllowedValues list is required for this value");
            }

            if (!allowedValues.Any(x => EqualityComparer<T>.Default.Equals(x, obj)))
            {
                var val = obj == null ? "NULL" : obj.ToString();
                var allowed = $"[{allowedValues.Aggregate("", (c, n) => c + ";" + n)}]";
                ValidationResult.AddRule(name, $"{name} has not allowed value. {name} value: {val}. Allowed values: {allowed}");
            }
        }

        protected void ValuesIn<T>(IList<T> objList, IList<T> allowedValues, string name)
        {
            if (allowedValues == null)
            {
                throw new Exception("AllowedValues list is required for this value");
            }
            var allowed = $"[{allowedValues.Aggregate("", (c, n) => c + ";" + n)}]";

            var rule = new Rule(name, $"All elements in {name} must have value from allowed values. Allowed values: {allowed}");
            var i = 0;
            foreach (var obj in objList)
            {
                if (!allowedValues.Any(x => EqualityComparer<T>.Default.Equals(x, obj)))
                {
                    var val = obj == null ? "NULL" : obj.ToString();
                    rule.BrokenRules.Add(new Rule($"{name}[{i}]", $"Element must have allowed value. {name} value: {val} Allowed values: {allowed}", new List<Rule>()));
                }

                i++;
            }

            if (rule.BrokenRules.Any())
            {
                ValidationResult.BrokenRules.Add(rule);
            }
        }

        protected void NotNull(object value, string name)
        {
            if (value != null) return;
            ValidationResult.AddRule(name, $"{name} can't be null");
        }

        protected void IsTrue(bool value, string name, string description = null)
        {
            if (!value)
                ValidationResult.AddRule(name, description ?? $"{name} can't be false");
        }

        protected void IsFalse(bool value, string name, string description = null)
        {
            if (value)
                ValidationResult.AddRule(name, description ?? $"{name} can't be true");
        }


        protected void AreNotDefault(IList<Guid> guids, string name)
        {
            if (guids == null || !guids.Any()) return;

            var rule = new Rule(name, $"None element in {name} can be default.");
            var i = 0;
            foreach (var guid in guids)
            {
                if (guid == default(Guid))
                    rule.BrokenRules.Add(new Rule($"{name}[{i}]",
                        $"Element can't have default value ({default(Guid)})."));
                i++;
            }

            if (rule.BrokenRules.Any()) ValidationResult.AddRule(rule);
        }

        protected void GreaterThanZero(decimal value, string name, string description = null)
        {
            if (value <= 0) ValidationResult.AddRule(name, description ?? $"{name} must be greater than zero");
        }

        protected void LessThanZero(decimal value, string name, string description = null)
        {
            if (value >= 0) ValidationResult.AddRule(name, description ?? $"{name} must be less than zero");
        }

        protected void NotZero(decimal value, string name)
        {
            if (value == 0) ValidationResult.AddRule(name, $"{name} can't be equal to zero");
        }

        protected void PositiveNumber(decimal value, string name)
        {
            if (value < 0) ValidationResult.AddRule(name, $"{name} must be greater than zero");
        }

        protected void NotDefault<T>(T value, string name)
        {
            var def = default(T);

            if (EqualityComparer<T>.Default.Equals(value, def))
            {
                var defValueStr = def == null ? "null" : def.ToString();
                ValidationResult.AddRule(name, $"{name} can't have default value ({defValueStr})");
            }
        }

        protected void NotNullOrDefault(Guid? value, string name, string description = null)
        {
            if (value == null || value.Value == default(Guid))
                ValidationResult.AddRule(name, description ?? $"{name} can't be null or default(Guid).");
        }

        protected void NullOrNotEmptyGuid(Guid? value, string name)
        {
            if (value == null) return;

            if (value == Guid.Empty)
            {
                ValidationResult.AddRule(name,
                    $"If {name} is not null, it can't be equal to Guid.Empty ({Guid.Empty}).");
            }
        }

        protected void NullOrNotDefault(long? value, string name, string description = null)
        {
            if (value == null) return;

            if (value == default(long))
                ValidationResult.AddRule(name,
                    description ?? $"If {name} is not null, it can't be equal to 0.");
        }


        protected void NullOrNotDefault(decimal? value, string name, string description = null)
        {
            if (value == null) return;

            if (value == default(decimal))
                ValidationResult.AddRule(name,
                    description ?? $"If {name} is not null, it can't be equal to 0.");
        }

        protected void NullOrNotDefault(int? value, string name, string description = null)
        {
            if (value == null) return;

            if (value == default(int))
                ValidationResult.AddRule(name,
                    description ?? $"If {name} is not null, it can't be equal to 0.");
        }

        protected void NullOrNotDefault(DateTime? value, string name, string description = null)
        {
            if (value == null) return;

            if (value == default(DateTime))
                ValidationResult.AddRule(name,
                   description ?? $"If {name} is not null, it can't be equal to default(DateTime) ({default(DateTime)}).");
        }

        protected void NotNullOrWhiteSpace(string value, string name)
        {
            if (string.IsNullOrWhiteSpace(value))
                ValidationResult.AddRule(name, $"{name} can't be null or white space");
        }

        protected void NotNullOrEmpty(string value, string name)
        {
            if (string.IsNullOrEmpty(value))
                ValidationResult.AddRule(name, $"{name} can't be null or empty");
        }

        protected void ValidCurrency(string value, string name = "Currency")
        {
            if (!(value != null && value.Length == 3 && value.Trim().Length == 3))
                ValidationResult.AddRule(name, $"{name} must be valid currency code");
        }

        protected void ValidMonth(int value, string name = "Month")
        {
            if (value <= 0 || value > 12) ValidationResult.AddRule(name, $"{name} must be  valid month");
        }

        protected void ValidYear(int value, string name = "Year")
        {
            if (value <= 0) ValidationResult.AddRule(name, $"{name} must be a valid year");
        }

        protected void IsValid<T, TValidator>(T obj, string name) where TValidator : Validator<T>, new()
        {
            if (obj == null)
            {
                ValidationResult.AddRule(name, $"{name} can't be null and must be valid");
                return;
            }

            var validator = (TValidator)Activator.CreateInstance(typeof(TValidator));
            var result = validator.Validate(obj);

            if (result.IsNotValid) ValidationResult.AddRule(name, $"{name} must be valid", result.BrokenRules);
        }

        protected void IsValid(ObjectWithValidation obj, string name)
        {
            if (obj == null)
            {
                ValidationResult.AddRule(name, $"{name} can't be null and must be valid");
                return;
            }
            var result = obj.Validate();

            if (result.IsNotValid)
            {
                if (result.IsNotValid) ValidationResult.AddRule(name, $"{name} must be valid", result.BrokenRules);
            }
        }

        protected void AreValid<T, TValidator>(IList<T> objList, string name) where TValidator : Validator<T>, new()
        {
            if (objList == null || !objList.Any()) return;

            var rule = new Rule(name, $"All elements in {name} must be valid.");
            var i = 0;
            foreach (var obj in objList)
            {
                var validator = (TValidator)Activator.CreateInstance(typeof(TValidator));
                var result = validator.Validate(obj);
                if (result.IsNotValid)
                    rule.BrokenRules.Add(new Rule($"{name}[{i}]", $"Element must be valid", result.BrokenRules));
                i++;
            }

            if (rule.BrokenRules.Any()) ValidationResult.AddRule(rule);
        }

        protected void AreValid<T>(IList<T> objList, string name) where T : ObjectWithValidation
        {
            if (objList == null || !objList.Any()) return;

            var rule = new Rule(name, $"All elements in {name} must be valid.");
            var i = 0;
            foreach (var obj in objList)
            {
                var result = obj.Validate();
                if (result.IsNotValid)
                    rule.BrokenRules.Add(new Rule($"{name}[{i}]", $"Element must be valid", result.BrokenRules));
                i++;
            }

            if (rule.BrokenRules.Any()) ValidationResult.AddRule(rule);
        }

        protected void NotEmpty<T>(IList<T> objList, string name)
        {
            if (objList == null || !objList.Any())
                ValidationResult.AddRule(name, $"{name} can't be null or empty list");
        }

        protected bool AllTrue(params bool[] flags)
        {
            return flags.All(x => x);
        }

        protected bool AllFalse(params bool[] flags)
        {
            return flags.All(x => !x);
        }
    }
}