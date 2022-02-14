namespace EWP.Extensions
{
    static class ItemExtensions
    {
        public static T Item<T>(this SAPbouiCOM.Items oItems, string index)
        {
            return (T)oItems.Item(index).Specific;
        }
    }
}
