using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlexibleUnit;

namespace SkillShop
{
    public class Sleep : DynamicMethod
    {
        public string Name()
        {
            return "Sleep";
        }

        public object Execute(object param, out bool isSuccessful)
        {
            isSuccessful = true;
            return "You used Sleep! Good Night! ^_^ !";
        }
    }
}
