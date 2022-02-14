using EWP.Extensions;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;

namespace EWP.Forms
{
    [FormAttribute("EWP.Forms.frmSboDeliveryItems", "Forms/frmSboDeliveryItems.b1f")]
    [FormType("IWP_FT_DLVRYITM")]
    class frmSboDeliveryItems : UserFormBase
    {
        private SAPbouiCOM.StaticText lStep;
        private SAPbouiCOM.Grid gItems;
        private SAPbouiCOM.Button bCreate;
        private SAPbouiCOM.Button bCancel;
        private SAPbouiCOM.Button bAdd;
        private SAPbouiCOM.Button bDel;

        public frmSboDeliveryItems()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.lStep = ((SAPbouiCOM.StaticText)(this.GetItem("lStep").Specific));
            this.gItems = ((SAPbouiCOM.Grid)(this.GetItem("gItems").Specific));
            this.gItems.ClickBefore += new SAPbouiCOM._IGridEvents_ClickBeforeEventHandler(this.gItems_ClickBefore);
            this.gItems.ChooseFromListAfter += new SAPbouiCOM._IGridEvents_ChooseFromListAfterEventHandler(this.gItems_ChooseFromListAfter);
            this.bCreate = ((SAPbouiCOM.Button)(this.GetItem("bCreate").Specific));
            this.bCreate.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.bCreate_PressedAfter);
            this.bCancel = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            this.bAdd = ((SAPbouiCOM.Button)(this.GetItem("bAdd").Specific));
            this.bAdd.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.bAdd_PressedAfter);
            this.bDel = ((SAPbouiCOM.Button)(this.GetItem("bDel").Specific));
            this.bDel.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.bDel_PressedAfter);
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
            try
            {
                this.UIAPIRawForm.State = SAPbouiCOM.BoFormStateEnum.fs_Restore;
                this.UIAPIRawForm.Height = 340;
                this.UIAPIRawForm.Width = 540;
                this.UIAPIRawForm.Top = Application.SBO_Application.Desktop.Height / 2 - this.UIAPIRawForm.Height / 2;
                this.UIAPIRawForm.Left = Application.SBO_Application.Desktop.Width / 2 - this.UIAPIRawForm.Width / 2;

                var tsTitle = SAPbouiCOM.BoTextStyle.ts_BOLD | SAPbouiCOM.BoTextStyle.ts_UNDERLINE;
                lStep.Item.TextStyle = (int)tsTitle;

                var cflItems = this.UIAPIRawForm.ChooseFromLists.Item("cflItems");
                cflItems.SetCondition("SellItem", "Y");

                var cflTaxes = this.UIAPIRawForm.ChooseFromLists.Item("cflTaxes");
                cflTaxes.SetCondition("ValidForAR", "Y");
            }
            catch (Exception ex)
            {
                SboStatusBar.SetTextAsError(ex);
            }
        }

        private void gItems_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (pVal.Row == -1)
            {
                BubbleEvent = false;
                return;
            }

            try
            {
                this.UIAPIRawForm.Freeze(true);

                if (pVal.ColUID != "RowsHeader")
                {
                    var selectedRows = gItems.Rows.SelectedRows;
                    selectedRows.Clear();
                    selectedRows.Add(pVal.Row);
                }

                if (pVal.ColUID == "ItemCode")
                {
                    int docEntry = Convert.ToInt32(gItems.GetSelectedValue("DocEntry"));
                    if (docEntry != 0)
                    {
                        BubbleEvent = false;
                        gItems.SetCellFocus(pVal.Row, gItems.Columns.Count - 1);
                        SboStatusBar.SetTextAsError("Item No. can't be modified if line is based in a sales order.");
                    }
                }
                else
                {
                    List<string> columns = new List<string>() { "WhsCode", "Currency", "Price", "RcptQty" };
                    if (!columns.Contains(pVal.ColUID))
                        gItems.SetCellFocus(pVal.Row, gItems.Columns.Count - 1);
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

        private void gItems_ChooseFromListAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                this.UIAPIRawForm.Freeze(true);

                string aliasID = null;
                switch (pVal.ColUID)
                {
                    case "ItemCode": aliasID = "ItemCode"; break;
                    case "WhsCode": aliasID = "WhsCode"; break;
                    case "Currency": aliasID = "CurrCode"; break;
                    case "TaxCode": aliasID = "Code"; break;
                }

                int index = gItems.GetSelectedIndex();
                if (pVal.ColUID != "ItemCode")
                {
                    var values = pVal.GetSelectedValues(aliasID);
                    if (values.Count > 0)
                        gItems.DataTable.SetValue(pVal.ColUID, index, values[aliasID]);
                }
                else
                {
                    var values = pVal.GetSelectedValues(aliasID, "ItemName");
                    if (values.Count > 0)
                    {
                        gItems.DataTable.SetValue(pVal.ColUID, index, values[aliasID]);
                        gItems.DataTable.SetValue("ItemName", index, values["ItemName"]);
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

        private void bAdd_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                this.UIAPIRawForm.Freeze(true);

                var dt = gItems.DataTable;

                int row = dt.Rows.Count - 1;
                if (dt.Rows.Count != 0)
                {
                    string itemCode = dt.GetValue("ItemCode", row).ToString().Trim();
                    if (!string.IsNullOrEmpty(itemCode))
                    {
                        dt.Rows.Add();
                        row = dt.Rows.Count - 1;
                    }
                    else
                    {
                        SboStatusBar.SetTextAsError("You must first select an item");
                    }
                }
                else
                {
                    dt.Rows.Add();
                    row = dt.Rows.Count - 1;
                }

                gItems.SetCellFocus(row, 3);

                var rows = gItems.Rows.SelectedRows;
                rows.Clear();
                rows.Add(row);
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

        private void bDel_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                this.UIAPIRawForm.Freeze(true);

                if (gItems.SelectedRowsCount() == 0)
                {
                    SboStatusBar.SetTextAsError("You must select at least one line.");
                    return;
                }

                List<int> lines = new List<int>();
                for (int j = 0; j < gItems.SelectedRowsCount(); j++)
                {
                    int rowIndex = gItems.Rows.SelectedRows.Item(j, SAPbouiCOM.BoOrderType.ot_RowOrder);
                    lines.Add(rowIndex);
                }

                for (int i = lines.Count - 1; i >= 0; i--)
                {
                    gItems.DataTable.Rows.Remove(lines[i]);
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
            int currentRow = -1;
            bool canClose = false;

            try
            {
                this.UIAPIRawForm.Freeze(true);

                var dt = gItems.DataTable;
                if (dt.Rows.Count == 0)
                {
                    SboStatusBar.SetTextAsError("You must add at least one line.");
                    return;
                }

                int result = Application.SBO_Application.MessageBox("Do you want to create a delivery note?", 1, "Yes", "No");
                if (result != 1)
                    return;

                DateTime curDate = DateTime.Now;

                var uds = this.UIAPIRawForm.DataSources.UserDataSources;
                string cardCode = uds.Item("udCardCode").Value.Trim();

                SAPbobsCOM.BoObjectTypes objType = SAPbobsCOM.BoObjectTypes.oDeliveryNotes;
                SAPbobsCOM.Documents oDelivery = (SAPbobsCOM.Documents)SboClient.Instance.Company.GetBusinessObject(objType);
                oDelivery.CardCode = cardCode;
                oDelivery.DocDate = curDate;
                oDelivery.DocDueDate = curDate;
                oDelivery.TaxDate = curDate;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    currentRow = i;

                    string itemCode = dt.GetValue("ItemCode", i).ToString();
                    int baseEntry = Convert.ToInt32(dt.GetValue("DocEntry", i));
                    string whsCode = dt.GetValue("WhsCode", i).ToString();
                    string currency = dt.GetValue("Currency", i).ToString();
                    double price = Convert.ToDouble(dt.GetValue("Price", i));
                    string taxCode = dt.GetValue("TaxCode", i).ToString();
                    double quantity = Convert.ToDouble(dt.GetValue("RcptQty", i));

                    if (string.IsNullOrEmpty(itemCode))
                    {
                        if (baseEntry != 0) { SetCellFocus("You must select a valid item.", i, 3); }
                        else { SetCellFocus("You must select a valid item.", i, 10); }
                        return;
                    }

                    if (string.IsNullOrEmpty(whsCode))
                    {
                        SetCellFocus("You must select a valid warehouse.", i, 5);
                        return;
                    }

                    if (string.IsNullOrEmpty(currency))
                    {
                        SetCellFocus("You must select a valid currency.", i, 6);
                        return;
                    }

                    if (price == 0)
                    {
                        SetCellFocus("Price must be greater than 0.", i, 7);
                        return;
                    }

                    if (string.IsNullOrEmpty(taxCode))
                    {
                        SetCellFocus("You must select a valid tax code.", i, 8);
                        return;
                    }

                    if (quantity == 0)
                    {
                        SetCellFocus("Quantity must be greater than 0.", i, 10);
                        return;
                    }

                    var lines = oDelivery.Lines;
                    if (baseEntry != 0)
                    {
                        lines.BaseType = (int)SAPbobsCOM.BoObjectTypes.oOrders;
                        lines.BaseEntry = baseEntry;
                        lines.BaseLine = Convert.ToInt32(dt.GetValue("LineNum", i));
                    }

                    lines.ItemCode = itemCode;
                    lines.WarehouseCode = whsCode;
                    lines.Currency = currency;
                    lines.Price = price;
                    lines.Quantity = quantity;
                    lines.TaxCode = taxCode;

                    SAPbobsCOM.Items oItem = (SAPbobsCOM.Items)SboClient.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems);
                    if (!oItem.GetByKey(itemCode))
                        throw new Exception(itemCode + ": Item couldn't be loaded.");

                    if (oItem.ManageBatchNumbers == SAPbobsCOM.BoYesNoEnum.tYES)
                    {
                        SetCellFocus(string.Format("'{0}': Item managed by batch numbers is not supported.", itemCode), i, 3);
                        return;
                    }
                    else if (oItem.ManageSerialNumbers == SAPbobsCOM.BoYesNoEnum.tYES)
                    {
                        SetCellFocus(string.Format("'{0}': Item managed by serial numbers is not supported.", itemCode), i, 3);
                        return;
                    }

                    lines.Add();
                }

                SboClient.Instance.lRetCode = oDelivery.Add();
                if (SboClient.Instance.lRetCode != 0)
                    throw new SboException();

                SboClient.Instance.GetNewObject();
                if (SboClient.Instance.NewObjectType != ((int)objType).ToString())
                    throw new Exception("Last business object is not a delivery note: " + SboClient.Instance.NewObjectType);

                canClose = true;

                Application.SBO_Application.OpenUserForm<frmSboDeliveries>();
                frmSboDeliveries.Search();

                Application.SBO_Application.OpenForm(SAPbouiCOM.BoFormObjectEnum.fo_DeliveryNotes, null, SboClient.Instance.NewObjectKey);

                Application.SBO_Application.MessageBox("Delivery note has been created successfully.");
            }
            catch (SboException ex)
            {
                SboStatusBar.SetTextAsError(ex);

            }
            catch (Exception ex)
            {
                SboStatusBar.SetTextAsError(ex);
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);

                if (canClose)
                    this.UIAPIRawForm.Close();
            }
        }

        private void SetCellFocus(string message, int rowNum, int colNum = -1)
        {
            gItems.Rows.SelectedRows.Clear();
            gItems.Rows.SelectedRows.Add(rowNum);

            if (colNum != -1)
                gItems.SetCellFocus(rowNum, colNum);

            SboStatusBar.SetTextAsError(message);
        }

        public static void GetItems(string cardCode, string[] orders)
        {
            var formType = SboFramework.GetAttribute<FormTypeAttribute>(typeof(frmSboDeliveryItems));
            var oForm = Application.SBO_Application.Forms.Item(formType.Name);

            try
            {
                oForm.Freeze(true);

                var uds = oForm.DataSources.UserDataSources;
                uds.Item("udCardCode").Value = cardCode;

                string sQuery = DbHelper.Instance.GetItemsByOrders(orders);

                var gItems = (SAPbouiCOM.Grid)oForm.Items.Item("gItems").Specific;
                gItems.DataTable.ExecuteQuery(sQuery);
                gItems.SetEditTextColumn("DocEntry", "Document Entry", false, "17");
                gItems.SetEditTextColumn("DocNum", "Document Number", false);
                gItems.SetEditTextColumn("LineNum", "Line", false);
                gItems.SetEditTextColumn("ItemCode", "Item No.", true, "4", "cflItems", "ItemCode");
                gItems.SetEditTextColumn("ItemName", "Description", false);
                gItems.SetEditTextColumn("WhsCode", "Warehouse", true, "64", "cflWhs", "WhsCode");
                gItems.SetEditTextColumn("Currency", "Currency", true, "37", "cflCurr", "CurrCode");
                gItems.SetEditTextColumn("Price", "Price", true);
                gItems.SetEditTextColumn("TaxCode", "Tax Code", true, "128", "cflTaxes", "Code");
                gItems.SetEditTextColumn("Quantity", "Open Quantity", false);
                gItems.SetEditTextColumn("RcptQty", "Quantity", true);

                gItems.AutoResizeColumns();
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
    }
}