using System;
using System.Collections.Generic;

namespace EWP.Extensions
{
    static class SBOItemEventArgExtensions
    {
        public static Dictionary<string, string> GetSelectedValues(this SAPbouiCOM.SBOItemEventArg pVal, params string[] columns)
        {
            try
            {
                Dictionary<string, string> values = new Dictionary<string, string>();

                SAPbouiCOM.ISBOChooseFromListEventArg chflarg = (SAPbouiCOM.ISBOChooseFromListEventArg)pVal;
                SAPbouiCOM.DataTable dt = chflarg.SelectedObjects;

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (var p in columns)
                        values[p] = dt.GetValue(p, 0).ToString().Trim();
                }

                return values;
            }
            catch (Exception ex)
            {
               SboStatusBar.SetTextAsError(ex.Message);
                return new Dictionary<string, string>();
            }
        }
    }
}
