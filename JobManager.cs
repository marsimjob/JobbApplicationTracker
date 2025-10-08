using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobbApplicationTracker
{
    internal class JobManager
    {

        public List<JobApplication> jobApplications = new List<JobApplication>();

        public void AddJob()
        {
            // TODO:: Add job to jobApplications list by Comapny and Position Title
        }

        public void UpdateStatus()
        {
            // TODO:: Update Status on chosen (from the job application list) jobApplication
        }

        public void ShowAll()
        {
            // TODO:: Show All applications
        }

        public void ShowByStatus(Status statusToShow)
        {
            // TODO:: Use LINQ to get a filtered list of jobs that matches the status
        }

        public void ShowStatistics()
        {
            // TODO:: Show with LINQ functions Count, Avarage, OrderBy, Where Statistics about the job
        }
    }
}
