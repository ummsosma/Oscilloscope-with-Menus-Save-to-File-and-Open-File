using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using NationalInstruments.DAQmx;

namespace Lab12
{
    public partial class Form1 : Form
    {
        // Global variables
        private NationalInstruments.DAQmx.Task analogReadTask;
        private string[] channelNames;
        private const double MaxAdRate = 250000; // 250 kS/s board's max A/D rate
        private const double MaxAcquisitionTime = 9; // seconds
        private double[,] currentChartData;

        public Form1()
        {
            InitializeComponent();
            InitializeDAQChannels();
        }

        //=================================//
        // Initialization
        //=================================//

        private void InitializeDAQChannels()
        {
            try
            {
                channelNames = DaqSystem.Local.Devices;

                if (channelNames == null || channelNames.Length == 0)
                {
                    MessageBox.Show("No NI DAQ channels detected. The NI board might not be powered on or connected.", "DAQ Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    cboPorts.Items.AddRange(channelNames);
                    if (cboPorts.Items.Count > 0)
                        cboPorts.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}\nThe NI board might not be powered on or connected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //=================================//
        // Event Handlers
        //=================================//

        private void Form1_Load(object sender, EventArgs e)
        {
            ConfigureChart();
        }
        private void acquireToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AcquireData();
        }

        private void clearChartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chtData.Series.Clear();
        }

        private void updSampleRate_ValueChanged(object sender, EventArgs e)
        {
            UpdateAcquisitionInfo();
        }

        private void updSamplesPerChannel_ValueChanged(object sender, EventArgs e)
        {
            UpdateAcquisitionInfo();
        }

        private void updLowChannel_ValueChanged(object sender, EventArgs e)
        {
            UpdateAcquisitionInfo();
        }

        private void updHighChannel_ValueChanged(object sender, EventArgs e)
        {
            UpdateAcquisitionInfo();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {

            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.FileName = "data.csv";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Title = "Save Data File";
            saveFileDialog.OverwritePrompt = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                try
                {
                    using (StreamWriter writer = new StreamWriter(filePath))
                    {
                        //Write the header information to the csv file
                        writer.WriteLine($"Date: {System.DateTime.Today}");
                        writer.WriteLine($"Time: {System.DateTime.Now}");
                        writer.WriteLine($"# Points:");
                        //Make a column header row
                        string[] header = new string[chtData.Series.Count + 1];
                        header[0] = "Elapsed Time";
                        var seriesCount = 0;
                        //Add the series names to the header
                        foreach (var series in chtData.Series)
                        {
                            header[seriesCount + 1] = series.Name;
                            seriesCount++;
                        }
                        //Write the header to the file
                        writer.WriteLine(string.Join(",", header));

                        // Write the data from the chart to the CSV file
                        for (int timeStep = 0; timeStep < currentChartData.GetLength(1); timeStep++) // Loop through each column
                        {
                            // Create a row to hold the data for this time step
                            string[] row = new string[currentChartData.GetLength(0) + 1]; // +1 for the elapsed time column

                            // Calculate and add the elapsed time to the row
                            double elapsedTime = timeStep / (double)updSampleRate.Value; // Elapsed time in seconds
                            row[0] = elapsedTime.ToString("F3"); // Format to 3 decimal places

                            // Loop through each channel (rows) to add voltage values to the row
                            for (int channel = 0; channel < currentChartData.GetLength(0); channel++)
                            {
                                // Add the voltage value for this channel and time step
                                row[channel + 1] = currentChartData[channel, timeStep].ToString("F3"); // Format to 3 decimal places
                            }

                            // Write the row to the CSV file as a comma-separated string
                            writer.WriteLine(string.Join(",", row));
                        }

                        // Close the file after writing all data
                        writer.Close();
                    }
                    MessageBox.Show("File saved successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving file: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Save operation canceled.");
            }
        }

        //=================================//
        // Helper Methods
        //=================================//

        private void ConfigureChart()
        {
            Title title = chtData.Titles.Add("Voltage vs Time");
            title.Font = new Font("Arial", 20, FontStyle.Bold);
            title.ForeColor = Color.Black;

            chtData.ChartAreas[0].AxisX.Minimum = 0.0;
            chtData.ChartAreas[0].AxisY.Minimum = 0.0;

            chtData.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Arial", 14, FontStyle.Bold);
            chtData.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Arial", 14, FontStyle.Bold);

            chtData.ChartAreas[0].AxisX.Title = "Time (s)";
            chtData.ChartAreas[0].AxisX.TitleFont = new Font("Arial", 14, FontStyle.Bold);
            chtData.ChartAreas[0].AxisY.Title = "Voltage (volts)";
            chtData.ChartAreas[0].AxisY.TitleFont = new Font("Arial", 14, FontStyle.Bold);

            chtData.Legends[0].Alignment = StringAlignment.Center;
            chtData.Legends[0].Docking = Docking.Right;
            chtData.Legends[0].LegendStyle = LegendStyle.Table;
            chtData.Legends[0].TableStyle = LegendTableStyle.Auto;
            chtData.Legends[0].Font = new Font("Arial", 12, FontStyle.Bold);

            chtData.Series.Clear();
        }

        private void AcquireData()
        {
            try
            {
                if (!ValidateInputs())
                    return;

                DisableControls();

                analogReadTask = new NationalInstruments.DAQmx.Task();
                ConfigureTask();

                AnalogMultiChannelReader reader = new AnalogMultiChannelReader(analogReadTask.Stream);
                reader.BeginReadMultiSample((int)updSamplesPerChannel.Value, new AsyncCallback(ReadCompleted), reader);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                EnableControls();
            }
        }

        private void ConfigureTask()
        {
            double selectedRange = GetSelectedVoltageRange();
            double minimumValue = -selectedRange;
            double maximumValue = selectedRange;
            double sampleRate = (double)updSampleRate.Value;
            int samplesPerChannel = (int)updSamplesPerChannel.Value;

            AITerminalConfiguration terminalConfig = GetTerminalConfiguration();

            for (int i = (int)updLowChannel.Value; i <= (int)updHighChannel.Value; i++)
            {
                string physicalChannel = $"{cboPorts.Text}/ai{i}";
                analogReadTask.AIChannels.CreateVoltageChannel(physicalChannel, "", terminalConfig, minimumValue, maximumValue, AIVoltageUnits.Volts);
            }

            analogReadTask.Timing.ConfigureSampleClock("", sampleRate, SampleClockActiveEdge.Rising, SampleQuantityMode.FiniteSamples, samplesPerChannel);
            analogReadTask.Control(TaskAction.Verify);
        }

        private double GetSelectedVoltageRange()
        {
            string numericPart = new string(cboVoltageRange.Text.Where(char.IsDigit).ToArray());
            double.TryParse(numericPart, out double selectedRange);
            return selectedRange;
        }

        private AITerminalConfiguration GetTerminalConfiguration()
        {
            switch (cboTerminal.Text)
            {
                case "Differential":
                    return AITerminalConfiguration.Differential;
                case "Nrse":
                    return AITerminalConfiguration.Nrse;
                case "Rse":
                    return AITerminalConfiguration.Rse;
                default:
                    return AITerminalConfiguration.Nrse;
            }
        }

        private void ReadCompleted(IAsyncResult result)
        {
            try
            {
                AnalogMultiChannelReader reader = (AnalogMultiChannelReader)result.AsyncState;
                double[,] data = reader.EndReadMultiSample(result);
                currentChartData = data;

                double selectedRange = GetSelectedVoltageRange();
                double sampleRate = (double)updSampleRate.Value;

                this.Invoke((MethodInvoker)delegate
                {
                    GraphData(data, -selectedRange, selectedRange, sampleRate);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Invoke((MethodInvoker)EnableControls);
                analogReadTask?.Dispose();
            }
        }

        private void GraphData(double[,] data, double minRange, double maxRange, double sampleRate)
        {
            chtData.Series.Clear();
            chtData.ChartAreas[0].AxisY.Minimum = minRange;
            chtData.ChartAreas[0].AxisY.Maximum = maxRange;

            // Get the starting channel number from the low channel value
            int startChannel = (int)updLowChannel.Value;

            for (int i = 0; i < data.GetLength(0); i++)
            {
                // Use the correct channel number (e.g., ai0, ai1, etc.)
                Series series = chtData.Series.Add($"Channel ai{startChannel + i}");
                series.ChartType = SeriesChartType.Line;

                for (int j = 0; j < data.GetLength(1); j++)
                {
                    double time = j / sampleRate;
                    series.Points.AddXY(time, data[i, j]);
                }
            }
        }

        private void UpdateAcquisitionInfo()
        {
            int numberOfChannels = (int)updHighChannel.Value - (int)updLowChannel.Value + 1;
            double sampleRate = (double)updSampleRate.Value;
            int samplesPerChannel = (int)updSamplesPerChannel.Value;

            double adRate = sampleRate * numberOfChannels;
            double acquisitionTime = samplesPerChannel / sampleRate;

            txtADRate.Text = $"{adRate} samples/s";
            txtAcqTime.Text = $"{acquisitionTime} s";
        }

        private bool ValidateInputs()
        {
            int numberOfChannels = (int)updHighChannel.Value - (int)updLowChannel.Value + 1;
            double sampleRate = (double)updSampleRate.Value;
            int samplesPerChannel = (int)updSamplesPerChannel.Value;

            if (updLowChannel.Value > updHighChannel.Value)
            {
                MessageBox.Show("Low channel cannot be greater than high channel.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (samplesPerChannel / sampleRate > MaxAcquisitionTime)
            {
                MessageBox.Show("Acquisition time cannot exceed 9 seconds.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (sampleRate * numberOfChannels > MaxAdRate)
            {
                MessageBox.Show($"A/D rate cannot exceed {MaxAdRate} samples/s.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void DisableControls()
        {
            foreach (Control control in this.Controls)
            {
                control.Enabled = false;
            }
        }

        private void EnableControls()
        {
            foreach (Control control in this.Controls)
            {
                control.Enabled = true;
            }
        }
    }
}