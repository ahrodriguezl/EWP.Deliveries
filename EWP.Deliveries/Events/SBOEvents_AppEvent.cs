namespace EWP.Events
{
    partial class SBOEvents
    {
        public static void SBO_Application_AppEvent(SAPbouiCOM.BoAppEventTypes EventType)
        {
            switch (EventType)
            {
                case SAPbouiCOM.BoAppEventTypes.aet_ShutDown:
                    System.Windows.Forms.Application.Exit();
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_CompanyChanged:
                    System.Windows.Forms.Application.Exit();
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_FontChanged:
                    System.Windows.Forms.Application.Restart();
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_LanguageChanged:
                    System.Windows.Forms.Application.Restart();
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_ServerTerminition:
                    System.Windows.Forms.Application.Exit();
                    break;
            }
        }
    }
}
