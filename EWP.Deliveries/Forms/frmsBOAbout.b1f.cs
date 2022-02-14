using SAPbouiCOM.Framework;

namespace EWP.Forms
{
    [FormAttribute("EWP.Forms.frmSboAbout", "Forms/frmSboAbout.b1f")]
    [FormType("IWP_FT_ABOUT")]
    [SimpleMenu("SM_ABOUT", "About...")]
    class frmSboAbout : UserFormBase
    {
        private SAPbouiCOM.StaticText lSAP;
        private SAPbouiCOM.StaticText lSQL;
        private SAPbouiCOM.StaticText lTitle;
        private SAPbouiCOM.Button bAction;
        private SAPbouiCOM.StaticText lDvlpr;
        private SAPbouiCOM.StaticText lEmail;
        private SAPbouiCOM.StaticText lInfo;
        private SAPbouiCOM.StaticText lVS;

        public frmSboAbout()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.lSAP = ((SAPbouiCOM.StaticText)(this.GetItem("lSAP").Specific));
            this.lSQL = ((SAPbouiCOM.StaticText)(this.GetItem("lSQL").Specific));
            this.lTitle = ((SAPbouiCOM.StaticText)(this.GetItem("lTitle").Specific));
            this.bAction = ((SAPbouiCOM.Button)(this.GetItem("bAction").Specific));
            this.bAction.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.bAction_PressedAfter);
            this.lDvlpr = ((SAPbouiCOM.StaticText)(this.GetItem("lDvlpr").Specific));
            this.lEmail = ((SAPbouiCOM.StaticText)(this.GetItem("lEmail").Specific));
            this.lInfo = ((SAPbouiCOM.StaticText)(this.GetItem("lInfo").Specific));
            this.lVS = ((SAPbouiCOM.StaticText)(this.GetItem("lVS").Specific));
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
        }

        private void OnCustomInitialize()
        {
            this.UIAPIRawForm.Top = Application.SBO_Application.Desktop.Height / 2 - this.UIAPIRawForm.Height / 2;
            this.UIAPIRawForm.Left = Application.SBO_Application.Desktop.Width / 2 - this.UIAPIRawForm.Width / 2;

            var tsTitle = SAPbouiCOM.BoTextStyle.ts_BOLD | SAPbouiCOM.BoTextStyle.ts_UNDERLINE;
            lTitle.Item.TextStyle = (int)tsTitle;
            lInfo.Item.TextStyle = (int)tsTitle;
        }

        private void bAction_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            this.UIAPIRawForm.Close();
        }
    }
}
