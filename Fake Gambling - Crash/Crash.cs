using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FakeGambling
{
    public partial class Crash : Form
    {
        private decimal Multiplier;
        private bool Cashit = false;
        private decimal Cash = 0;
        private decimal Bet;
        private const decimal DefaultCash = 10.00M;
        private int Nummer2;
        private decimal CrashPoint;
        public Crash()
        {
            InitializeComponent();
            Cash = DefaultCash;
            label5.Text = Cash + "$";
        }

        private void NewCrashPoint()
        {
            Nummer2 = Values.Between(1, 1000000000);
            CrashPoint = 999999999 / Convert.ToDecimal(Nummer2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Bet = Convert.ToDecimal(textBox2.Text);
            }
            catch (Exception)
            {
                
            }
            if (Cash >= Bet)
            {
                button1.Enabled = false;
                button1.Visible = false;
                button2.Visible = true;
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                Cash -= Bet;
                label5.Text = Cash + "$";
                NewCrashPoint();
                if (richTextBox1.Lines.Length >= 18)
                {
                    richTextBox1.Clear();
                }
                Animate();
            }
            else if (Cash < Bet)
            {
                MessageBox.Show("You do not have enough money to make that bet!");
                Bet = 0;
            }
        }

        private async void Animate()
        {
            for (Multiplier = 1; Multiplier <= 1000000; Multiplier = Multiplier + 0.01M)
            {
                label6.Text = Convert.ToString(Multiplier) + "x";
                await Task.Delay(1);
                if (Multiplier >= Convert.ToDecimal(textBox1.Text))
                {
                    button2.Enabled = false;
                    label6.ForeColor = System.Drawing.Color.Green;
                    Cash += Bet * Multiplier;
                    label5.Text = Cash + "$";
                    break;
                }
                else if (Multiplier >= CrashPoint)
                {
                    button2.Enabled = false;
                    label6.ForeColor = System.Drawing.Color.Red;
                    break;
                }
                else if (Cashit == true)
                {
                    label6.ForeColor = System.Drawing.Color.Green;
                    Cash += Bet * Multiplier;
                    label5.Text = Cash + "$";
                    break;
                }
            }
            await Task.Delay(3000);
            richTextBox1.Text += "Value crashed at: " + CrashPoint + "\r\n";
            label6.Text = "1.00x";
            label6.ForeColor = System.Drawing.Color.Black;
            button1.Enabled = true;
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            button1.Visible = true;
            button2.Visible = false;
            button2.Enabled = true;
            Cashit = false;
            if (Cash <= 0)
            {
                MessageBox.Show("You Lost! Restarting program...");
                Values.ForcedExit = true;
                Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Cashit = true;
            button2.Enabled = false;
        }
    }
}
