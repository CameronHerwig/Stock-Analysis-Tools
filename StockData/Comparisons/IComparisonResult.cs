namespace Stock_Data
{
    public interface IComparisonResult
    {
        string TestName { get; set; }
        double SuccessPercent{ get; set; }
        double Total { get; set; }
        double Success { get; set; }
    }
}
