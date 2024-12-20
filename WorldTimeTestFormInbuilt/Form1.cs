using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WorldTimeTestFormInbuilt; // Updated namespace
using WorldTimeConversion;     // Utility methods

namespace WorldTimeTestFormInbuilt
{
    public partial class Form1 : Form
    {
        private DateTimePicker datePickerLocal;
        private DateTimePicker datePickerUTC;

        public Form1()
        {
            InitializeComponent();
            PopulateTimezonesIntoComboBox();

            // Initialize DateTimePickers
            datePickerLocal = new DateTimePicker
            {
                Format = DateTimePickerFormat.Short,
                Location = new Point(232, 219),
                Size = new Size(400, 39)
            };
            this.Controls.Add(datePickerLocal);

            datePickerUTC = new DateTimePicker
            {
                Format = DateTimePickerFormat.Short,
                Location = new Point(232, 417),
                Size = new Size(400, 39)
            };
            this.Controls.Add(datePickerUTC);

            // Set default label text to empty
            lblUtcTimeResult.Text = string.Empty;
            lblLocalTimeResult.Text = string.Empty;
        }

        private List<string> GetTimezoneKeys()
        {
            var resourceSet = WorldTimeConversion.TZDictionary.ResourceManager.GetResourceSet(
                System.Globalization.CultureInfo.InvariantCulture, true, true);

            if (resourceSet == null)
            {
                MessageBox.Show("Resource set is null.");
                return null;
            }

            return resourceSet.Cast<System.Collections.DictionaryEntry>()
                              .Select(entry => entry.Key.ToString())
                              .OrderBy(key => key)
                              .ToList();
        }

        private void PopulateTimezonesIntoComboBox()
        {
            var timezoneKeys = GetTimezoneKeys();
            if (timezoneKeys == null || timezoneKeys.Count == 0)
            {
                MessageBox.Show("No timezone keys found or resource set is null.");
                return;
            }

            comboBoxTimeZones.Items.Clear();
            foreach (var key in timezoneKeys)
            {
                comboBoxTimeZones.Items.Add(key);
            }

            if (comboBoxTimeZones.Items.Count > 0)
                comboBoxTimeZones.SelectedIndex = 0;
        }

        private bool ValidateTime(string timeInput, out DateTime validTime)
        {
            validTime = default;

            string[] validFormats = {
                "h:mm tt", "h:mm:ss tt", "HH:mm", "HH:mm:ss", "h:mm", "h:mm:ss"
            };

            foreach (string format in validFormats)
            {
                if (DateTime.TryParseExact(timeInput, format, null, System.Globalization.DateTimeStyles.None, out validTime))
                {
                    return true;
                }
            }
            return false;
        }

        private bool ValidateDate(DateTimePicker datePicker, out DateTime validDate)
        {
            validDate = default;
            string dateInput = datePicker.Value.ToString("MM/dd/yyyy");
            if (DateTime.TryParseExact(dateInput, "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out validDate))
            {
                return true;
            }
            return false;
        }

        private void btnConvertToUTC_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateDate(datePickerLocal, out DateTime localDate) &&
                    ValidateTime(txtLocalTime.Text, out DateTime localTime) &&
                    comboBoxTimeZones.SelectedItem != null)
                {
                    string selectedTimeZoneKey = comboBoxTimeZones.SelectedItem.ToString();
                    string timezoneResourceValue = WorldTimeConversion.TZDictionary.ResourceManager.GetString(selectedTimeZoneKey);

                    if (string.IsNullOrEmpty(timezoneResourceValue))
                    {
                        MessageBox.Show("Timezone resource value not found.");
                        return;
                    }

                    DateTime localDateTime = new DateTime(localDate.Year, localDate.Month, localDate.Day,
                                                          localTime.Hour, localTime.Minute, localTime.Second);

                    // Split resource string into offset and DST rules
                    var parts = timezoneResourceValue.Split(';');
                    if (parts.Length < 2)
                    {
                        MessageBox.Show($"Invalid timezone resource format for: {selectedTimeZoneKey}");
                        return;
                    }

                    string[] offsets = parts[0].Split(',');
                    string dstRules = parts.Length > 1 ? parts[1] : string.Empty;

                    // Validate and parse offsets
                    if (offsets.Length < 2 ||
                        TimeSpan.TryParse(offsets[0], out TimeSpan standardOffset) ||
                        TimeSpan.TryParse(offsets[1], out TimeSpan dstOffset))
                    {
                        MessageBox.Show($"Invalid offset format in timezone resource: {timezoneResourceValue}");
                        return;
                    }

                    // Check if DST is active
                    bool isDstActive = !string.IsNullOrEmpty(dstRules) && IsDstActive(localDateTime, dstRules);

                    // Subtract DST offset if active
                    TimeSpan effectiveOffset = isDstActive ? dstOffset : standardOffset;
                    DateTime utcTime = localDateTime - effectiveOffset;

                    lblUtcTimeResult.Text = $"UTC Time: {utcTime} {(isDstActive ? "(DST Active)" : "(No DST)")}";
                }
                else
                {
                    MessageBox.Show("Invalid local date/time format or no time zone selected.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }


        private void btnConvertToLocal_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateDate(datePickerUTC, out DateTime utcDate) &&
                    ValidateTime(txtUtcTime.Text, out DateTime utcTime) &&
                    comboBoxTimeZones.SelectedItem != null)
                {
                    string selectedTimeZoneKey = comboBoxTimeZones.SelectedItem.ToString();
                    string timezoneResourceValue = WorldTimeConversion.TZDictionary.ResourceManager.GetString(selectedTimeZoneKey);

                    if (string.IsNullOrEmpty(timezoneResourceValue))
                    {
                        MessageBox.Show("Timezone resource value not found.");
                        return;
                    }

                    DateTime utcDateTime = new DateTime(utcDate.Year, utcDate.Month, utcDate.Day,
                                                        utcTime.Hour, utcTime.Minute, utcTime.Second);

                    // Split resource string into offset and DST rules
                    var parts = timezoneResourceValue.Split(';');
                    if (parts.Length < 2)
                    {
                        MessageBox.Show($"Invalid timezone resource format for: {selectedTimeZoneKey}");
                        return;
                    }

                    string[] offsets = parts[0].Split(',');
                    string dstRules = parts.Length > 1 ? parts[1] : string.Empty;

                    // Validate and parse offsets
                    if (offsets.Length < 2 ||
                        TimeSpan.TryParse(offsets[0], out TimeSpan standardOffset) ||
                        TimeSpan.TryParse(offsets[1], out TimeSpan dstOffset))
                    {
                        MessageBox.Show($"Invalid offset format in timezone resource: {timezoneResourceValue}");
                        return;
                    }

                    // Add the appropriate offset
                    TimeSpan effectiveOffset = standardOffset;
                    DateTime localTime = utcDateTime + effectiveOffset;

                    // Add DST offset if active
                    bool isDstActive = !string.IsNullOrEmpty(dstRules) && IsDstActive(localTime, dstRules);
                    if (isDstActive)
                    {
                        localTime = localTime.AddHours(1);
                    }

                    lblLocalTimeResult.Text = $"Local Time: {localTime} {(isDstActive ? "(DST Active)" : "(No DST)")}";
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



        private bool IsDstActive(DateTime localTime, string dstRules)
        {
            if (string.IsNullOrWhiteSpace(dstRules))
            {
                return false; // No DST rules mean DST is not active.
            }

            var rules = dstRules.Split(',');
            foreach (string rule in rules)
            {
                if (rule.Contains("MarFourthSat") && localTime.Month == 3 && localTime.DayOfWeek == DayOfWeek.Saturday && localTime.Day >= 22)
                {
                    return true;
                }
                if (rule.Contains("JunFirstSun") && localTime.Month == 6 && localTime.DayOfWeek == DayOfWeek.Sunday && localTime.Day <= 7)
                {
                    return true;
                }
                // Add additional rules as needed.
            }
            return false;
        }


    }
}
