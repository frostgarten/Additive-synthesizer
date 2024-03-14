using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.IO;

namespace Synth
{
    public partial class Synth : Form
    {
        public const int SAMPLE_RATE = 44100;
        public const short BITS_PER_SAMPLE = 16;
        public Synth()
        {
            InitializeComponent();
        }

        private void Synth_KeyDown(object sender, KeyEventArgs e)
        {
            IEnumerable<Oscillator> oscillators = this.Controls.OfType<Oscillator>().Where(o =>o.on);
            short[] wave = new short[SAMPLE_RATE];
            byte[] binaryWave = new byte[SAMPLE_RATE * sizeof(short)];
            float frequency;
            int oscillatorsCount = oscillators.Count();
            switch (e.KeyCode)
            {
                case Keys.Z:
                    frequency = 65.4f;
                    break;
                case Keys.X:
                    frequency = 138.59f;
                    break;
                case Keys.C:
                    frequency = 261.62f;
                    break;
                case Keys.V:
                    frequency = 523.25f;
                    break;
                case Keys.B:
                    frequency = 1046.5f;
                    break;
                case Keys.N:
                    frequency = 2093f;
                    break;
                case Keys.M:
                    frequency = 4186.01f;
                    break;
                default:
                    return;

            }

            foreach (Oscillator oscillator in oscillators)
            {
                int samplesPerWaveLength = (int)(SAMPLE_RATE / frequency);
                short tempSample;
                short ampStep = (short)((short.MaxValue * 2) / samplesPerWaveLength);
                Random random = new Random();
                switch (oscillator.waveform)
                {
                    case waveForm.Sine:
                        for (int i = 0; i < SAMPLE_RATE; i++)
                        {
                            wave[i] += Convert.ToInt16((short.MaxValue * Math.Sin(((Math.PI * 2 * frequency) / SAMPLE_RATE) * i))/oscillatorsCount);
                        }
                        break;
                    case waveForm.Square:
                        for (int i = 0; i < SAMPLE_RATE; i++)
                        {
                            wave[i] += Convert.ToInt16((short.MaxValue * Math.Sign(Math.Sin((Math.PI * 2 * frequency) / SAMPLE_RATE * i)))/oscillatorsCount);
                        }
                        break;
                    case waveForm.Saw:
                        for (int i = 0; i < SAMPLE_RATE; i++)
                        {
                            tempSample = -short.MaxValue;
                            for (int j = 0; j < SAMPLE_RATE; j++)
                            {
                                tempSample += ampStep;
                                wave[i++] += Convert.ToInt16(tempSample/oscillatorsCount);
                            }
                            i--;

                        }
                        break;
                    case waveForm.Triangle:
                        tempSample = -short.MaxValue;
                        for (int i = 0; i < SAMPLE_RATE; i++)
                        {
                            if (Math.Abs(tempSample + ampStep) > short.MaxValue)
                            {
                                ampStep = (short)-ampStep;
                            }
                            tempSample += ampStep;
                            wave[i] += Convert.ToInt16(tempSample/oscillatorsCount);
                        }
                        break;
                    case waveForm.Noise:
                        for (int i = 0; i < SAMPLE_RATE; i++)
                        {
                            wave[i] += Convert.ToInt16(random.Next(-short.MaxValue, short.MaxValue)/oscillatorsCount);
                        }
                        break;





                }








            }
            Buffer.BlockCopy(wave, 0, binaryWave, 0, wave.Length * sizeof(short));
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter binaryWriter = new BinaryWriter(ms))
            {
                short blockAlign = BITS_PER_SAMPLE / 8;
                int subCHunk2size = SAMPLE_RATE * blockAlign;
                binaryWriter.Write(new[] { 'R', 'I', 'F', 'F' });
                binaryWriter.Write(36 + subCHunk2size);
                binaryWriter.Write(new[] { 'W', 'A', 'V', 'E', 'f', 'm', 't', ' ' });
                binaryWriter.Write(16);
                binaryWriter.Write((short)1);
                binaryWriter.Write((short)1);
                binaryWriter.Write(SAMPLE_RATE);
                binaryWriter.Write(SAMPLE_RATE * blockAlign);
                binaryWriter.Write(blockAlign);
                binaryWriter.Write(BITS_PER_SAMPLE);
                binaryWriter.Write(new[] { 'd', 'a', 't', 'a' });
                binaryWriter.Write(subCHunk2size);
                binaryWriter.Write(binaryWave);
                ms.Position = 0;
                new SoundPlayer(ms).Play();
            }
        }

        private void oscillator1_Enter(object sender, EventArgs e)
        {

        }
    }

    public enum waveForm 
    { 
        Sine, Square, Saw, Triangle, Noise


    
    
    
    }

}
