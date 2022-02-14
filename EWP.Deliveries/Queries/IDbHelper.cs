namespace EWP
{
    interface IDbHelper
    {
        string GetOrders(string cardCode, string startDate, string endDate);
        string GetItemsByOrders(string[] orders);
    }
}
