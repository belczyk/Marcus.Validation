using System.Collections.Generic;

namespace Marcus.Validation
{
    public class Rule
    {
        public Rule(string name, string description)
        {
            Name = name ?? string.Empty;
            Description = description;
        }


        public Rule(string name, string description, IList<Rule> brokenRules)
        {
            Name = name ?? string.Empty;
            Description = description;
            BrokenRules = brokenRules;
        }

        public string Name { get; }
        public string Description { get; set; }
        public IList<Rule> BrokenRules { get; set; } = new List<Rule>();
    }
}