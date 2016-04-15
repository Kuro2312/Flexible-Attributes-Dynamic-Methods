using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlexibleUnit;

namespace GameUnit
{
    public class Character
    {
        private string _name;
        private MethodManager _methodManager;
        private AttributeManager _attributeManager;

        public string[] Stats
        {
            get
            {
                return _attributeManager.Attributes.Keys.ToArray();
            }
        }

        public string[] Skills
        {
            get
            {
                return _methodManager.Methods.Keys.ToArray();
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }

        public Character()
        {
            _methodManager = new MethodManager();
            _attributeManager = new AttributeManager();

            _attributeManager.AddAttribute(new FlexibleAttribute("HP", "10", "int"));
            _attributeManager.AddAttribute(new FlexibleAttribute("ATK", "10", "int"));
            _attributeManager.AddAttribute(new FlexibleAttribute("DEF", "10", "int"));
        }

        public string this[string attributeName]
        {
            get
            {
                return _attributeManager[attributeName];
            }

            set
            {
                _attributeManager.SetAttributeValue(attributeName, value, "int");
            }
        }

        public object ExecuteMethod(string name, object param, out bool isSuccessful)
        {
            return _methodManager.ExecuteMethod(name, param, out isSuccessful);
        }

        public void AddNewStat(string statName)
        {
            _attributeManager.AddAttribute(new FlexibleAttribute(statName, "0", "int"));
        }

        public void AddNewSkill(DynamicMethod dynamicMethod)
        {
            if (!_methodManager.Methods.ContainsKey(dynamicMethod.Name()))
                _methodManager.AddMethod(dynamicMethod);
        }

        public List<String> GetAllSkills()
        {
            return _methodManager.Methods.Keys.ToList();
        }
    }
}
