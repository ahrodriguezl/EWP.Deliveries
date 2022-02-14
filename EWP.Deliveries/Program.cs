using EWP.Events;
using SAPbouiCOM.Framework;
using System;

namespace EWP
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                Application _application = args.Length == 0 ? new Application() : new Application(args[0]);

                SboInit.MainMenu();

                _application.RegisterMenuEventHandler(SBOEvents.SBO_Application_MenuEvent);
                Application.SBO_Application.AppEvent += SBOEvents.SBO_Application_AppEvent;
                SboStatusBar.SetTextAsSuccess("Deliveries AddOn: Started successfully!!!.");

                _application.Run();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
    }
}
