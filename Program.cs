namespace JobbApplicationTracker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            JobManager jobTracker = new JobManager();
            
            bool running = true;
            while (running)
            {
                Console.WriteLine("Welcome to the JOB APPLICATION service. What do you wish to do today?");
                Console.WriteLine("1) Add a new job application");
                Console.WriteLine("2) Show all job applications");
                Console.WriteLine("3) Filter job applications by status");
                Console.WriteLine("4) Sort jobb applications by date");
                Console.WriteLine("5) Show statistics");
                Console.WriteLine("6) Update status of an job application");
                Console.WriteLine("7) Delete an application");
                Console.WriteLine("8) Exit program");
                Console.Write("Enter a choice: ");
                string choice = Console.ReadLine();

                Console.WriteLine();
                switch (choice)
                {
                    case "1":
                        jobTracker.AddJob();
                        break;
                    case "2":
                        jobTracker.ShowAll();
                        break;
                    case "3":
                        jobTracker.ShowByStatus();
                        break;
                    case "4":
                        jobTracker.ShowStatisticsNewestFirst();
                        break;
                    case "5":
                        jobTracker.ShowStatistics();
                        break;
                    case "6":
                        jobTracker.UpdateStatus();
                        break;
                    case "7":
                        jobTracker.RemoveJob();
                        break;
                    case "8":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Your choice is not valid, try again.");
                        break;
                }
            }

            Console.WriteLine("Completing program. Thank you for using our services.");
        }
    }
}
