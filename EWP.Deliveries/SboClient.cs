namespace EWP
{
    public sealed class SboClient
    {
        private SAPbobsCOM.Company _company = null;
        public string lErrMsg;
        public int lErrCode;
        public int lRetCode;

        public string NewObjectKey;
        public string NewObjectType;

        public SAPbobsCOM.Company Company
        {
            get { return _company; }
            set { _company = value; }
        }

        public void GetLastError()
        {
            _company.GetLastError(out lErrCode, out lErrMsg);
        }

        public void GetNewObject()
        {
            NewObjectKey = _company.GetNewObjectKey();
            NewObjectType = _company.GetNewObjectType();
        }

        private static readonly SboClient _instance = new SboClient();

        private SboClient()
        {
            _company = (SAPbobsCOM.Company)SAPbouiCOM.Framework.Application.SBO_Application.Company.GetDICompany();
        }

        public static SboClient Instance
        {
            get { return _instance; }
        }
    }
}