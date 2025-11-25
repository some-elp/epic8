using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epic8.Calcs
{
    public enum Element { Fire, Ice, Earth, Light, Dark}
    public enum ElementalAdvantage { Advantage, Neutral, Disadvantage}
    public static class ElementHelper
    {
        //Do we have elemental advantage or not?
        public static ElementalAdvantage GetElementalAdvantage(Element user, Element target)
        {
            if ((user == Element.Fire && target == Element.Earth) ||
               (user == Element.Ice && target == Element.Fire) ||
               (user == Element.Earth && target == Element.Ice) ||
               (user == Element.Light && target == Element.Dark) ||
               (user == Element.Dark && target == Element.Light))
                return ElementalAdvantage.Advantage;

            if ((user == Element.Fire && target == Element.Ice) ||
               (user == Element.Ice && target == Element.Earth) ||
               (user == Element.Earth && target == Element.Fire))
                return ElementalAdvantage.Disadvantage;
            return ElementalAdvantage.Neutral;
        }

        //If we have elemental advantage, we deal 10% more damage.
        public static float GetEleAdvantageMultiplier(ElementalAdvantage advantage)
        {
            if (advantage == ElementalAdvantage.Advantage)
                return 1.1f;
            return 1.0f;
        }
    }
}
