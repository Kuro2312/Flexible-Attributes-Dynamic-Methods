using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlexibleUnit;

namespace SkillShop
{
    public class Rest : DynamicMethod
    {
        public string Name()
        {
            return "Rest";
        }

        public object Execute(object param, out bool isSuccessful)
        {
            isSuccessful = true;
            return "You used Rest! It'll recover your strength!";
        }
    }
}
