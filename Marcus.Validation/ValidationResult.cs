using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Marcus.Validation
{
    public class ValidationResult
    {
        private const string ObjectIsValid = "Object is valid";
        public bool IsValid => BrokenRules == null || !BrokenRules.Any();
        public bool IsNotValid => !IsValid;
        public IList<Rule> BrokenRules { get; } = new List<Rule>();

        public void AddRule(string name, string description)
        {
            BrokenRules.Add(new Rule(name, description));
        }

        public void AddRule(string name, string description, IList<Rule> brokenRules)
        {
            BrokenRules.Add(new Rule(name, description, brokenRules));
        }

        public void AddRule(Rule rule)
        {
            BrokenRules.Add(rule);
        }

        public override string ToString()
        {
            if (IsValid) return ObjectIsValid;

            var sb = new StringBuilder("Invalid object. Broken rules: \n");
            sb.AppendLine("\t" + BrokenRulesToString(BrokenRules).Replace("\n", "\n\t"));

            return sb.ToString();
        }

        private string BrokenRulesToString(IList<Rule> rules)
        {
            var sb = new StringBuilder();
            var maxNameLength = rules.Max(x => x.Name.Length);
            foreach (var rule in rules)
            {
                sb.Append($"[{rule.Name.PadLeft(maxNameLength)}]: {rule.Description}");
                if (rule.BrokenRules != null && rule.BrokenRules.Any())
                {
                    sb.AppendLine(" Broken Rules:");
                    var subRules = "\t" + BrokenRulesToString(rule.BrokenRules).Replace("\n", "\n\t");
                    sb.AppendLine(subRules);
                }
                else
                {
                    sb.AppendLine();
                }
            }

            return sb.ToString();
        }

        public void ThrowIfInvalid()
        {
            if (IsNotValid) throw new InvalidObjectException(this);
        }

        public bool HasBrokenRule(string ruleName)
        {
            return HasBrokenRule(BrokenRules, ruleName);
        }

        private bool HasBrokenRule(IList<Rule> rules, string ruleName)
        {
            foreach (var rule in rules)
            {
                if (rule.Name == ruleName || HasBrokenRule(rule.BrokenRules, ruleName)) return true;
            }

            return false;
        }
    }
}