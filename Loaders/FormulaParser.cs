using epic8.Skills;
using epic8.Units;
using NCalc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Loaders
{
    public static class FormulaParser
    {
        public static Func<SkillContext, Character, float> Compile(string expression)
        {
            //Should be an NCalc expression of the string
            var exp = new Expression(expression);

            //returns a function so I don't need to change anything else.
            return (ctx, t) =>
            {
                exp.Parameters["target_hp"] = t.GetEffectiveStats().Hp;

                var result = exp.Evaluate();
                return Convert.ToSingle(result);
            };

        }
    }
}
