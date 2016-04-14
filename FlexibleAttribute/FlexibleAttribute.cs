using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlexibleUnit
{
    public class FlexibleAttribute
    {
        private string _attributeName;
        private string _attributeValue;
        private string _type;
        public bool _isValid = true;

        public FlexibleAttribute(string name, string value, string type)
        {
            Name = name;
            Value = value;
            Type = type;
        }

        public string Name
        {
            get
            {
                return _attributeName;
            }

            set
            {
                _attributeName = value;
            }
        }

        public string Value
        {
            get
            {
                return _attributeValue;
            }

            set
            {
                this._attributeValue = value;
            }
        }

        public string Type
        {
            get
            {
                return _type;
            }

            set
            {
                _type = value;
            }
        }
    }
}
