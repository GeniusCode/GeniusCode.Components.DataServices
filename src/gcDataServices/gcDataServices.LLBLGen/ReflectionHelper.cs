using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GeniusCode.Components.DataServices
{
    public static class ReflectionHelper
    {

        private const BindingFlags DefaultFlags = BindingFlags.Default | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

        public static void SetPropertyValue(string propertyName, object obj, object value, Type typeToUse = null)
        {
            var success = TrySetPropertyValue(propertyName, obj, value, typeToUse);

            if(!success)
                throw new Exception("Property value was not set successfully via reflection.  The property was not detected.");
        }

        public static bool TrySetPropertyValue(string propertyName, object obj, object value, Type typeToUse = null)
        {
            var type = typeToUse ?? obj.GetType();

            var lmb = type.GetProperty(propertyName, DefaultFlags | BindingFlags.SetProperty);
            if (lmb != null)
            {
                
                lmb.SetValue(obj,value,null);
                return true;
            }

            return false;
        }


    }
}
