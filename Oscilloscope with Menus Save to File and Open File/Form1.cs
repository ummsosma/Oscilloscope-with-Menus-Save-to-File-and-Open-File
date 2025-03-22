using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using NationalInstruments.DAQmx;
using NationalInstruments.DataInfrastructure.Descriptors;

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
            var filePath = string.Empty;

            // Open file dialog to select a CSV file
            OpenFileDialog openFileDialog = new OpenFileDialog();
            // Set filter for file types
            openFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            // Set filter index to 1 (CSV files)
            openFileDialog.FilterIndex = 1;
            // Set default file name
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            // Set title for the dialog
            openFileDialog.Title = "Open Data File";
            // Allow only single file selection
            openFileDialog.Multiselect = false;
            // Restore the directory after closing the dialog
            openFileDialog.RestoreDirectory = true;

            // Show the dialog and check if the user selected a file
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
                int numChannels = 0; // Initialize numChannels

                try
                {
                    // Read the contents of the file
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        // Initialize a list to store the data
                        List<double[]> data = new List<double[]>();

                        // Skip the header lines (first 4 lines)
                        for (int i = 0; i < 4; i++)
                        {
                            // Extract the number of channels from the header
                            if (i == 3)
                            {
                                string headerLine = reader.ReadLine();
                                string[] headerColumns = headerLine.Split(',');
                                numChannels = headerColumns.Length - 1; // Subtract 1 for the elapsed time column

                                // Initialize the chart series based on the number of channels
                                for (int j = 0; j < numChannels; j++)
                                {
                                    chtData.Series.Add($"Channel ai{(int)updLowChannel.Value + j}");
                                    chtData.Series[j].ChartType = SeriesChartType.Line;
                                }
                            }
                            else
                            {
                                reader.ReadLine(); // Skip the header lines
                            }
                        }

                        // Read the data rows
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            // Split the line into columns
                            string[] columns = line.Split(',');

                            // Convert the columns to double and store in a row array
                            double[] row = new double[columns.Length];

                            // Parse the columns into double values
                            for (int i = 0; i < columns.Length; i++)
                            {
                                if (double.TryParse(columns[i], out double value))
                                {
                                    row[i] = value;
                                }
                                else
                                {
                                    throw new FormatException("Invalid data format in CSV file. You cannot graph data that has been appeneded");
                                }
                            }

                            // Add the row to the data list
                            data.Add(row);
                        }

                        // Graph the data from the file
                        chtData.Series.Clear();
                        chtData.ChartAreas[0].AxisY.Minimum = -10;
                        chtData.ChartAreas[0].AxisY.Maximum = 10;

                        // Get the starting channel number from the low channel value
                        int startChannel = (int)updLowChannel.Value;

                        // Add a series for each channel
                        for (int i = 0; i < numChannels; i++)
                        {
                            Series series = chtData.Series.Add($"Channel ai{startChannel + i}");
                            series.ChartType = SeriesChartType.Line;

                            // Add data points to the series
                            for (int j = 0; j < data.Count; j++)
                            {
                                double time = data[j][0]; // Elapsed time is the first column
                                double voltage = data[j][i + 1]; // Voltage values start from the second column
                                series.Points.AddXY(time, voltage);
                            }
                        }

                        MessageBox.Show("File opened and graphed successfully!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error opening file: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Open operation canceled.");
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
                        writer.WriteLine($"# Points: {updSamplesPerChannel.Value}");
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
                            row[0] = elapsedTime.ToString("F6"); // Format to 6 decimal places

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

        private void appendToolStripMenuItem_Click(object sender, EventArgs e)
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
                    // Open the file in append mode, allowing new data to be added without overwriting existing data
                    // Use StreamWriter with append mode set to true
                    using (StreamWriter writer = new StreamWriter(filePath, true))
                    {
                        //Write the header information to the csv file
                        writer.WriteLine($"Date: {System.DateTime.Today}");
                        writer.WriteLine($"Time: {System.DateTime.Now}");
                        writer.WriteLine($"# Points: {updSamplesPerChannel.Value}");
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
                            row[0] = elapsedTime.ToString("F6"); // Format to 6 decimal places

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