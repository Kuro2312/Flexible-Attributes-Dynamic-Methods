using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlexibleUnit;

namespace SkillShop
{
    public class Flamethrower : DynamicMethod
    {
        public string Name()
        {
            return "Flamethrower";
        }

        public object Execute(object param, out bool isSuccessful)
        {
            isSuccessful = true;
            return "You used Flamethrower! It's super attack!";
        }
    }
}
