using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlexibleUnit
{
    public class MethodManager
    {
        private Dictionary<string, DynamicMethod> _methods;

        public MethodManager()
        {
            _methods = new Dictionary<string, DynamicMethod>();
        }

        public MethodManager(List<DynamicMethod> methods)
        {
            _methods = new Dictionary<string, DynamicMethod>();

            for (int i = 0; i < _methods.Count; i++)
            {
                string methodName = methods[i].Name();
                if (_methods.ContainsKey(methodName))
                    _methods[methodName] = methods[i];
                else
                    _methods.Add(methodName, methods[i]);
            }
        }

        public bool AddMethod(DynamicMethod newMethod)
        {
            if (_methods.ContainsKey(newMethod.Name()))
                return false;

            _methods.Add(newMethod.Name(), newMethod);
            return true;
        }

        public bool OverrideMethod(DynamicMethod newMethod)
        {
            if (!_methods.ContainsKey(newMethod.Name()))
                return false;

            _methods[newMethod.Name()] = newMethod;
            return true;
        }

        public bool RemoveMethod(string methodName)
        {
            if (!_methods.ContainsKey(methodName))
                return false;

            _methods.Remove(methodName);
            return true;
        }

        public DynamicMethod GetMethod(string methodName)
        {
            if (!_methods.ContainsKey(methodName))
                return null;

            return _methods[methodName];
        }

        public object ExecuteMethod(string methodName, object param, out bool isSuccessful)
        {
            DynamicMethod method = GetMethod(methodName);
            isSuccessful = false;

            if (method == null)
                return null;

            return method.Execute(param, out isSuccessful);
        }

        public DynamicMethod this[string methodName]
        {
            get
            {
                return GetMethod(methodName);
            }
        }
    }
}
