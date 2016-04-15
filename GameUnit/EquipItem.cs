using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlexibleUnit;

namespace GameUnit
{
    public class EquipItem : DynamicMethod
    {
        public string Name()
        {
            return "Equip Game Item";
        }

        public object Execute(object param, out bool isSuccessful)
        {
            try
            {
                List<object> data = param as List<object>;

                Character character = data[0] as Character;
                ItemUnit item = data[1] as ItemUnit;

                foreach (string key in item.Attributes.Attributes.Keys)
                    if (key != "Name")
                    {
                        if (character[key] == "")
                            character.AddNewStat(key);
                        int stat = Convert.ToInt32(character[key]);
                        int itemStat = Convert.ToInt32(item[key]);
                        character[key] = (stat + itemStat).ToString();
                    }

                isSuccessful = true;
                return null;
            }
            catch (Exception e)
            {
                isSuccessful = false;
                return null;
            }
        }
    }
}
