using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlexibleUnit
{
    public class AttributeManager
    {
        private Dictionary<string, FlexibleAttribute> _attributes;

        public AttributeManager()
        {
            _attributes = new Dictionary<string, FlexibleAttribute>();
        }

        public AttributeManager(List<FlexibleAttribute> attributes)
        {
            _attributes = new Dictionary<string, FlexibleAttribute>();

            for (int i = 0; i < attributes.Count; i++)
            {
                string attributeName = attributes[i].Name;
                if (_attributes.ContainsKey(attributeName))
                    _attributes[attributeName] = attributes[i];
                else
                    _attributes.Add(attributeName, attributes[i]);
            }
        }

        public string GetAtttributeValue(string attributeName)
        {
            if (_attributes.ContainsKey(attributeName))
                return _attributes[attributeName].Value;

            return "";
        }

        public bool SetAttributeValue(string attributeName, string newvalue, string type)
        {
            if (_attributes.ContainsKey(attributeName))
            {
                if (_attributes[attributeName].Equals(type))
                {
                    _attributes[attributeName].Value = newvalue;
                    return true;
                }
            }

            return false;
        }

        public bool AddAttribute(FlexibleAttribute newAttribute)
        {
            if (_attributes.ContainsKey(newAttribute.Name))
            {
                _attributes[newAttribute.Name] = newAttribute;
                return true;
            }

            _attributes.Add(newAttribute.Name, newAttribute);
            return true;
        }

        public bool RemoveAttribute(string attributeName)
        {
            if (_attributes.ContainsKey(attributeName))
            {
                _attributes.Remove(attributeName);
                return true;
            }

            return false;
        }

        public string this[string attributeName]
        {
            get
            {
                return GetAtttributeValue(attributeName);
            }
        }
    }
}
