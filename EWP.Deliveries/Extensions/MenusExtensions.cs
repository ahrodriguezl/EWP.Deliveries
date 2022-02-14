using SAPbouiCOM.Framework;
using System;
using System.IO;

namespace EWP.Extensions
{
    static class MenusExtensions
    {
        private static bool Exist(string MenuUID)
        {
            return Application.SBO_Application.Menus.Exists(MenuUID);
        }

        public static SAPbouiCOM.MenuItem AddSimpleMenuItem<T>(this SAPbouiCOM.MenuItem oMenuItem, int Position = -1)
        {
            var formType = SboFramework.GetAttribute<FormTypeAttribute>(typeof(T));
            if (formType == null)
                throw new Exception("'FormTypeAttribute' has not been defined in the '" + typeof(T).FullName + "' class.");

            var simpleMenu = SboFramework.GetAttribute<SimpleMenuAttribute>(typeof(T));
            if (simpleMenu == null)
                throw new Exception("'SimpleMenuAttribute' has not been defined in the '" + typeof(T).FullName + "' class."); ;

            var oMenus = Application.SBO_Application.Menus.Item(oMenuItem.UID).SubMenus;

            SAPbouiCOM.MenuItem oItem = null;

            if (!Exist(simpleMenu.UniqueID))
            {
                oItem = oMenus.Add(simpleMenu.UniqueID, simpleMenu.Caption, SAPbouiCOM.BoMenuType.mt_STRING, Position == -1 ? oMenus.Count + 1 : Position);
            }
            else
            {
                oItem = oMenus.Item(simpleMenu.UniqueID);
            }

            oItem.String = simpleMenu.Caption;
            oItem.Image = !string.IsNullOrEmpty(simpleMenu.Image) ? System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, simpleMenu.Image) : string.Empty;

            MenuEventExtensions.Add(simpleMenu.UniqueID, typeof(T));

            return oItem;
        }

        public static SAPbouiCOM.MenuItem AddPopMenuItem(this SAPbouiCOM.MenuItem oMenuItem, string UniqueID, string Caption, string Image = null, int Position = -1)
        {
            var oMenus = Application.SBO_Application.Menus.Item(oMenuItem.UID).SubMenus;

            SAPbouiCOM.MenuItem oItem = null;

            if (!Exist(UniqueID))
            {
                oItem = oMenus.Add(UniqueID, Caption, SAPbouiCOM.BoMenuType.mt_POPUP, Position == -1 ? oMenus.Count + 1 : Position);
            }
            else
            {
                oItem = oMenus.Item(UniqueID);
            }

            oItem.String = Caption;
            oItem.Image = GetImagePath(Image);

            return oItem;
        }

        private static string GetImagePath(string image)
        {
            if (string.IsNullOrEmpty(image))
                return string.Empty;

            string filename = Path.Combine(System.Windows.Forms.Application.StartupPath,"Icons",  image);

            if(!File.Exists(filename))
                return string.Empty;

            return filename;
        }
    }
}
