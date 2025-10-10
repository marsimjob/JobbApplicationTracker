using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobbApplicationTracker
{
    internal static class Helper
    {
        // Menu displaying method, returns an int at selection to choose for the main menu.
        // Prompt is what the menu should ask the user to input the List of string should be options for them to choose.
        public static int Menu(string prompt, List<string> options)
        {
            // Selection will be presented in int
            int selected = 0;

            // Set key command
            ConsoleKeyInfo key;

            // Remove cursor from console
            Console.CursorVisible = false;

            while (true)
            {
                // Introduce prompt for questions
                Console.Clear();
                Console.WriteLine(prompt);
                Console.WriteLine();

                // Write out all the options to choose from (string) and highlight the selected one
                for (int i = 0; i < options.Count; i++)
                {
                    if (i == selected)  // If selected, set a highlight (the background of selection is all white with black foreground text)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine($" > {options[i]}");
                        Console.ResetColor();
                    }
                    else // Write it out normally
                    {
                        Console.WriteLine($"   {options[i]}");
                    }
                }

                // Set key to read
                key = Console.ReadKey(true);


                // Switch what key returns to change selected choice
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow: // Move down in options.Count
                        selected--;
                        if (selected < 0)
                            selected = options.Count - 1;
                        break;

                    case ConsoleKey.DownArrow: // Move up in options.Count
                        selected++;
                        if (selected >= options.Count)
                            selected = 0;
                        break;

                    case ConsoleKey.Enter:
                        Console.CursorVisible = true;
                        return selected;  // Return the chosen selection

                }
                // Small delay will avoid flicker apparently
                Thread.Sleep(2);
            }
        }

        // Top menu choices
        public static List<string> menuOptions = new List<string>() {
        "Add a new job application",
        "Show all job applications",
        "Filter job applications by status",
        "Sort job applications by date",
        "Show job applications statistics",
        "Update status of a job application",
        "Delete a job application",
        "Show non-rejected job applications (2 weeks max)",
        "Show unanswered job applications (2 weeks or older)",
        "Exit program"
        };

        // Remove menu choices
        public static List<string> removeOptions = new List<string>() {
           "Remove a particular job application(s)",
           "Remove rejected job application(s)",
           "Remove all job applications",
           "Back to main menu"
        };


        public static void WriteOut(string text, float charPerSecond = 100f)
        {
            // Writing out my text progressively like video game text boxes for the sake of experimentation
            // Takes a string argument and a float argument for how many characeters of the string to appear per second

            float timePerChar = 1.0f / charPerSecond; // If a char has to fit 100 char per second the lenght of a char to display is 0.01 (1/100) seconds.

            // Set default values to start from 0
            float elapsedTime = 0.0f; // Elapsed time from start to finish of the method block
            int shownCount = 0; // Shows how many chars have been displayed from current text, 0 to start

            // Get a timer to help compute my delaTime (I use this to make update framerate indepedenant)
            Stopwatch timer = Stopwatch.StartNew(); // Initiate the stopwatch.

            //Keep updating until lenght of the text parameter has been met
            while (shownCount < text.Length)
            {
                // Elapsed time since last loop (consistent time update unrelated to clockspeed)
                // Show realtime seconds passed since last iteration.
                float deltaTime = (float)timer.Elapsed.TotalSeconds;
                timer.Restart(); // Reset the stopwatch for next loop

                // Add the current deltaTime to accumlated total time 
                elapsedTime += deltaTime;

                // Show how many characters that need to be shown based on time spanned
                int charToShow = (int)Math.Floor(elapsedTime / timePerChar); // this is a floating value. Floor to get integer value.

                if (charToShow > shownCount)
                {
                    // Don't exceed the length of text, the amount to be shown
                    int writeCount = Math.Min(charToShow, text.Length);

                    // Write each substring of text
                    Console.Write(text.Substring(shownCount, writeCount - shownCount));

                    // Update shownCount to count justw written out
                    shownCount = writeCount;
                }

                // Pause for a few miliseconds
                Thread.Sleep(10);
            }

            // When loop is completed move to next line
            Console.WriteLine();

            //Show characer up to to toShow

        }

        internal static void CallReturnToMenu()
        {
            Console.WriteLine($"Press ENTER to return to main menu.");
            Console.ReadLine();
        }

        // Colors text depending on job status (meant to be used in foreach loops), but can be used with any text and enum Status
        internal static void WriteWithStatusColor(string text, Status status)
        {
            ConsoleColor original = Console.ForegroundColor;

            switch (status)
            {
                case Status.Offer:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case Status.Reject:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case Status.Interview:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case Status.Applied:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
            }

            WriteOut(text);

            Console.ForegroundColor = original;
        }
    }

}
