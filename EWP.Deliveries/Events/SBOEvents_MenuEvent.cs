using EWP.Extensions;
using SAPbouiCOM.Framework;
using System;

namespace EWP.Events
{
    partial class SBOEvents
    {
        public static void SBO_Application_MenuEvent(ref SAPbouiCOM.MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (pVal.BeforeAction)
            {
                #region MENU SAP BUSINESS ONE [ADD-ON]
                try
                {
                    if (pVal.IsUserForm())
                    {
                        Application.SBO_Application.OpenUserForm(pVal.GetFormType());
                    }
                }
                catch (Exception Ex)
                {
                    SboStatusBar.SetTextAsError(Ex.Message);
                }
                #endregion
            }
        }
    }
}
