using System;

namespace EWP.Extensions
{
    static class GridExtensions
    {
        public static bool HasSelectedRows(this SAPbouiCOM.Grid oGrid)
        {
            var t = oGrid.Rows.SelectedRows;
            return t.Count > 0;
        }

        public static int SelectedRowsCount(this SAPbouiCOM.Grid oGrid)
        {
            var t = oGrid.Rows.SelectedRows;
            return t.Count;
        }

        public static int GetSelectedIndex(this SAPbouiCOM.Grid oGrid)
        {
            return oGrid.GetSelectedIndex(0);
        }

        public static int GetSelectedIndex(this SAPbouiCOM.Grid oGrid, int index)
        {
            return oGrid.Rows.SelectedRows.Item(index, SAPbouiCOM.BoOrderType.ot_RowOrder);
        }

        public static string GetSelectedValue(this SAPbouiCOM.Grid oGrid, string column)
        {
            var t = oGrid.Rows.SelectedRows;

            if (t.Count == 0)
                return null;

            int index = t.Item(0, SAPbouiCOM.BoOrderType.ot_RowOrder);
            return Convert.ToString(oGrid.DataTable.GetValue(column, index));
        }

        public static string GetSelectedValue(this SAPbouiCOM.Grid oGrid, int index, string column)
        {
            var t = oGrid.Rows.SelectedRows;

            if (t.Count == 0)
                return null;

            int currIndex = t.Item(index, SAPbouiCOM.BoOrderType.ot_RowOrder);
            return Convert.ToString(oGrid.DataTable.GetValue(column, currIndex));
        }

        public static bool ExistValue(this SAPbouiCOM.Grid oGrid, string Column, string Value)
        {
            for (int i = 0; i < oGrid.DataTable.Rows.Count; i++)
            {
                if (!Convert.ToString(oGrid.DataTable.GetValue(Column, i)).Equals(Value))
                    continue;

                return true;
            }

            return false;
        }

        public static void SetCheckBoxColumn(this SAPbouiCOM.Grid oGrid, string ColumnName, string Caption, bool IsEditable)
        {
            SAPbouiCOM.GridColumn gridColumn = oGrid.Columns.Item(ColumnName);

            gridColumn.TitleObject.Caption = Caption;
            gridColumn.Type = SAPbouiCOM.BoGridColumnType.gct_CheckBox;
            gridColumn.Editable = IsEditable;
        }

        public static void SetEditTextColumn(this SAPbouiCOM.Grid oGrid, string ColumnName, string Caption, bool IsEditable, string LinkedObjectType = null, string cflUID = null, string aliasID = null)
        {
            SAPbouiCOM.GridColumn gridColumn = oGrid.Columns.Item(ColumnName);

            gridColumn.TitleObject.Caption = Caption;
            gridColumn.Type = SAPbouiCOM.BoGridColumnType.gct_EditText;
            gridColumn.Editable = IsEditable;

            SAPbouiCOM.EditTextColumn oEditCol = (SAPbouiCOM.EditTextColumn)oGrid.Columns.Item(ColumnName);
            if (!string.IsNullOrEmpty(LinkedObjectType))
                oEditCol.LinkedObjectType = LinkedObjectType;

            if (!string.IsNullOrEmpty(cflUID))
            {
                oEditCol.ChooseFromListUID = cflUID;
                oEditCol.ChooseFromListAlias = aliasID;
            }
        }

        public static void SetComboBoxColumn(this SAPbouiCOM.Grid oGrid, string ColumnName, string Caption, bool IsEditable, bool FirstRowIsEmpty = false)
        {
            SAPbouiCOM.GridColumn gridColumn = oGrid.Columns.Item(ColumnName);

            gridColumn.TitleObject.Caption = Caption;
            gridColumn.Type = SAPbouiCOM.BoGridColumnType.gct_ComboBox;
            gridColumn.Editable = IsEditable;
        }

        public static void SetComboBoxColumn<T>(this SAPbouiCOM.Grid oGrid, string ColumnName, string Caption, bool IsEditable, bool FirstRowIsEmpty = false)
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be enum type");

            SAPbouiCOM.GridColumn gridColumn = oGrid.Columns.Item(ColumnName);

            gridColumn.TitleObject.Caption = Caption;
            gridColumn.Type = SAPbouiCOM.BoGridColumnType.gct_ComboBox;
            gridColumn.Editable = IsEditable;

            SAPbouiCOM.ComboBoxColumn oEditCol = (SAPbouiCOM.ComboBoxColumn)oGrid.Columns.Item(ColumnName);
            oEditCol.ValidValues.AddRange<T>(FirstRowIsEmpty);
        }
    }
}
