using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobbApplicationTracker
{
    public enum Status
    {
        Applied,
        Interview,
        Offer,
        Reject
    }
    internal class JobApplication
    {
        public string CompanyName;
        public string PositionTitle;
        public Status Status;
        public DateTime ApplicationDate;
        public DateTime? ResponseDate;
        public int SalaryExpectation;

        public JobApplication(string companyName, string positionTitle, Status status, DateTime applicationDate, DateTime? responseDate, int salaryExpectation)
        {
            CompanyName = companyName;
            PositionTitle = positionTitle;
            Status = status;
            ApplicationDate = applicationDate;
            ResponseDate = responseDate;
            SalaryExpectation = salaryExpectation;
        }

        public int GetDaysSinceApplied()
        {
            // Return int amount of days
            return (DateTime.Now - ApplicationDate).Days;
        }

        public string GetSummary()
        {
            return $"{CompanyName}: {PositionTitle}, Status: {Status}, Applied: {ApplicationDate:d}, Salary: {SalaryExpectation}kr";
        }

    }
}
