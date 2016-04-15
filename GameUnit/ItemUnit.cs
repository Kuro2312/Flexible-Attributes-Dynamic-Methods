using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlexibleUnit;
using System.IO;
using System.Reflection;

namespace GameUnit
{
    public class ItemUnit
    {
        // Phần biến toàn cục
        protected static Dictionary<string, ItemUnit> _unitCatalog = new Dictionary<string, ItemUnit>();
        protected static Dictionary<string, DynamicMethod> _unitMethod = new Dictionary<string, DynamicMethod>();
        protected static int _nextAvailableID = 1;
        protected static string _dataPath = "Item.dat";

        // Phần biến cần của 1 Game Unit
        private MethodManager _methodManager;
        private AttributeManager _attributeManager;
        protected string _classType;
        protected int _id;

        public static string[] Units
        {
            get
            {
                return _unitCatalog.Keys.ToArray();
            }
        }

        public static string GetUnitTypeFromName(string unitName)
        {
            if (_unitCatalog.ContainsKey(unitName))
                return _unitCatalog[unitName].ClassType;

            return "Unit";
        }

        public string ClassType
        {
            get
            {
                return _classType;
            }
        }

        public AttributeManager Attributes
        {
            get
            {
                return _attributeManager;
            }
        }

        public MethodManager Methods
        {
            get
            {
                return _methodManager;
            }
        }

        protected ItemUnit(string classType)
        {
            _id = _nextAvailableID++;
            _classType = classType;
            _methodManager = new MethodManager();
            _attributeManager = new AttributeManager();
        }

        protected ItemUnit(ItemUnit unit)
        {
            _id = _nextAvailableID++;
            _classType = unit.ClassType;
            _methodManager.CopyFrom(unit.Methods);
            _attributeManager.CopyFrom(unit.Attributes);
        }

        public string this[string key]
        {
            get
            {
                return _attributeManager.GetAtttributeValue(key);
            }
            set
            {
                _attributeManager.SetAttributeValue(key, value, "");
            }

        }

        public static bool Initialize()
        {
            _unitCatalog = new Dictionary<string, ItemUnit>();
            LoadMethodUnit();

            LoadUnitFromFile(_dataPath);

            return true;
        }

        public static ItemUnit CreateUnit(string UnitTypeName)
        {
            ItemUnit unit = _unitCatalog[UnitTypeName];
            ItemUnit clone = null;

            if (unit != null)
                clone = new ItemUnit(unit);

            return clone;
        }

        protected static bool LoadUnitFromFile(string fileName)
        {
            try
            {
                string url = Directory.GetCurrentDirectory() + "\\";

                using (StreamReader sr = new StreamReader(fileName))
                {
                    while (!sr.EndOfStream)
                    {
                        string[] unitData =  sr.ReadLine().Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);

                        ItemUnit unit = new ItemUnit(unitData[0]);
                        unit.Attributes.AddAttribute(new FlexibleAttribute("Name", unitData[1] , "string"));

                        string[] unitStat = unitData[2].Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < unitStat.Length; i = i + 2)
                            unit.Attributes.AddAttribute(new FlexibleAttribute(unitStat[i], unitStat[i + 1], "int"));

                        string[] unitMethod = unitData[3].Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < unitMethod.Length; i++)
                        {
                            if (_unitMethod.ContainsKey(unitMethod[i]))
                                unit.Methods.AddMethod(_unitMethod[unitMethod[i]]);
                        }

                        _unitCatalog.Add(unit["Name"], unit); 
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        protected static void LoadMethodUnit()
        {
            _unitMethod.Add("Equip", new EquipItem());
            _unitMethod.Add("Remove", new RemoveItem());
            _unitMethod.Add("Use", new EquipItem());
        }

        public static object Execute(string unitName, string unitMethod, object param, out bool isSuccessful)
        {
            isSuccessful = false;
            if (!_unitCatalog.ContainsKey(unitName))
                return null;

            ItemUnit unit = _unitCatalog[unitName];

            unitMethod = NormalizeMethodName(unitMethod);
            if (!unit.Methods.Methods.ContainsKey(unitMethod + " Game Item"))
                return null;

            List<object> p = new List<object>();
            p.Add(param);
            p.Add(unit);

            return unit.Methods.ExecuteMethod(unitMethod + " Game Item", p, out isSuccessful);
        }

        public static string NormalizeMethodName(string methodName)
        {
            if (methodName == "Use")
                return "Equip";

            return methodName;
        }
    }
}
