using System;
using System.Collections.Generic;

namespace EWP.Extensions
{
    static class MenuEventExtensions
    {
        private static Dictionary<string, Type> userForms = new Dictionary<string, Type>();

        public static void Add(string menuUID, Type formType)
        {
            userForms[menuUID] = formType;
        }

        public static bool IsUserForm(this SAPbouiCOM.MenuEvent pVal)
        {
            return userForms.ContainsKey(pVal.MenuUID);
        }

        public static Type GetFormType(this SAPbouiCOM.MenuEvent pVal)
        {
            if (!userForms.ContainsKey(pVal.MenuUID))
                return null;

            return userForms[pVal.MenuUID];
        }
    }
}
