using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FakeGambling
{
    public partial class Crash : Form
    {
        // Set the variables used by the program
        private decimal Multiplier;
        private bool Cashit = false;
        private decimal Cash = 0;
        private decimal Bet;
        // Change this value to change the starting cash of the player.
        private const decimal DefaultCash = 10.00M;
        private int Nummer2;
        private decimal CrashPoint;
        // Load on form load.
        public Crash()
        {
            InitializeComponent();
            // Set current cash to the constant default cash.
            Cash = DefaultCash;
            // Update text on screen
            label5.Text = Cash + "$";
        }

        private void NewCrashPoint()
        {
            // Use RNGCryptoService function to create a very large random number, making the multiplier small.
            Nummer2 = Values.Between(1, 1000000000);
            CrashPoint = 999999999 / Convert.ToDecimal(Nummer2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Test that the text the player placed is a number and not some words.
            try
            {
                Bet = Convert.ToDecimal(textBox2.Text);
            }
            catch (Exception)
            {
                // Ignore if it is text.   
            }
            // Test that the bet does not exceed current cash value
            if (Cash >= Bet)
            {
                // Disable bet button, and enable the cash out button.
                button1.Enabled = false;
                button1.Visible = false;
                button2.Visible = true;
                // Disable the bet textboxes.
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                // Remove bet $$$ from the cash ammount
                Cash -= Bet;
                // Update the cash on screen
                label5.Text = Cash + "$";
                // Generate new random crash multiplier
                NewCrashPoint();
                // Check that the 'last bets' text box is not full, if it is, clear it.
                if (richTextBox1.Lines.Length >= 18)
                {
                    richTextBox1.Clear();
                }
                // Begin the animation once all the presets are done.
                Animate();
            }
            else if (Cash < Bet)
            {
                // Display to the player that their bet is too high, and reset the bet value.
                MessageBox.Show("You do not have enough money to make that bet!");
                Bet = 0;
            }
        }

        private async void Animate()
        {
            // Animate the script, up to a multiplier of 1 billion and increasing by 0.01
            for (Multiplier = 1.00M; Multiplier <= 1000000000; Multiplier = Multiplier + 0.01M)
            {
                // Update the multiplier on screen so that the player can see the current multiplier.
                label6.Text = Convert.ToString(Multiplier) + "x";
                // Check if the multiplier is the same as the player's auto cash out value.
                if (Multiplier >= Convert.ToDecimal(textBox1.Text))
                {
                    // If it is, disable the cash out button, freeze the multiplier and change it green to tell the player they've won
                    button2.Enabled = false;
                    label6.ForeColor = System.Drawing.Color.Green;
                    // Add the money to the player's account and update it on screen
                    Cash += Bet * Multiplier;
                    label5.Text = Cash + "$";
                    // Exit the loop, stopping the animation.
                    break;
                }
                // Check if the multiplier is the same as the random crash multiplier
                else if (Multiplier >= CrashPoint)
                {
                    // Disable the cash out button
                    button2.Enabled = false;
                    // Change the multiplier color to red to tell the player they've lost
                    label6.ForeColor = System.Drawing.Color.Red;
                    // Exit the loop, stopping the animation.
                    break;
                }
                // Check to see if the player has pressed the 'cash out' button
                else if (Cashit == true)
                {
                    // (Yes, crashpoint takes preference between the button and the crash $$$)
                    // Player has pressed the button, turn the multiplier green to tell him he's won.
                    label6.ForeColor = System.Drawing.Color.Green;
                    // Add the money to the player's account
                    Cash += Bet * Multiplier;
                    // Update money on screen
                    label5.Text = Cash + "$";
                    // Exit the loop, stopping the animation.
                    break;
                }
                // Slow the script down to prevent the program from freezing.
                await Task.Delay(1);
            }
            // Wait 3 seconds
            await Task.Delay(3000);
            // Add the actual, exact value the game crashed at.
            richTextBox1.Text += "Value crashed at: " + CrashPoint + "\r\n";
            // Reset the multiplier and turn the color back to black.
            label6.Text = "1.00x";
            label6.ForeColor = System.Drawing.Color.Black;
            // disable the cash out button, enable the bet button and enable the betting selection.
            button1.Enabled = true;
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            button1.Visible = true;
            button2.Visible = false;
            button2.Enabled = true;
            // Turn the teller, for the loop, that the player has cashed out, off.
            Cashit = false;
            // Check if the player still has money. If he doesn't, display a "You've lost!" message and reset the program.
            if (Cash <= 0)
            {
                MessageBox.Show("You Lost! Restarting program...");
                Values.ForcedExit = true;
                Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Tell the loop on the animation that the player wants to cash out.
            Cashit = true;
            // Disable the button to avoid click spam.
            button2.Enabled = false;
        }
    }
}
