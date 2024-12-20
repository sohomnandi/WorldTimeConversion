using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WorldTimeConversion;

namespace WorldTimeConversion
{
    public partial class MainWindow : Window
    {
        private Dictionary<string, string> timeZoneData; // Dictionary to hold time zone data

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow1_Loaded; // Attach Loaded event
        }

        private void MainWindow1_Loaded(object sender, RoutedEventArgs e)
        {
            lblUtcTimeResult.Content = string.Empty;
            lblLocalTimeResult.Content = string.Empty;

            LoadTimeZoneDataFromResourceFile(); // Load time zones
            PopulateTimezonesIntoComboBox(); // Populate ComboBox with time zones
        }

        private void LoadTimeZoneDataFromResourceFile()
        {
            timeZoneData = new Dictionary<string, string>();

            try
            {
                var resourceSet = TZDictionary.ResourceManager
                    .GetResourceSet(System.Globalization.CultureInfo.InvariantCulture, true, true);

                if (resourceSet == null)
                {
                    MessageBox.Show("No resource set found.");
                    Debug.WriteLine("Resource set is null.");
                    return;
                }

                foreach (System.Collections.DictionaryEntry entry in resourceSet)
                {
                    string key = entry.Key.ToString();
                    string value = entry.Value?.ToString();

                    if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(value))
                    {
                        timeZoneData[key] = value;
                    }
                }

                Debug.WriteLine($"Loaded {timeZoneData.Count} time zones from resource file.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading time zones: {ex.Message}");
                Debug.WriteLine($"Exception: {ex}");
            }
        }

        private void PopulateTimezonesIntoComboBox()
        {
            if (timeZoneData == null || timeZoneData.Count == 0)
            {
                MessageBox.Show("No time zones found.");
                return;
            }

            comboBoxTimeZones.Items.Clear();
            foreach (var key in timeZoneData.Keys.OrderBy(k => k)) // Sort keys alphabetically
            {
                comboBoxTimeZones.Items.Add(key);
            }

            if (comboBoxTimeZones.Items.Count > 0)
                comboBoxTimeZones.SelectedIndex = 0; // Select the first item
        }

        private void btnConvertToUTC_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidateDate(datePickerLocal, out DateTime localDate) &&
                    ValidateTime(txtLocalTime.Text, out DateTime localTime) &&
                    comboBoxTimeZones.SelectedItem != null)
                {
                    string selectedTimeZoneKey = comboBoxTimeZones.SelectedItem.ToString();

                    if (!timeZoneData.TryGetValue(selectedTimeZoneKey, out string timezoneResourceValue))
                    {
                        MessageBox.Show("Timezone resource value not found.");
                        return;
                    }

                    DateTime localDateTime = new DateTime(localDate.Year, localDate.Month, localDate.Day, localTime.Hour, localTime.Minute, localTime.Second);
                    DateTime utcTime = TimezoneUtility.ConvertLocalTimeToUTC(timezoneResourceValue, localDateTime);

                    lblUtcTimeResult.Content = $"UTC Time: {utcTime}";
                }
                else
                {
                    MessageBox.Show("Invalid date/time format or no time zone selected.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void btnConvertToLocal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidateDate(datePickerUTC, out DateTime utcDate) &&
                    ValidateTime(txtUtcTime.Text, out DateTime utcTime) &&
                    comboBoxTimeZones.SelectedItem != null)
                {
                    string selectedTimeZoneKey = comboBoxTimeZones.SelectedItem.ToString();

                    if (!timeZoneData.TryGetValue(selectedTimeZoneKey, out string timezoneResourceValue))
                    {
                        MessageBox.Show("Timezone resource value not found.");
                        return;
                    }

                    DateTime utcDateTime = new DateTime(utcDate.Year, utcDate.Month, utcDate.Day, utcTime.Hour, utcTime.Minute, utcTime.Second);
                    DateTime localTime = TimezoneUtility.ConvertUTCToLocalTime(timezoneResourceValue, utcDateTime);

                    lblLocalTimeResult.Content = $"Local Time: {localTime}";
                }
                else
                {
                    MessageBox.Show("Invalid UTC date/time format or no time zone selected.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private bool ValidateDate(DatePicker datePicker, out DateTime validDate)
        {
            validDate = default;
            try
            {
                validDate = datePicker.SelectedDate ?? DateTime.Now; // Use SelectedDate or fallback to Now
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool ValidateTime(string timeInput, out DateTime validTime)
        {
            validTime = default;

            string[] validFormats = { "h:mm tt", "h:mm:ss tt", "HH:mm", "HH:mm:ss", "h:mm", "h:mm:ss" };
            foreach (string format in validFormats)
            {
                if (DateTime.TryParseExact(timeInput, format, null, System.Globalization.DateTimeStyles.None, out validTime))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
