using System.Collections.Generic;

namespace EWP
{
    class DbHelper_HANA : IDbHelper
    {
        public string GetOrders(string cardCode, string startDate, string endDate)
        {
            string sQuery = @"SELECT 
                                'N' AS ""Checked""
	                            ,T0.""DocEntry""
	                            ,T0.""DocNum""
	                            ,T0.""DocDueDate"" 
	                            ,T0.""CardCode"" 
	                            ,T0.""CardName"" 
	                            ,(CASE WHEN T0.""DocRate"" = 1 THEN T0.""DocTotal"" ELSE T0.""DocTotalFC"" END) AS ""DocTotal""
	                            ,T0.""DocCur""
                            FROM ""ORDR"" T0 
                            WHERE T0.""DocStatus"" = 'O'
	                            {0}";

            List<string> sConds = new List<string>();

            if (!string.IsNullOrEmpty(cardCode))
                sConds.Add(string.Format(@"T0.""CardCode"" = '{0}'", cardCode));

            if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                sConds.Add(string.Format(@"T0.""DocDueDate"" BETWEEN '{0}' AND '{1}'", startDate, endDate));
            }
            else
            {
                if (!string.IsNullOrEmpty(startDate))
                {
                    sConds.Add(string.Format(@"T0.""DocDueDate"" = '{0}'", startDate));
                }
                else if (!string.IsNullOrEmpty(endDate))
                {
                    sConds.Add(string.Format(@"T0.""DocDueDate"" = '{0}'", endDate));
                }
            }

            string sWhere = sConds.Count != 0 ? "AND " + string.Join(" AND ", sConds) : null;
            return string.Format(sQuery, sWhere);
        }

        public string GetItemsByOrders(string[] orders)
        {
            string sQuery = @"SELECT 
                                T0.""DocEntry""
                                ,T1.""DocNum""
	                            ,T0.""LineNum""
	                            ,T0.""ItemCode""
	                            ,T0.""Dscription"" AS ""ItemName""
	                            ,T0.""WhsCode""
	                            ,T0.""Currency""
	                            ,T0.""Price""
	                            ,T0.""TaxCode""
	                            ,T0.""OpenQty"" AS ""Quantity""
	                            ,T0.""OpenQty"" AS ""RcptQty""
                            FROM ""RDR1"" T0 
                            INNER JOIN ""ORDR"" T1 ON T1.""DocEntry"" = T0.""DocEntry""
                            WHERE T0.""DocEntry"" IN ({0}) AND T0.""OpenQty"" > 0";

            return string.Format(sQuery, string.Join(", ", orders));
        }
    }
}
