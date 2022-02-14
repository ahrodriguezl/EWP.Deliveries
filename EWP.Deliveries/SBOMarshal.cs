using SAPbouiCOM.Framework;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace EWP
{
    public static class SboMarshal
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int SetProcessWorkingSetSize(IntPtr process, int minimumWorkingSetSize, int maximumWorkingSetSize);

        public static void ReleaseMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
        }

        public static void ReleaseComObject(object obj)
        {
            try
            {
                if (obj == null)
                    return;

                Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                Application.SBO_Application.MessageBox("Object COM couldn't released. " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}
