using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using NationalInstruments.DAQmx;

namespace Lab12
{
    public partial class Form1 : Form
    {
        // Global variables
        private Task analogReadTask; // Task for analog data acquisition
        private string[] channelNames; // Array to store available DAQ channel names
        private const double MaxAdRate = 250000; // Maximum A/D rate (250 kS/s)
        private const double MaxAcquisitionTime = 9; // Maximum acquisition time in seconds
        private double[,] currentChartData; // 2D array to store the current chart data

        public Form1()
        {
            InitializeComponent();
            InitializeDAQChannels(); // Initialize DAQ channels on form load
        }

        //=================================//
        // Initialization Methods
        //=================================//

        /// <summary>
        /// Initializes the DAQ channels by detecting available devices and populating the dropdown.
        /// </summary>
        private void InitializeDAQChannels()
        {
            try
            {
                // Get the list of available DAQ devices
                channelNames = DaqSystem.Local.Devices;

                // Check if any devices are detected
                if (channelNames == null || channelNames.Length == 0)
                {
                    MessageBox.Show("No NI DAQ channels detected. The NI board might not be powered on or connected.", "DAQ Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    // Populate the dropdown with available channel names
                    cboPorts.Items.AddRange(channelNames);
                    if (cboPorts.Items.Count > 0)
                        cboPorts.SelectedIndex = 0; // Select the first item by default
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}\nThe NI board might not be powered on or connected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Configures the chart with titles, axes, and legends.
        /// </summary>
        private void ConfigureChart()
        {
            // Add a title to the chart
            Title title = chtData.Titles.Add("Voltage vs Time");
            title.Font = new Font("Arial", 20, FontStyle.Bold);
            title.ForeColor = Color.Black;

            // Set the minimum values for the X and Y axes
            chtData.ChartAreas[0].AxisX.Minimum = 0.0;
            chtData.ChartAreas[0].AxisY.Minimum = 0.0;

            // Configure the font for the axis labels
            chtData.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Arial", 14, FontStyle.Bold);
            chtData.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Arial", 14, FontStyle.Bold);

            // Set the titles for the X and Y axes
            chtData.ChartAreas[0].AxisX.Title = "Time (s)";
            chtData.ChartAreas[0].AxisX.TitleFont = new Font("Arial", 14, FontStyle.Bold);
            chtData.ChartAreas[0].AxisY.Title = "Voltage (volts)";
            chtData.ChartAreas[0].AxisY.TitleFont = new Font("Arial", 14, FontStyle.Bold);

            // Configure the legend
            chtData.Legends[0].Alignment = StringAlignment.Center;
            chtData.Legends[0].Docking = Docking.Right;
            chtData.Legends[0].LegendStyle = LegendStyle.Table;
            chtData.Legends[0].TableStyle = LegendTableStyle.Auto;
            chtData.Legends[0].Font = new Font("Arial", 12, FontStyle.Bold);

            // Clear any existing series in the chart
            chtData.Series.Clear();
        }

        //=================================//
        // Event Handlers
        //=================================//

        private void Form1_Load(object sender, EventArgs e)
        {
            ConfigureChart(); // Configure the chart when the form loads
        }

        private void acquireToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AcquireData(); // Start data acquisition when the "Acquire" menu item is clicked
        }

        private void clearChartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chtData.Series.Clear(); // Clear the chart when the "Clear Chart" menu item is clicked
        }

        private void updSampleRate_ValueChanged(object sender, EventArgs e)
        {
            UpdateAcquisitionInfo(); // Update acquisition info when the sample rate changes
        }

        private void updSamplesPerChannel_ValueChanged(object sender, EventArgs e)
        {
            UpdateAcquisitionInfo(); // Update acquisition info when the number of samples per channel changes
        }

        private void updLowChannel_ValueChanged(object sender, EventArgs e)
        {
            UpdateAcquisitionInfo(); // Update acquisition info when the low channel value changes
        }

        private void updHighChannel_ValueChanged(object sender, EventArgs e)
        {
            UpdateAcquisitionInfo(); // Update acquisition info when the high channel value changes
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close(); // Close the application when the "Quit" menu item is clicked
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile(); // Open a CSV file when the "Open" menu item is clicked
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile(false); // Save data to a new CSV file when the "New" menu item is clicked
        }

        private void appendToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile(true); // Append data to an existing CSV file when the "Append" menu item is clicked
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string helpMessage = "Welcome to the Oscilloscope Application!\n\n" +
                                 "• Use the 'Acquire' button to start data acquisition.\n" +
                                 "• Use the 'Open' menu to load data from a CSV file.\n" +
                                 "• Use the 'Save' or 'Append' menu to save or append data to a CSV file.\n" +
                                 "   - Note: When you append data, you can no longer open and graph that data in this program.\n" +
                                 "• Adjust the sample rate, channels, and other settings in the controls.\n\n" +
                                 "Application Limits:\n" +
                                 "   - Maximum sample rate: 250 kS/s\n" +
                                 "   - Maximum acquisition time: 9 seconds\n\n" +
                                 "For more detailed help, refer to the user manual.";

            MessageBox.Show(helpMessage, "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //=================================//
        // Data Acquisition Methods
        //=================================//

        /// <summary>
        /// Starts the data acquisition process.
        /// </summary>
        private void AcquireData()
        {
            try
            {
                if (!ValidateInputs()) // Validate user inputs before proceeding
                    return;

                DisableControls(); // Disable controls during acquisition

                analogReadTask = new Task(); // Create a new DAQ task
                ConfigureTask(); // Configure the task with user settings

                // Create a reader to read multiple samples asynchronously
                AnalogMultiChannelReader reader = new AnalogMultiChannelReader(analogReadTask.Stream);
                reader.BeginReadMultiSample((int)updSamplesPerChannel.Value, new AsyncCallback(ReadCompleted), reader);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                EnableControls(); // Re-enable controls if an error occurs
            }
        }

        /// <summary>
        /// Configures the DAQ task with the selected channels, sample rate, and voltage range.
        /// </summary>
        private void ConfigureTask()
        {
            double selectedRange = GetSelectedVoltageRange(); // Get the selected voltage range
            double minimumValue = -selectedRange; // Set the minimum voltage value
            double maximumValue = selectedRange; // Set the maximum voltage value
            double sampleRate = (double)updSampleRate.Value; // Get the selected sample rate
            int samplesPerChannel = (int)updSamplesPerChannel.Value; // Get the number of samples per channel

            AITerminalConfiguration terminalConfig = GetTerminalConfiguration(); // Get the terminal configuration

            // Create voltage channels for each selected channel
            for (int i = (int)updLowChannel.Value; i <= (int)updHighChannel.Value; i++)
            {
                string physicalChannel = $"{cboPorts.Text}/ai{i}";
                analogReadTask.AIChannels.CreateVoltageChannel(physicalChannel, "", terminalConfig, minimumValue, maximumValue, AIVoltageUnits.Volts);
            }

            // Configure the sample clock for the task
            analogReadTask.Timing.ConfigureSampleClock("", sampleRate, SampleClockActiveEdge.Rising, SampleQuantityMode.FiniteSamples, samplesPerChannel);
            analogReadTask.Control(TaskAction.Verify); // Verify the task configuration
        }

        /// <summary>
        /// Callback method that is called when the data acquisition is completed.
        /// </summary>
        private void ReadCompleted(IAsyncResult result)
        {
            try
            {
                AnalogMultiChannelReader reader = (AnalogMultiChannelReader)result.AsyncState;
                double[,] data = reader.EndReadMultiSample(result); // Get the acquired data
                currentChartData = data; // Store the data for later use

                double selectedRange = GetSelectedVoltageRange(); // Get the selected voltage range
                double sampleRate = (double)updSampleRate.Value; // Get the sample rate

                // Update the chart with the acquired data
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
                this.Invoke((MethodInvoker)EnableControls); // Re-enable controls after acquisition
                analogReadTask?.Dispose(); // Dispose of the task
            }
        }

        /// <summary>
        /// Graphs the acquired data on the chart.
        /// </summary>
        private void GraphData(double[,] data, double minRange, double maxRange, double sampleRate)
        {
            chtData.Series.Clear(); // Clear any existing series
            chtData.ChartAreas[0].AxisY.Minimum = minRange; // Set the Y-axis minimum
            chtData.ChartAreas[0].AxisY.Maximum = maxRange; // Set the Y-axis maximum

            int startChannel = (int)updLowChannel.Value; // Get the starting channel number

            // Add a series for each channel and plot the data
            for (int i = 0; i < data.GetLength(0); i++)
            {
                Series series = chtData.Series.Add($"Channel ai{startChannel + i}");
                series.ChartType = SeriesChartType.Line;

                for (int j = 0; j < data.GetLength(1); j++)
                {
                    double time = j / sampleRate; // Calculate the time for each sample
                    series.Points.AddXY(time, data[i, j]); // Add the data point to the series
                }
            }
        }

        //=================================//
        // File Handling Methods
        //=================================//

        /// <summary>
        /// Opens a CSV file and graphs the data.
        /// </summary>
        private void OpenFile()
        {
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                openFileDialog.Title = "Open Data File";
                openFileDialog.Multiselect = false;
                openFileDialog.RestoreDirectory = true;

                //clear the chart before loading new data
                chtData.Series.Clear();

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                    try
                    {
                        using (StreamReader reader = new StreamReader(filePath))
                        {
                            List<double[]> data = new List<double[]>();
                            int numChannels = 0;

                            // Skip the header lines and extract the number of channels
                            for (int i = 0; i < 4; i++)
                            {
                                if (i == 3)
                                {
                                    string headerLine = reader.ReadLine();
                                    string[] headerColumns = headerLine.Split(',');
                                    numChannels = headerColumns.Length - 1;

                                    // Initialize the chart series based on the number of channels
                                    for (int j = 0; j < numChannels; j++)
                                    {
                                        chtData.Series.Add($"Channel ai{(int)updLowChannel.Value + j}");
                                        chtData.Series[j].ChartType = SeriesChartType.Line;
                                    }
                                }
                                else
                                {
                                    reader.ReadLine();
                                }
                            }

                            // Read the data rows
                            string line;
                            while ((line = reader.ReadLine()) != null)
                            {
                                string[] columns = line.Split(',');
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
                                        throw new FormatException("Invalid data format in CSV file. You cannot graph data that has been appended.");
                                    }
                                }

                                data.Add(row);
                            }

                            // Clear the chart and set the Y-axis range
                            chtData.Series.Clear();
                            chtData.ChartAreas[0].AxisY.Minimum = -10;
                            chtData.ChartAreas[0].AxisY.Maximum = 10;

                            int startChannel = (int)updLowChannel.Value;

                            // Add a series for each channel and plot the data
                            for (int i = 0; i < numChannels; i++)
                            {
                                Series series = chtData.Series.Add($"Channel ai{startChannel + i}");
                                series.ChartType = SeriesChartType.Line;

                                for (int j = 0; j < data.Count; j++)
                                {
                                    double time = data[j][0];
                                    double voltage = data[j][i + 1];
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
        }

        /// <summary>
        /// Saves the current chart data to a CSV file.
        /// </summary>
        /// <param name="append">If true, appends data to an existing file. If false, creates a new file.</param>
        private void SaveFile(bool append)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
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
                        using (StreamWriter writer = new StreamWriter(filePath, append))
                        {
                            // Write the header information
                            writer.WriteLine($"Date: {DateTime.Today}");
                            writer.WriteLine($"Time: {DateTime.Now}");
                            writer.WriteLine($"# Points: {updSamplesPerChannel.Value}");

                            // Create a header row with channel names
                            string[] header = new string[chtData.Series.Count + 1];
                            header[0] = "Elapsed Time";
                            var seriesCount = 0;

                            foreach (var series in chtData.Series)
                            {
                                header[seriesCount + 1] = series.Name;
                                seriesCount++;
                            }

                            writer.WriteLine(string.Join(",", header));

                            // Write the data rows
                            for (int timeStep = 0; timeStep < currentChartData.GetLength(1); timeStep++)
                            {
                                string[] row = new string[currentChartData.GetLength(0) + 1];
                                double elapsedTime = timeStep / (double)updSampleRate.Value;
                                row[0] = elapsedTime.ToString("F6");

                                for (int channel = 0; channel < currentChartData.GetLength(0); channel++)
                                {
                                    row[channel + 1] = currentChartData[channel, timeStep].ToString("F3");
                                }

                                writer.WriteLine(string.Join(",", row));
                            }

                            MessageBox.Show("File saved successfully!");
                        }
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
        }

        //=================================//
        // Helper Methods
        //=================================//

        /// <summary>
        /// Updates the acquisition information based on user inputs.
        /// </summary>
        private void UpdateAcquisitionInfo()
        {
            int numberOfChannels = (int)updHighChannel.Value - (int)updLowChannel.Value + 1;
            double sampleRate = (double)updSampleRate.Value;
            int samplesPerChannel = (int)updSamplesPerChannel.Value;

            double adRate = sampleRate * numberOfChannels; // Calculate the A/D rate
            double acquisitionTime = samplesPerChannel / sampleRate; // Calculate the acquisition time

            txtADRate.Text = $"{adRate} samples/s"; // Display the A/D rate
            txtAcqTime.Text = $"{acquisitionTime} s"; // Display the acquisition time
        }

        /// <summary>
        /// Validates user inputs to ensure they are within acceptable limits.
        /// </summary>
        private bool ValidateInputs()
        {
            int numberOfChannels = (int)updHighChannel.Value - (int)updLowChannel.Value + 1;
            double sampleRate = (double)updSampleRate.Value;
            int samplesPerChannel = (int)updSamplesPerChannel.Value;

            // Check if the low channel is greater than the high channel
            if (updLowChannel.Value > updHighChannel.Value)
            {
                MessageBox.Show("Low channel cannot be greater than high channel.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Check if the acquisition time exceeds the maximum allowed
            if (samplesPerChannel / sampleRate > MaxAcquisitionTime)
            {
                MessageBox.Show("Acquisition time cannot exceed 9 seconds.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Check if the A/D rate exceeds the maximum allowed
            if (sampleRate * numberOfChannels > MaxAdRate)
            {
                MessageBox.Show($"A/D rate cannot exceed {MaxAdRate} samples/s.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Disables all controls on the form.
        /// </summary>
        private void DisableControls()
        {
            foreach (Control control in this.Controls)
            {
                control.Enabled = false;
            }
        }

        /// <summary>
        /// Enables all controls on the form.
        /// </summary>
        private void EnableControls()
        {
            foreach (Control control in this.Controls)
            {
                control.Enabled = true;
            }
        }

        /// <summary>
        /// Gets the selected voltage range from the dropdown.
        /// </summary>
        private double GetSelectedVoltageRange()
        {
            string numericPart = new string(cboVoltageRange.Text.Where(char.IsDigit).ToArray());
            double.TryParse(numericPart, out double selectedRange);
            return selectedRange;
        }

        /// <summary>
        /// Gets the terminal configuration based on the selected dropdown value.
        /// </summary>
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
    }
}