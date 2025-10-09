using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobbApplicationTracker
{
    internal class JobManager
    {

        public List<JobApplication> jobApplications;

        public JobManager() 
        {
            // Mock data to begin the application 
            jobApplications = new List<JobApplication> {
            new JobApplication("Company A", "Software Engineer", Status.Applied, new DateTime(2025, 9, 15), null, 60000),
            new JobApplication("Company B", "Data Analyst", Status.Interview, new DateTime(2025, 9, 20), new DateTime(2025, 10, 5), 55000),
            new JobApplication("Company C", "Product Manager", Status.Offer, new DateTime(2025, 8, 30), new DateTime(2025, 9, 10), 70000),
            new JobApplication("Company D", "UX Designer", Status.Reject, new DateTime(2025, 7, 25), null, 50000),
            new JobApplication("Company E", "HR Specialist", Status.Applied, new DateTime(2025, 9, 10), null, 45000),
            new JobApplication("Company F", "Marketing Coordinator", Status.Interview, new DateTime(2025, 9, 18), new DateTime(2025, 10, 3), 48000)
            };
        }
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
            Console.Write("What is your salary expectation in kr (whole numbers): ");
            string salaryInput = Console.ReadLine();
            int salaryExpectation;
            while (!int.TryParse(salaryInput, out salaryExpectation))
            {
                Console.Write("Please enter an whole number in kr: ");
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
        public void RemoveJob()
        {
            Console.WriteLine($"Would you like to 1) Remove one particular job application OR 2) Remove All Rejected applications?");
            Console.Write("Please enter: ");
            string answer = Console.ReadLine();
            if (answer != null)
            {
                if (answer == "1")
                {
                    Console.WriteLine("We will now look for the job application you wish to delete.");
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
                        Console.WriteLine("No entry in your job lists is found that matches your input. Try again in another session");
                        return;
                    }

                    // Remove job from job applications
                    jobApplications.Remove(job);
                    Console.WriteLine($"Jobb application for {job.PositionTitle} at {job.CompanyName} has successfully been removed from your list!");
                }

                else if (answer == "2")
                {
                    // Used LINQ to filter out the entires in the list marked Status.Reject
                    // Filters out the status-matching jobs using LINQ: 
                    IOrderedEnumerable<JobApplication> jobs = jobApplications // Get a list of filtered object based on jobApplications
                        .Where(job => job.Status == Status.Reject) // Where. picks only if matching status
                        .OrderBy(job => job.ApplicationDate);  // Sorts by time (earliest application first)

                    // Use the new filtered list and remove all of them
                    foreach (JobApplication jobApplication in jobs) 

                        {
                        jobApplications.Remove(jobApplication);
                        Console.WriteLine($"Jobb application for {jobApplication.PositionTitle} at {jobApplication.CompanyName} has successfully been removed from your list!");
                        }
                    return;
                }
                else
                {
                    Console.WriteLine("Invalid answer, booting back to main");
                    return;
                }
            }
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
            if(jobApplications.Count <= 0)
            {
                Console.WriteLine("The are a no available job applications!");
            }
            foreach (JobApplication application in jobApplications)
            {
               Console.WriteLine(application.GetSummary());
            }
        }

        public void ShowByStatus()
        {
            Console.Write("Enter the new status to search by (Applied, Interviewing, Offer, Rejected): ");
            string statusInput = Console.ReadLine();

            Status newStatus;
            while (!Enum.TryParse<Status>(statusInput, true, out newStatus)
                   || !Enum.IsDefined(typeof(Status), newStatus))
            {
                Console.Write("The status you have entered is unavailable. Please enter one of the following:" +
                    " Applied, Interviewing, Offer, or Rejected: ");
                statusInput = Console.ReadLine();
            }

            // Filters out the status-matching jobs using LINQ:
            IOrderedEnumerable<JobApplication> filteredJobs = jobApplications // Get a list of filtered object based on jobApplications
                .Where(job => job.Status == newStatus) // Where. picks only if matching status
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
        }

        public void ShowStatisticsNewestFirst()
        {
            ShowStatistics();

            // SHOW NEWEST FIRST:
            var newest = jobApplications
                .OrderBy(j => j.ApplicationDate)
                .Last();

            Console.WriteLine("Newest application: " + newest.GetSummary());
        }

    }
}
