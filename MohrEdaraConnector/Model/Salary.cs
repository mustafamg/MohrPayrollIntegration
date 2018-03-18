namespace MohrEdaraConnector.Model
{
    public class Salary
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public System.DateTime CreateDate { get; set; }
        public bool IsPaid { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public double Value { get; set; }
        public System.DateTime From { get; set; }
        public System.DateTime To { get; set; }
        public string Label { get; set; }
        public double PaidAmount { get; set; }

        //public virtual ICollection<SalaryItem> SalaryItems { get; set; }
    }
}
