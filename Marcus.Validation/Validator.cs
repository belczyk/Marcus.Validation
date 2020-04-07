using System;
using System.Collections.Generic;
using System.Linq;

namespace Marcus.Validation
{
    public abstract class Validator<T> : Validator
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

    public abstract class Validator
    {
        protected ValidationResult ValidationResult { get; } = new ValidationResult();

        protected virtual Validator ValueIn<T>(T obj, IList<T> allowedValues, string name)
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

            return this;
        }

        protected virtual Validator ValuesIn<T>(IList<T> objList, IList<T> allowedValues, string name)
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

            return this;
        }

        protected virtual Validator NotNull(object value, string name)
        {
            if (value != null) return this;
            ValidationResult.AddRule(name, $"{name} can't be null");

            return this;
        }

        protected virtual Validator IsTrue(bool value, string name, string description = null)
        {
            if (!value)
                ValidationResult.AddRule(name, description ?? $"{name} can't be false");

            return this;
        }

        protected virtual Validator IsFalse(bool value, string name, string description = null)
        {
            if (value)
                ValidationResult.AddRule(name, description ?? $"{name} can't be true");

            return this;
        }

        protected virtual Validator AreNotDefault(IList<Guid> guids, string name)
        {
            if (guids == null || !guids.Any()) return this;

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

            return this;
        }

        protected virtual Validator GreaterThanZero(decimal value, string name, string description = null)
        {
            if (value <= 0) ValidationResult.AddRule(name, description ?? $"{name} must be greater than zero");
            return this;
        }

        protected virtual Validator LessThanZero(decimal value, string name, string description = null)
        {
            if (value >= 0) ValidationResult.AddRule(name, description ?? $"{name} must be less than zero");
            return this;
        }

        protected virtual Validator NotZero(decimal value, string name)
        {
            if (value == 0) ValidationResult.AddRule(name, $"{name} can't be equal to zero");
            return this;
        }

        protected virtual Validator PositiveNumber(decimal value, string name)
        {
            if (value < 0) ValidationResult.AddRule(name, $"{name} must be greater than zero");
            return this;
        }

        protected virtual Validator GreaterThanZero(int value, string name, string description = null)
        {
            if (value <= 0) ValidationResult.AddRule(name, description ?? $"{name} must be greater than zero");
            return this;
        }

        protected virtual Validator LessThanZero(int value, string name, string description = null)
        {
            if (value >= 0) ValidationResult.AddRule(name, description ?? $"{name} must be less than zero");
            return this;
        }

        protected virtual Validator NotZero(int value, string name)
        {
            if (value == 0) ValidationResult.AddRule(name, $"{name} can't be equal to zero");
            return this;
        }

        protected virtual Validator PositiveNumber(int value, string name)
        {
            if (value < 0) ValidationResult.AddRule(name, $"{name} must be greater than zero");
            return this;
        }

        protected virtual Validator GreaterThanZero(float value, string name, string description = null)
        {
            if (value <= 0) ValidationResult.AddRule(name, description ?? $"{name} must be greater than zero");
            return this;
        }

        protected virtual Validator LessThanZero(float value, string name, string description = null)
        {
            if (value >= 0) ValidationResult.AddRule(name, description ?? $"{name} must be less than zero");
            return this;
        }

        protected virtual Validator NotZero(float value, string name)
        {
            if (value == 0) ValidationResult.AddRule(name, $"{name} can't be equal to zero");
            return this;
        }

        protected virtual Validator PositiveNumber(float value, string name)
        {
            if (value < 0) ValidationResult.AddRule(name, $"{name} must be greater than zero");
            return this;
        }

        protected virtual Validator GreaterThanZero(double value, string name, string description = null)
        {
            if (value <= 0) ValidationResult.AddRule(name, description ?? $"{name} must be greater than zero");
            return this;
        }

        protected virtual Validator LessThanZero(double value, string name, string description = null)
        {
            if (value >= 0) ValidationResult.AddRule(name, description ?? $"{name} must be less than zero");
            return this;
        }

        protected virtual Validator NotZero(double value, string name)
        {
            if (value == 0) ValidationResult.AddRule(name, $"{name} can't be equal to zero");
            return this;
        }

        protected virtual Validator PositiveNumber(double value, string name)
        {
            if (value < 0) ValidationResult.AddRule(name, $"{name} must be greater than zero");
            return this;
        }

        protected virtual Validator NotDefault<T>(T value, string name)
        {
            var def = default(T);

            if (EqualityComparer<T>.Default.Equals(value, def))
            {
                var defValueStr = def == null ? "null" : def.ToString();
                ValidationResult.AddRule(name, $"{name} can't have default value ({defValueStr})");
            }
            return this;
        }

        protected virtual Validator NotNullOrDefault(Guid? value, string name, string description = null)
        {
            if (value == null || value.Value == default(Guid))
                ValidationResult.AddRule(name, description ?? $"{name} can't be null or default(Guid).");
            return this;
        }

        protected virtual Validator NullOrNotEmptyGuid(Guid? value, string name)
        {
            if (value == null) return this;

            if (value == Guid.Empty)
            {
                ValidationResult.AddRule(name,
                    $"If {name} is not null, it can't be equal to Guid.Empty ({Guid.Empty}).");
            }
            return this;
        }

        protected virtual Validator NullOrNotDefault(long? value, string name, string description = null)
        {
            if (value == null) return this;

            if (value == default(long))
                ValidationResult.AddRule(name,
                    description ?? $"If {name} is not null, it can't be equal to 0.");
            return this;
        }


        protected virtual Validator NullOrNotDefault(decimal? value, string name, string description = null)
        {
            if (value == null) return this;

            if (value == default(decimal))
                ValidationResult.AddRule(name,
                    description ?? $"If {name} is not null, it can't be equal to 0.");
            return this;
        }

        protected virtual Validator NullOrNotDefault(int? value, string name, string description = null)
        {
            if (value == null) return this;

            if (value == default(int))
                ValidationResult.AddRule(name,
                    description ?? $"If {name} is not null, it can't be equal to 0.");
            return this;
        }

        protected virtual Validator NullOrNotDefault(DateTime? value, string name, string description = null)
        {
            if (value == null) return this;

            if (value == default(DateTime))
                ValidationResult.AddRule(name,
                   description ?? $"If {name} is not null, it can't be equal to default(DateTime) ({default(DateTime)}).");
            return this;
        }
        protected virtual Validator NotNullOrWhiteSpace(string value, string name)
        {
            if (string.IsNullOrWhiteSpace(value))
                ValidationResult.AddRule(name, $"{name} can't be null or white space");
            return this;
        }

        protected virtual Validator NotNullOrEmpty(string value, string name)
        {
            if (string.IsNullOrEmpty(value))
                ValidationResult.AddRule(name, $"{name} can't be null or empty");
            return this;
        }

        protected virtual Validator ValidCurrency(string value, string name = "Currency")
        {
            if (!(value != null && value.Length == 3 && value.Trim().Length == 3))
                ValidationResult.AddRule(name, $"{name} must be valid currency code");
            return this;
        }

        protected virtual Validator ValidMonth(int value, string name = "Month")
        {
            if (value <= 0 || value > 12) ValidationResult.AddRule(name, $"{name} must be  valid month");
            return this;
        }

        protected virtual Validator ValidYear(int value, string name = "Year")
        {
            if (value <= 0) ValidationResult.AddRule(name, $"{name} must be a valid year");
            return this;
        }

        protected virtual Validator IsValid<T, TValidator>(T obj, string name) where TValidator : Validator<T>, new()
        {
            if (obj == null)
            {
                ValidationResult.AddRule(name, $"{name} can't be null and must be valid");
                return this;
            }

            var validator = (TValidator)Activator.CreateInstance(typeof(TValidator));
            var result = validator.Validate(obj);

            if (result.IsNotValid) ValidationResult.AddRule(name, $"{name} must be valid", result.BrokenRules);
            return this;
        }

        protected virtual Validator IsValid(ObjectWithValidation obj, string name)
        {
            if (obj == null)
            {
                ValidationResult.AddRule(name, $"{name} can't be null and must be valid");
                return this;
            }
            var result = obj.Validate();

            if (result.IsNotValid)
            {
                if (result.IsNotValid) ValidationResult.AddRule(name, $"{name} must be valid", result.BrokenRules);
            }
            return this;
        }

        protected virtual Validator AreValid<T, TValidator>(IList<T> objList, string name) where TValidator : Validator<T>, new()
        {
            if (objList == null || !objList.Any()) return this;

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
            return this;
        }

        protected virtual Validator AreValid<T>(IList<T> objList, string name) where T : ObjectWithValidation
        {
            if (objList == null || !objList.Any()) return this;

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
            return this;
        }

        protected virtual Validator NotEmpty<T>(IList<T> objList, string name)
        {
            if (objList == null || !objList.Any())
                ValidationResult.AddRule(name, $"{name} can't be null or empty list");
            return this;
        }

        protected virtual bool AllTrue(params bool[] flags)
        {
            return flags.All(x => x);
        }
        protected virtual bool AllFalse(params bool[] flags)
        {
            return flags.All(x => !x);
        }
    }
}