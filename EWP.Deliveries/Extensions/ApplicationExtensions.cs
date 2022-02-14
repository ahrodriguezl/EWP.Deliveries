using SAPbouiCOM.Framework;
using System;

namespace EWP.Extensions
{
    static class ApplicationExtensions
    {
        public static void OpenUserForm<T>(this SAPbouiCOM.Application application) where T : new()
        {
            application.OpenUserForm(typeof(T));
        }

        public static void OpenUserForm(this SAPbouiCOM.Application application, Type userForm)
        {
            string FormType = Application.SBO_Application.Forms.GetUserFormType(userForm);

            try
            {
                Application.SBO_Application.Forms.Item(FormType).Visible = true;
            }
            catch
            {
                try
                {
                    dynamic tmp = Activator.CreateInstance(userForm);
                    tmp.Show();
                }
                catch (Exception ex)
                {
                   SboStatusBar.SetTextAsError("Couldn't be open specified form. {0}", ex.Message);
                }
            }
        }
    }
}