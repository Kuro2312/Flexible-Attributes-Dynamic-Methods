using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlexibleUnit;

namespace SkillShop
{
    public class SelfDestructcs : DynamicMethod
    {
        public string Name()
        {
            return "SelfDestructcs";
        }

        public object Execute(object param, out bool isSuccessful)
        {
            isSuccessful = true;
            return "You used SelfDestructcs! Good bye my friend! +_+ !";
        }
    }
}
