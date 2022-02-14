using EWP.Extensions;
using EWP.Forms;
using SAPbouiCOM.Framework;

namespace EWP
{
    public sealed class SboInit
    {
        public static void MainMenu()
        {
            var mainMenu = Application.SBO_Application.Menus.Item("43520");
            var addonMenu = mainMenu.AddPopMenuItem("M_IWP", "Deliveries AddOn", "ic_logo.png");

            addonMenu.AddSimpleMenuItem<frmSboDeliveries>();
            addonMenu.AddSimpleMenuItem<frmSboAbout>();
        }
    }
}