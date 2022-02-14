namespace EWP.Extensions
{
    static class ChooseFromListExtensions
    {
        public static void SetCondition(this SAPbouiCOM.ChooseFromList oCFL, string Alias, string CondVal, SAPbouiCOM.BoConditionOperation Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL)
        {
            var oCons = new SAPbouiCOM.Conditions();
            var oCon = oCons.Add();

            oCon.Alias = Alias;
            oCon.Operation = Operation;
            oCon.CondVal = CondVal;

            oCFL.SetConditions(oCons);
        }
    }
}