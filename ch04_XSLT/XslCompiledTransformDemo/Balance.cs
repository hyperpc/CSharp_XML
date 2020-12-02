namespace XslCompiledTransformDemo
{
    public class Balance
    {
        private decimal newbalance=0;
        public decimal RMBBalance(decimal balance, decimal conv){
            decimal tmp = balance * conv;
            newbalance = decimal.Round(tmp, 2);
            return newbalance;
        }
    }
}