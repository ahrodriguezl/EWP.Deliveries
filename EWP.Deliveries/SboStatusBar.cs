using SAPbouiCOM.Framework;
using System;

namespace EWP
{
    static class SboStatusBar
    {
        private static string FormaString(string Text, object[] args)
        {
            return args.Length == 0 ? Text : string.Format(Text, args);
        }

        public static SAPbouiCOM.ProgressBar CreateProgressBar(string text, int max, bool stoppable)
        {
            return Application.SBO_Application.StatusBar.CreateProgressBar(text, max, stoppable);
        }

        public static void SetTextAsWarning(string Text)
        {
            Application.SBO_Application.StatusBar.SetText(Text, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
        }

        public static void SetTextAsWarning(string Text, params object[] args)
        {
            SetTextAsWarning(FormaString(Text, args));
        }

        public static void SetTextAsError(SboException ex)
        {
            SetTextAsError("An error has been ocurred. {0} [{1}]", ex.ErrorMessage, ex.ErrorCode);
            SboMessage.Send("Error: Deliveries AddOn", ex);
        }

        public static void SetTextAsError(Exception ex)
        {
            SetTextAsError("An error has been ocurred. {0}", ex.Message);
            SboMessage.Send("Error: Deliveries AddOn", ex);
        }

        public static void SetTextAsError(string Text)
        {
            Application.SBO_Application.StatusBar.SetText(Text, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
        }

        public static void SetTextAsError(string Text, params object[] args)
        {
            SetTextAsError(FormaString(Text, args));
        }

        public static void SetTextAsError(int ErrorCode, string ErrorMessage)
        {
            Application.SBO_Application.StatusBar.SetSystemMessage(ErrorMessage, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error, "", "", "", ErrorCode);
        }

        public static void SetTextAsSuccess(string Text)
        {
            Application.SBO_Application.StatusBar.SetText(Text, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
        }

        public static void SetTextAsSuccess(string Text, params object[] args)
        {
            SetTextAsSuccess(FormaString(Text, args));
        }

        public static void Clear()
        {
            Application.SBO_Application.StatusBar.SetText("", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_None);
        }
    }
}
