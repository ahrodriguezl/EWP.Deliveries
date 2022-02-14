using System;

namespace EWP.Extensions
{
    static class FormsExtensions
    {
        public static string GetUserFormType<T>(this SAPbouiCOM.Forms form)
        {
            Type type = typeof(T);
            return form.GetUserFormType(type);
        }

        public static string GetUserFormType(this SAPbouiCOM.Forms form, Type type)
        {
            var formTypeAttr = SboFramework.GetAttribute<FormTypeAttribute>(type);
            if (formTypeAttr == null)
                throw new Exception("FormTypeAttribute has not been defined.");

            return formTypeAttr.Name;
        }
    }
}