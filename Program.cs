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
                Console.WriteLine();

                int selected = Helper.Menu("Welcome to the JOB APPLICATION service. What do you wish to do today?", Helper.menuOptions);

                switch (selected)
                {
                    case 0: 
                        jobTracker.AddJob(); 
                        break;
                    case 1: 
                        jobTracker.ShowAll(); 
                        break;
                    case 2: 
                        jobTracker.ShowByStatus(); 
                        break;
                    case 3: 
                        jobTracker.ShowAllInNewestOrder(); 
                        break;
                    case 4: 
                        jobTracker.ShowStatistics(); 
                        break;
                    case 5: 
                        jobTracker.UpdateStatus(); 
                        break;
                    case 6: 
                        jobTracker.RemoveJob(); 
                        break;
                    case 7: 
                        running = false;
                        break;
                }
            }

            Helper.WriteOut("Completing program. Thank you for using our services.");
            Console.ReadLine();
        }
    }
}
