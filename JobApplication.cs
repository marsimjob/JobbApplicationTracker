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


        public void GetDaysSinceApplied()
        {
            // TODO::  Compare time with now
        }
        public void GetSummary()
        {
            // TODO:: Get all info from this job application neatly displayed
        }
    }
}
