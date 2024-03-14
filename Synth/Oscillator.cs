using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Synth
{
    public class Oscillator: GroupBox
    {
        public Oscillator()
        {
            this.Controls.Add(new Button()
            {
                Name = "Sine",
                Location = new Point(10, 15),
                Text = "Sine",
                BackColor = Color.Yellow


            }) ;
            this.Controls.Add(new Button()
            {
                Name = "Saw",
                Location = new Point(65, 15),
                Text = "Saw",



            });
            this.Controls.Add(new Button()
            {
                Name = "Square",
                Location = new Point(120, 15),
                Text = "Square",



            });
            this.Controls.Add(new Button()
            {
                Name = "Triangle",
                Location = new Point(10, 50),
                Text = "Triangle",



            });
            this.Controls.Add(new Button()
            {
                Name = "Noise",
                Location = new Point(65, 50),
                Text = "Noise",



            });
            foreach (Control c in this.Controls)
            {
                c.Size = new Size(50, 30);
                c.Font = new Font("Microsoft San Serif", 6.75f);
                c.Click += waveButton_Click;
            }


            this.Controls.Add(new CheckBox() 
            { 
            
              Name = "OscillatorOn",
              Location =new Point(210, 10),
              Size = new Size(40, 30),
              Text="On",
              Checked = true
            
            
            });



        }

        public waveForm waveform { get; private set; }
        public bool on => ((CheckBox)this.Controls["OscillatorOn"]).Checked;

        private void waveButton_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            this.waveform = (waveForm)Enum.Parse(typeof(waveForm), button.Text);
            foreach (Button b in this.Controls.OfType<Button>())
            {
                b.UseVisualStyleBackColor = true;
            }
            button.BackColor= Color.Yellow;
        }





    }
}

