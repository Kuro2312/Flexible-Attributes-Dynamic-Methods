using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlexibleUnit
{
    public interface DynamicMethod
    {
        string Name();
        object Execute(object param, out bool isSuccessful);
    }
}
