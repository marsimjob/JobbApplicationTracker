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
            Console.WriteLine("You will now submit a job application to your job application list.");

            // NAME:
            Console.Write("What is the company called? ");
            string companyName = Console.ReadLine();

            // TITLE:
            Console.Write("What is the position title? ");
            string positionTitle = Console.ReadLine();

            // APPLICATION DATE:
            Console.Write("When did you apply? (YYYY-MM-DD) ");
            string appDateInput = Console.ReadLine();
            DateTime applicationDate;
            while (!DateTime.TryParse(appDateInput, out applicationDate))
            {
                Console.Write("Please enter date as YYYY-MM-DD: ");
                appDateInput = Console.ReadLine();
            }

            // RESPONSE DATE (response date can also be null or blank):
            Console.Write("If you have a response date, enter it (YYYY-MM-DD), otherwise leave blank: ");
            string respDateInput = Console.ReadLine();
            DateTime? responseDate = null;
            if (!string.IsNullOrWhiteSpace(respDateInput))
            {
                DateTime respDt;
                while (!DateTime.TryParse(respDateInput, out respDt))
                {
                    Console.Write("Please enter as YYYY-MM-DD (or leave blank): ");
                    respDateInput = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(respDateInput))
                    {
                        respDt = default;
                        break;
                    }
                }
                if (!string.IsNullOrWhiteSpace(respDateInput))
                    responseDate = respDt;
            }

            // SALARAY
            Console.Write("What is your salary expectation (integer, e.g. 50000)? ");
            string salaryInput = Console.ReadLine();
            int salaryExpectation;
            while (!int.TryParse(salaryInput, out salaryExpectation))
            {
                Console.Write("Invalid number. Please enter an integer salary expectancy: ");
                salaryInput = Console.ReadLine();
            }

            // Create a job object
            JobApplication newJob = new JobApplication(
                companyName,
                positionTitle,
                Status.Applied, // I just put status applied here since this is a job application
                applicationDate,
                responseDate,
                salaryExpectation
            );

            // Check for duplicate application with the same Company name and title ignore the application using LINQ
            bool exists = jobApplications.Any(j =>
                j.CompanyName.Equals(companyName, StringComparison.OrdinalIgnoreCase)
                && j.PositionTitle.Equals(positionTitle, StringComparison.OrdinalIgnoreCase));

            if (exists)
            {
                Console.WriteLine($"An application at this company for this application already exists!");
            }
            else
            {
                jobApplications.Add(newJob);
                Console.WriteLine("Your job application was a sucessfully completed!");
            }
            Console.WriteLine($"Returning to main menu.");
        }

        public void UpdateStatus()
        {
            Console.WriteLine("Which job application would you like to change the status for?");
            Console.Write("Enter the company name: ");
            string companyInput = Console.ReadLine();

            Console.Write("Enter the position title: ");
            string titleInput = Console.ReadLine();

            // Try to find matching job with LINQ, matching strings with user input, .First will just give the first best match
            JobApplication job = jobApplications
                .First(j => j.CompanyName.Equals(companyInput, StringComparison.OrdinalIgnoreCase)
                                  && j.PositionTitle.Equals(titleInput, StringComparison.OrdinalIgnoreCase));

            // If no job found I will tell the user here
            if (job == null)
            {
                Console.WriteLine("No matching job application found for that company and position.");
            }
            else
            {
                Console.WriteLine($"Current status: {job.Status}");
                Console.Write("Enter the new status (Applied, Interviewing, Offer, Rejected): ");
                string statusInput = Console.ReadLine();

                Status newStatus;
                while (!Enum.TryParse<Status>(statusInput, true, out newStatus)
                       || !Enum.IsDefined(typeof(Status), newStatus))
                {
                    Console.Write("The status you have entered is unavailable. Please enter one of the following:" +
                        " Applied, Interviewing, Offer, or Rejected: ");
                    statusInput = Console.ReadLine();
                }

                job.Status = newStatus;
                Console.WriteLine($"Status updated. {job.CompanyName} — {job.PositionTitle} is now {job.Status}.");
            }
        }


        public void ShowAll()
        {
            foreach (JobApplication application in jobApplications)
            {
                application.GetSummary();
            }
        }

        public void ShowByStatus(Status statusToShow)
        {
            // Filters out the status-matching jobs using LINQ:
            IOrderedEnumerable<JobApplication> filteredJobs = jobApplications // Get a list of filtered object based on jobApplications
                .Where(job => job.Status == statusToShow) // Where. picks only if matching status
                .OrderBy(job => job.ApplicationDate);  // Sorts by time (earliest application first)

            // Print out info via JobApplication's GetSummary() function.
            foreach (var job in filteredJobs)
            {
                Console.WriteLine(job.GetSummary());
            }
        }

        public void ShowStatistics()
        {
            if (jobApplications.Count <= 0)
            {
                Console.WriteLine("You currently have no job applications!");
                return; // Used return to break out of ShowStatistics() instead of if else (test).
            }

            // Total job counts
            int totalJobs = jobApplications.Count;

            // Using LINQ here to show avaragere salary, uses an IEnumarable function to show the int to double
            double avrgSalary = jobApplications.Average(j => j.SalaryExpectation);

            // More LINQ to show how many jobs there are by a certain status (how many applied, rejected, interview and so on)
            var byStatus = jobApplications
                .GroupBy(job => job.Status) // Make groups by looking at the status of a jobApplications member
                .Select(group => new
                {
                    Status = group.Key,
                    Count = group.Count()
                }); // Select makes a small dictionary struct out of these groups

            // DISPLAY INFORMATION:
            // JOB TOTAL:
            Console.WriteLine($"Job Count: {totalJobs}");
            // AVARAGE SALARY FOR ALL (2 decimal points):
            Console.WriteLine($"Average salary expectation: {avrgSalary:F2}");
            // SHOW TOTAL OF DIFFERENT STATUS JOBS:
            foreach (var group in byStatus)
            {
                Console.WriteLine($"{group.Status}: {group.Count}");
            }

            // SHOW NEWEST FIRST:
            var newest = jobApplications
                .OrderBy(j => j.ApplicationDate)
                .Last();

            Console.WriteLine("Newest application: " + newest.GetSummary());
        }

    }
}
