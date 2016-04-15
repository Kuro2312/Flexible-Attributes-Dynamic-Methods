using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlexibleUnit;

namespace SkillShop
{
    public class Yawn : DynamicMethod
    {
        public string Name()
        {
            return "Yawn";
        }

        public object Execute(object param, out bool isSuccessful)
        {
            isSuccessful = true;
            return "You used Yawn! It make enemy sleepy!";
        }
    }
}
