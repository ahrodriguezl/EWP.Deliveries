using System;

namespace EWP
{
    class SboException : Exception
    {
        private int errCode;
        private string errMsg;
        private string customMsg;

        public int ErrorCode { get { return errCode; } }
        public string ErrorMessage { get { return string.IsNullOrEmpty(customMsg) ? errMsg : string.Format("{0}. {1}", customMsg, errMsg); } }

        public SboException()
        {
            GetLastError();
        }

        public SboException(string message, params string[] args)
            : base(message)
        {
            GetLastError(message, args);
        }

        public SboException(string message, Exception exception)
            : base(message, exception)
        {
            GetLastError();
        }

        public SboException(int ErrorCode, string ErrorMessage)
        {
            errCode = ErrorCode;
            errMsg = ErrorMessage;
        }

        private void GetLastError(string message = null, params string[] args)
        {
            SboClient.Instance.GetLastError();
            errCode = SboClient.Instance.lErrCode;
            errMsg = SboClient.Instance.lErrMsg;

            if (message != null)
                customMsg = args.Length == 0 ? message : string.Format(message, args);
        }
    }
}
