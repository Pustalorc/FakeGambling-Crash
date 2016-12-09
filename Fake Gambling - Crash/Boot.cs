using System;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace FakeGambling
{
    class Boot
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Use a function so that restart is simpler
            Run();
        }
        static void Run()
        {
            // Set forced exit to false, to prevent an infinite restart even when closing the program
            Values.ForcedExit = false;
            // Run the program
            Application.Run(new Crash());
            // If player lost, restart the app
            if (Values.ForcedExit == true)
            {
                Run();
            }
        }
    }
    static class Values
    {
        // Set lost variable
        public static bool ForcedExit = false;

        // Use RNGCryptoService to create a somewhat random number
        private static readonly RNGCryptoServiceProvider _generator = new RNGCryptoServiceProvider();

        public static int Between(int minimumValue, int maximumValue)
        {
            byte[] randomNumber = new byte[1];

            _generator.GetBytes(randomNumber);

            double asciiValueOfRandomCharacter = Convert.ToDouble(randomNumber[0]);

            // We are using Math.Max, and substracting 0.00000000001, 
            // to ensure "multiplier" will always be between 0.0 and .99999999999
            // Otherwise, it's possible for it to be "1", which causes problems in our rounding.
            double multiplier = Math.Max(0, (asciiValueOfRandomCharacter / 255d) - 0.00000000001d);

            // We need to add one to the range, to allow for the rounding done with Math.Floor
            int range = maximumValue - minimumValue + 1;

            double randomValueInRange = Math.Floor(multiplier * range);

            return (int)(minimumValue + randomValueInRange);
        }

    }
}
