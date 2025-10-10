using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace JobbApplicationTracker
{
    internal class JobManager
    {

        public List<JobApplication> jobApplications;

        public JobManager()
        {
            // Mock data to begin the application 
            jobApplications = new List<JobApplication> {
            new JobApplication("Apple", "Software Engineer", Status.Offer, DateTime.Today, null, 60000),
            new JobApplication("Microsoft", "Data Analyst", Status.Interview, new DateTime(2025, 9, 20), new DateTime(2025, 10, 5), 55000),
            new JobApplication("Google", "Product Manager", Status.Offer, new DateTime(2025, 8, 30), new DateTime(2025, 9, 10), 70000),
            new JobApplication("Volvo", "UX Designer", Status.Reject, new DateTime(2025, 7, 25), null, 50000),
            new JobApplication("Ericsson", "HR Specialist", Status.Reject, new DateTime(2025, 9, 10), null, 45000),
            new JobApplication("Spotify", "Marketing Coordinator", Status.Interview, new DateTime(2025, 9, 18), new DateTime(2025, 10, 3), 48000),
            new JobApplication("Sony", "HR Consultant", Status.Applied, new DateTime(2024, 11, 5), new DateTime(2999, 12, 3), 59000)
            };
        }
        public void AddJob()
        {
            Console.Clear();
            Helper.WriteOut("You will now submit a job application to your job application list.");

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
                Helper.WriteOut($"An application at this company for this application already exists!");
            }
            else
            {
                jobApplications.Add(newJob);
                Helper.WriteOut("Your job application was a sucessfully completed!");
            }
            Helper.CallReturnToMenu();
        }
        public void RemoveJob()
        {
            Console.Clear();

            if (jobApplications.Count <= 0)
            {
                Helper.WriteOut("You currently have no job applications registered. There is nothing to remove.");
                Helper.CallReturnToMenu();
                return;
            }

            int rmvanswer = Helper.Menu("What sort of removal would you you like to commit?", Helper.removeOptions);
            if (rmvanswer != null)
            {
                if (rmvanswer == 0)
                {
                    Helper.WriteOut("We will now look for the job application you wish to delete.");
                    Console.Write("Enter the company name: ");
                    string companyInput = Console.ReadLine();

                    Console.Write("Enter the position title: ");
                    string titleInput = Console.ReadLine();

                    // Try to find matching job with LINQ, matching strings with user input, .FirstOrDefault will just give the first best match
                    JobApplication job = jobApplications
                        .FirstOrDefault(j => j.CompanyName.Equals(companyInput, StringComparison.OrdinalIgnoreCase)
                                          && j.PositionTitle.Equals(titleInput, StringComparison.OrdinalIgnoreCase));

                    // If no job found I will tell the user here
                    if (job == null)
                    {
                        Helper.WriteOut("No entry in your job lists is found that matches your input. Try again in another session");
                    }
                    else
                    {
                        // Remove job from job applications
                        jobApplications.Remove(job);
                        Helper.WriteWithStatusColor($"Jobb application for {job.PositionTitle} at {job.CompanyName} has successfully been removed from your list!", Status.Reject);
                    }
                }

                else if (rmvanswer == 1)
                {
                    // Remove all rejected applications with LINQ
                    int removedCount = jobApplications.RemoveAll(job => job.Status == Status.Reject);
                    Helper.WriteWithStatusColor($"{removedCount} rejected job applications have been removed.", Status.Reject);
                }
                else if (rmvanswer == 2)
                {
                    // Remove all jobs
                    jobApplications.Clear();
                    Helper.WriteWithStatusColor($"All your job applications have been successfully deleted.", Status.Reject);
                }
                else
                {
                    Helper.WriteOut("Booting back to main.");
                }
            }
            Helper.CallReturnToMenu();
        }
        public void UpdateStatus()
        {
            Console.Clear();

            if (jobApplications.Count <= 0)
            {
                Helper.WriteOut("You currently have no job applications registered. There is nothing do update.");
                Helper.CallReturnToMenu();
                return;
            }

            Helper.WriteOut("Which job application would you like to change the status for?");
            Console.Write("Enter the company name: ");
            string companyInput = Console.ReadLine();

            Console.Write("Enter the position title: ");
            string titleInput = Console.ReadLine();

            // Try to find matching job with LINQ, matching strings with user input, .FirstOrDefault will just give the first best match or default if nothing
            JobApplication job = jobApplications
                .FirstOrDefault(j => j.CompanyName.Equals(companyInput, StringComparison.OrdinalIgnoreCase)
                                  && j.PositionTitle.Equals(titleInput, StringComparison.OrdinalIgnoreCase));

            // If no job found I will tell the user here
            if (job == null)
            {
                Helper.WriteOut("No matching job application found for that company and position.");
            }
            else
            {
                Helper.WriteOut($"Current status: {job.Status}");
                Console.Write("Enter the new status (Applied, Interview, Offer, Reject): ");
                string statusInput = Console.ReadLine();

                Status newStatus;

                while (!Enum.TryParse<Status>(statusInput, true, out newStatus)
                       || !Enum.IsDefined(typeof(Status), newStatus))
                {
                    Console.Write("The status you have entered is unavailable. Please enter one of the following:" +
                        " Applied, Interview, Offer, or Reject: ");
                    statusInput = Console.ReadLine();
                }

                job.Status = newStatus;

                Helper.WriteWithStatusColor($"Status updated. {job.CompanyName} — {job.PositionTitle} is now {job.Status}.", job.Status);
            }
            Helper.CallReturnToMenu();
        }


        public void ShowAll()
        {
            Console.Clear();

            if (jobApplications.Count <= 0)
            {
                Helper.WriteOut("You currently have no job applications registered. There is nothing to show.");
                Helper.CallReturnToMenu();
                return;
            }

            foreach (JobApplication application in jobApplications)
            {
                Helper.WriteWithStatusColor(application.GetSummary(), application.Status);
            }
            Helper.CallReturnToMenu();
        }

        public void ShowByStatus()
        {
            Console.Clear();

            if (jobApplications.Count <= 0)
            {
                Helper.WriteOut("You currently have no job applications registered. There is nothing to show.");
                Helper.CallReturnToMenu();
                return;
            }

            Console.Write("Enter the new status to search by: 'Applied', 'Interview', 'Offer', or 'Reject'): ");
            string statusInput = Console.ReadLine();

            Status newStatus;

            while (!Enum.TryParse<Status>(statusInput, true, out newStatus)
                   || !Enum.IsDefined(typeof(Status), newStatus))
            {
                Console.Write("Please search by the available options: 'Applied', 'Interview', 'Offer', or 'Reject'): ");
                statusInput = Console.ReadLine();
            }

            // Filters out the status-matching jobs using LINQ:
            IOrderedEnumerable<JobApplication> filteredJobs = jobApplications // Get a list of filtered object based on jobApplications
                .Where(job => job.Status == newStatus) // Where. picks only if matching status
                .OrderBy(job => job.ApplicationDate);  // Sorts by time (earliest application first)

            // Print out info via JobApplication's GetSummary() function.
            foreach (var job in filteredJobs)
            {
                Helper.WriteWithStatusColor(job.GetSummary(), job.Status);
            }
            Helper.CallReturnToMenu();
        }

        public void ShowStatistics()
        {
            Console.Clear();

            if (jobApplications.Count <= 0)
            {
                Helper.WriteOut("You currently have no job applications registered. There are no stastics to display.");
                Helper.CallReturnToMenu();
                return;
            }

            if (jobApplications.Count <= 0)
            {
                Helper.WriteOut("You currently have no job applications!");
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
            Helper.WriteOut($"Job Count: {totalJobs}");
            // AVARAGE SALARY FOR ALL (2 decimal points):
            Helper.WriteOut($"Average salary expectation: {avrgSalary:F2}kr");
            // SHOW TOTAL OF DIFFERENT STATUS JOBS:
            foreach (var group in byStatus)
            {
                Helper.WriteWithStatusColor($"{group.Status}: {group.Count}", group.Status);
            }
            Helper.CallReturnToMenu();
        }

        public void ShowAllInNewestOrder()
        {
            Console.Clear();

            if (jobApplications.Count <= 0)
            {
                Helper.WriteOut("You currently have no job applications registered. No new or old entries!");
                Helper.CallReturnToMenu();
                return;
            }

            if (jobApplications.Count <= 0)
            {
                Helper.WriteOut("There are no available job applications!");
                return;
            }

            // Sort job applications by ApplicationDate in descending order as a list
            var newestSorted = jobApplications
                .OrderByDescending(j => j.ApplicationDate)
                .ToList();

            // Display each job application
            foreach (JobApplication application in newestSorted)
            {
                Helper.WriteWithStatusColor(application.GetSummary(), application.Status);
            }
            Helper.CallReturnToMenu();
        }

        public void ShowNonRejectJobsTwoWeeksMax()
        {
            // If jobapplication is applied within span of 2 weeks old display it it
            Console.Clear();
            // Check today's date
            DateTime today = DateTime.Today;
            // Two weeks time
            TimeSpan twoWeeks = TimeSpan.FromDays(14);

            // Go over all jobapplications with LINQ and get a new list with jobs
            // that were applied withing the 14 days for now  AND are NOT reject status
            var availableJob = jobApplications
            .Where(j => j.Status != Status.Reject
            && j.ApplicationDate <= today
            && (today - j.ApplicationDate) <= twoWeeks).ToList();

            if (!availableJob.Any())
            {
                Helper.WriteOut("No non-rejected applications in the last two weeks.");
                Helper.CallReturnToMenu();
                return;
            }

            // Go over the new list in a foreach loop and present it using Helper.WriteWithStatusColor()
            foreach (var job in availableJob)
            {
                Helper.WriteWithStatusColor(job.GetSummary(), job.Status);
            }

            Helper.CallReturnToMenu();
        }

        public void ShowOldAppliedJobsWithNoResponse()
        {
            Console.Clear();

            DateTime today = DateTime.Today;
            TimeSpan twoWeeks = TimeSpan.FromDays(14);

            // If status is Applied and no responseDate and the application is over 2 weeks old

            var oldJobs= jobApplications
                .Where(j =>
                    j.Status == Status.Applied // If the job has been applied to
                    && j.ResponseDate >= today // If the response date is stille active
                    && j.ApplicationDate <= today // if it was applied today or earlier
                    && (today - j.ApplicationDate) > twoWeeks // If the application date is 2 weeks old
                ).ToList(); // Make a list

            // If there are no jobs that match the requirements
            if (!oldJobs.Any())
            {
                Helper.WriteOut("No old 'Applied' with no response");
                Helper.CallReturnToMenu();
                return;
            }

            foreach (var job in oldJobs)
            {
                Helper.WriteWithStatusColor(job.GetSummary(), job.Status);
            }

            Helper.CallReturnToMenu();
        }
    }

}
