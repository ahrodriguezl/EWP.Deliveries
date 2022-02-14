using SAPbouiCOM.Framework;
using EWP.Extensions;
using System;
using System.Collections.Generic;

namespace EWP.Forms
{
    [FormAttribute("EWP.Forms.frmSboDeliveries", "Forms/frmSboDeliveries.b1f")]
    [FormType("IWP_FT_DLVRS")]
    [SimpleMenu("SM_DLVRS", "Create Deliveries")]
    class frmSboDeliveries : UserFormBase
    {
        private SAPbouiCOM.StaticText lStep01;
        private SAPbouiCOM.StaticText lCardCode;
        private SAPbouiCOM.EditText etCardCode;
        private SAPbouiCOM.EditText etCardName;
        private SAPbouiCOM.LinkedButton lkCardCode;
        private SAPbouiCOM.StaticText lDocDate;
        private SAPbouiCOM.StaticText lFromDate;
        private SAPbouiCOM.EditText etFromDate;
        private SAPbouiCOM.StaticText lToDate;
        private SAPbouiCOM.EditText etToDate;
        private SAPbouiCOM.Button bFind;
        private SAPbouiCOM.Grid gOrders;
        private SAPbouiCOM.Button bNext;
        private SAPbouiCOM.Button bCancel;

        private int startIndex = -1;
        private int endIndex = -1;

        public frmSboDeliveries()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.lStep01 = ((SAPbouiCOM.StaticText)(this.GetItem("lStep01").Specific));
            this.lCardCode = ((SAPbouiCOM.StaticText)(this.GetItem("lCardCode").Specific));
            this.etCardCode = ((SAPbouiCOM.EditText)(this.GetItem("etCardCode").Specific));
            this.etCardCode.ValidateAfter += new SAPbouiCOM._IEditTextEvents_ValidateAfterEventHandler(this.etCardCode_ValidateAfter);
            this.etCardCode.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.etCardCode_ChooseFromListAfter);
            this.etCardName = ((SAPbouiCOM.EditText)(this.GetItem("etCardName").Specific));
            this.lkCardCode = ((SAPbouiCOM.LinkedButton)(this.GetItem("lkCardCode").Specific));
            this.bFind = ((SAPbouiCOM.Button)(this.GetItem("bFind").Specific));
            this.bFind.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.bFind_PressedAfter);
            this.gOrders = ((SAPbouiCOM.Grid)(this.GetItem("gOrders").Specific));
            this.gOrders.ClickBefore += new SAPbouiCOM._IGridEvents_ClickBeforeEventHandler(this.gOrders_ClickBefore);
            this.gOrders.PressedAfter += new SAPbouiCOM._IGridEvents_PressedAfterEventHandler(this.gOrders_PressedAfter);
            this.bNext = ((SAPbouiCOM.Button)(this.GetItem("bCreate").Specific));
            this.bNext.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.bCreate_PressedAfter);
            this.bCancel = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            this.lDocDate = ((SAPbouiCOM.StaticText)(this.GetItem("lDueDate").Specific));
            this.lFromDate = ((SAPbouiCOM.StaticText)(this.GetItem("lFromDate").Specific));
            this.etFromDate = ((SAPbouiCOM.EditText)(this.GetItem("etFromDate").Specific));
            this.etFromDate.ValidateAfter += new SAPbouiCOM._IEditTextEvents_ValidateAfterEventHandler(this.etFromDate_ValidateAfter);
            this.lToDate = ((SAPbouiCOM.StaticText)(this.GetItem("lToDate").Specific));
            this.etToDate = ((SAPbouiCOM.EditText)(this.GetItem("etToDate").Specific));
            this.etToDate.ValidateAfter += new SAPbouiCOM._IEditTextEvents_ValidateAfterEventHandler(this.etToDate_ValidateAfter);
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.ResizeAfter += new ResizeAfterHandler(this.Form_ResizeAfter);
        }

        private void OnCustomInitialize()
        {
            try
            {
                this.UIAPIRawForm.State = SAPbouiCOM.BoFormStateEnum.fs_Restore;
                this.UIAPIRawForm.Height = 340;
                this.UIAPIRawForm.Width = 540;
                this.UIAPIRawForm.Top = Application.SBO_Application.Desktop.Height / 2 - this.UIAPIRawForm.Height / 2;
                this.UIAPIRawForm.Left = Application.SBO_Application.Desktop.Width / 2 - this.UIAPIRawForm.Width / 2;

                var tsTitle = SAPbouiCOM.BoTextStyle.ts_BOLD | SAPbouiCOM.BoTextStyle.ts_UNDERLINE;
                lStep01.Item.TextStyle = (int)tsTitle;

                var cfl = this.UIAPIRawForm.ChooseFromLists.Item("cflCstmr");
                cfl.SetCondition("CardType", "C");

                //var now = DateTime.Now;
                //var uds = this.UIAPIRawForm.DataSources.UserDataSources;
                //uds.Item("udFromDate").Value = now.AddDays(-7).ToString("yyyyMMdd");
                //uds.Item("udToDate").Value = now.ToString("yyyyMMdd");
            }
            catch (Exception ex)
            {
                SboStatusBar.SetTextAsError(ex);
            }
        }

        private void Form_ResizeAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
            lFromDate.Item.Left = 150;
            etFromDate.Item.Left = 205;
            lToDate.Item.Left = 290;
            etToDate.Item.Left = 345;
        }

        private void etCardCode_ChooseFromListAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                this.UIAPIRawForm.Freeze(true);

                var values = pVal.GetSelectedValues("CardCode", "CardName");
                if (values.Count == 0)
                    return;

                var uds = this.UIAPIRawForm.DataSources.UserDataSources;
                uds.Item("udCardCode").Value = values["CardCode"];
                uds.Item("udCardName").Value = values["CardName"];
            }
            catch (Exception ex)
            {
                SboStatusBar.SetTextAsError(ex);
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        private void etCardCode_ValidateAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            if (string.IsNullOrEmpty(etCardCode.Value))
                etCardName.Value = string.Empty;
        }

        private void etFromDate_ValidateAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                this.UIAPIRawForm.Freeze(true);

                var uds = this.UIAPIRawForm.DataSources.UserDataSources;
                var dsSD = uds.Item("udFromDate");
                var startDate = dsSD.Value.Trim();

                var dsED = uds.Item("udToDate");
                var endDate = dsED.Value.Trim();

                if (!string.IsNullOrEmpty(startDate))
                {
                    if (string.IsNullOrEmpty(endDate))
                    {
                        dsED.Value = dsSD.Value;
                    }
                    else
                    {
                        var dt1 = Convert.ToDateTime(dsSD.Value.Trim());
                        var dt2 = Convert.ToDateTime(dsED.Value.Trim());

                        if (dt1 > dt2)
                            dsED.Value = dsSD.Value;
                    }
                }
                else
                {
                    dsED.Value = "";
                }
            }
            catch (Exception ex)
            {
                SboStatusBar.SetTextAsError(ex);
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        private void etToDate_ValidateAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                this.UIAPIRawForm.Freeze(true);

                var uds = this.UIAPIRawForm.DataSources.UserDataSources;
                var dsSD = uds.Item("udToDate");
                var startDate = dsSD.Value.Trim();

                var dsED = uds.Item("udFromDate");
                var endDate = dsED.Value.Trim();

                if (!string.IsNullOrEmpty(startDate))
                {
                    if (string.IsNullOrEmpty(endDate))
                    {
                        dsED.Value = dsSD.Value;
                    }
                    else
                    {
                        var dt1 = Convert.ToDateTime(dsSD.Value.Trim());
                        var dt2 = Convert.ToDateTime(dsED.Value.Trim());

                        if (dt2 > dt1)
                            dsED.Value = dsSD.Value;
                    }
                }
                else
                {
                    dsED.Value = "";
                }
            }
            catch (Exception ex)
            {
                SboStatusBar.SetTextAsError(ex);
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        private void bFind_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                this.UIAPIRawForm.Freeze(true);

                GetSalesOrders();
            }
            catch (Exception ex)
            {
                SboStatusBar.SetTextAsError(ex);
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        private void gOrders_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = pVal.Row != -1;
        }

        private void gOrders_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                this.UIAPIRawForm.Freeze(true);

                if (pVal.Row >= 0 && pVal.ColUID == "Checked")
                {
                    if (pVal.Modifiers == SAPbouiCOM.BoModifiersEnum.mt_None)
                    {
                        startIndex = pVal.Row;
                        endIndex = -1;

                        SetSelectedRow(startIndex);
                    }
                    else if (pVal.Modifiers == SAPbouiCOM.BoModifiersEnum.mt_SHIFT)
                    {
                        if (startIndex == -1)
                            startIndex = pVal.Row;

                        endIndex = pVal.Row;

                        int first = Math.Min(startIndex, endIndex);
                        int last = Math.Max(startIndex, endIndex);

                        for (int i = first; i <= last; i++)
                            SetSelectedRow(i);
                    }
                }
            }
            catch (Exception ex)
            {
                SboStatusBar.SetTextAsError(ex);
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        private void bCreate_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                this.UIAPIRawForm.Freeze(true);

                if (!gOrders.HasSelectedRows())
                {
                    SboStatusBar.SetTextAsError("You must select at least one sales order.");
                    return;
                }

                List<string> orders = new List<string>();
                List<string> partners = new List<string>();
                for (int i = 0; i < gOrders.SelectedRowsCount(); i++)
                {
                    int rowIndex = gOrders.Rows.SelectedRows.Item(i, SAPbouiCOM.BoOrderType.ot_RowOrder);
                    string partner = gOrders.DataTable.GetValue("CardCode", rowIndex).ToString().Trim();
                    string docEntry = gOrders.DataTable.GetValue("DocEntry", rowIndex).ToString().Trim();

                    if (!partners.Contains(partner))
                        partners.Add(partner);

                    orders.Add(docEntry);
                }

                if (partners.Count > 1)
                {
                    SboStatusBar.SetTextAsError("You can't select sales orders from different customers.");
                    return;
                }

                Application.SBO_Application.OpenUserForm<frmSboDeliveryItems>();
                frmSboDeliveryItems.GetItems(partners[0], orders.ToArray());
            }
            catch (Exception ex)
            {
                SboStatusBar.SetTextAsError(ex);
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        private void GetSalesOrders()
        {
            string cardCode = etCardCode.Value.Trim();
            string startDate = etFromDate.Value.Trim();
            string endDate = etToDate.Value.Trim();

            string sQuery = DbHelper.Instance.GetOrders(cardCode, startDate, endDate);

            gOrders.DataTable.ExecuteQuery(sQuery);
            gOrders.SetCheckBoxColumn("Checked", "Select", true);
            gOrders.SetEditTextColumn("DocEntry", "Document Entry", false, "17");
            gOrders.SetEditTextColumn("DocNum", "Document Number", false);
            gOrders.SetEditTextColumn("DocDueDate", "Delivery date", false);
            gOrders.SetEditTextColumn("CardCode", "Customer", false, "2");
            gOrders.SetEditTextColumn("CardName", "Name", false);
            gOrders.SetEditTextColumn("DocTotal", "Document total", false);
            gOrders.SetEditTextColumn("DocCur", "Currency", false);

            gOrders.AutoResizeColumns();
        }

        private void SetSelectedRow(int rowIndex)
        {
            var dt = gOrders.DataTable;
            bool selected = dt.GetValue("Checked", rowIndex).ToString() == "Y";
            if (selected)
            {
                gOrders.Rows.SelectedRows.Add(rowIndex);
            }
            else
            {
                gOrders.Rows.SelectedRows.Remove(rowIndex);
            }
        }

        public static void Search()
        {
            var formType = SboFramework.GetAttribute<FormTypeAttribute>(typeof(frmSboDeliveries));
            var oForm = Application.SBO_Application.Forms.Item(formType.Name);

            try
            {
                oForm.Freeze(true);
                var bFind = oForm.Items.Item<SAPbouiCOM.Button>("bFind");
                bFind.Item.Click();
            }
            catch (Exception ex)
            {
                SboStatusBar.SetTextAsError(ex);
            }
            finally
            {
                oForm.Freeze(false);
            }
        }

        private string StringToDate(string date)
        {
            try
            {
                SAPbobsCOM.SBObob oSBObob = (SAPbobsCOM.SBObob)SboClient.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoBridge);
                SAPbobsCOM.Recordset oRecordset = oSBObob.Format_StringToDate(date);
                oRecordset.MoveFirst();

                if (oRecordset.RecordCount > 0)
                    return oRecordset.Fields.Item(0).Value.ToString();
            }
            catch
            {
                throw;
            }

            return string.Empty;
        }
    }
}
