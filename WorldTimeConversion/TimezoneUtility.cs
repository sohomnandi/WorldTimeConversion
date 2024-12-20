using System;
using System.Reflection;
using System.Resources;
using System.Globalization;
using System.Collections;


namespace WorldTimeConversion
{
    /// <summary>
    /// The UTC time converted to desired time zone with considering the Day light saving feature. The Day saving rule define in the resource along with the 
    /// UTC time offset, those varies according to the countrywise.
    /// </summary>
    public static class TimezoneUtility
    {
        #region Public Methods

        /// <summary>
        /// The main invoking method to convert a UTC date to desired local time zone
        /// </summary>
        /// <param name="timezoneResourceValue">The value part of the Time zone from Resource file, contain UTC offsets and time zone start and end day of the year</param>
        /// <param name="utcDateTime">The actual time that need to convert</param>
        /// <returns>Final output of the converted time</returns>
        public static DateTime ConvertUTCToLocalTime(string timezoneResourceValue, DateTime utcDateTime)
        {
            int startYearVal = utcDateTime.Year;
            int endYearVal = utcDateTime.Year;

            string[] resxValArr = timezoneResourceValue.Split(';');

            string[] utcOffsetArr = resxValArr[0].Split(',');
            string[] dstRangeArr = resxValArr[1].Split(',');

            string activeOffset;
            if (dstRangeArr[0].Length == 0 || dstRangeArr[1].Length == 0)
            {
                activeOffset = utcOffsetArr[0];
            }
            else
            {
                DateTime dstStartDate;
                DateTime dstEndDate;
                bool ret1 = InterpretDSTStartEndDateTime(startYearVal, endYearVal, dstRangeArr[0].Trim(), dstRangeArr[1].Trim(), out dstStartDate, out dstEndDate);
                activeOffset = DetermineUTCOffset(utcDateTime, resxValArr[0], dstStartDate, dstEndDate);
            }

            double varHrs = 0;
            double varMins = 0;
            DateTime outputDate;

            string[] activeOffsetArr = activeOffset.Split(':');
            bool res1 = double.TryParse(activeOffsetArr[0], out varHrs);
            bool res2 = double.TryParse(activeOffsetArr[1], out varMins);

            outputDate = utcDateTime.AddHours(varHrs);
            outputDate = outputDate.AddMinutes(varMins);

            return outputDate;

        }

        /// <summary>
        /// The main invoking method to convert a local time date to UTC time zone
        /// </summary>
        /// <param name="LocalTimezoneResourceValue"></param>
        /// <param name="LocalDateTime"></param>
        /// <returns></returns>
        public static DateTime ConvertLocalTimeToUTC(string LocalTimezoneResourceValue, DateTime LocalDateTime)
        {
            int startYearVal = LocalDateTime.Year;
            int endYearVal = LocalDateTime.Year;

            string[] resxValArr = LocalTimezoneResourceValue.Split(';');

            string[] utcOffsetArr = resxValArr[0].Split(',');
            string[] dstRangeArr = resxValArr[1].Split(',');

            string activeOffset;
            if (dstRangeArr[0].Length == 0 || dstRangeArr[1].Length == 0)
            {
                activeOffset = utcOffsetArr[0];
            }
            else
            {
                DateTime dstStartDate;
                DateTime dstEndDate;
                bool ret1 = InterpretDSTStartEndDateTime(startYearVal, endYearVal, dstRangeArr[0].Trim(), dstRangeArr[1].Trim(), out dstStartDate, out dstEndDate);
                activeOffset = DetermineUTCOffset(LocalDateTime, resxValArr[0], dstStartDate, dstEndDate);
            }

            if (activeOffset.StartsWith("+"))
                activeOffset = activeOffset.Replace("+", "-");
            else
                activeOffset = activeOffset.Replace("-", "+");

            double varHrs = 0;
            double varMins = 0;
            DateTime outputDate;

            string[] activeOffsetArr = activeOffset.Split(':');
            bool res1 = double.TryParse(activeOffsetArr[0], out varHrs);
            bool res2 = double.TryParse(activeOffsetArr[1], out varMins);

            outputDate = LocalDateTime.AddHours(varHrs);
            outputDate = outputDate.AddMinutes(varMins);

            return outputDate;

        }

        /// <summary>
        /// Returns the UTC sign from the local timezone
        /// </summary>
        /// <param name="timezoneResourceValue">The value part of the Time zone from Resource file, contain UTC offsets and time zone start and end day of the year</param>
        /// <returns></returns>
        public static string GetUTCSignOfLocalTimezone(string timezoneResourceValue)
        {
            string sign = timezoneResourceValue.Substring(0, 1);
            return sign;
        }

        //public static string[] GetAllTimezones()
        //{
        //    string[] retStrKeyarr = null;
        //    int idx = 0;
        //    ResourceSet rs = TZDictionary.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
                
        //    foreach(DictionaryEntry entry in rs)
        //    {
        //        retStrKeyarr[idx] = entry.Key.ToString();
        //        idx++;
        //    }
           
        //    return retStrKeyarr;
        //}

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// The value pair of all months of the year
        /// </summary>
        /// <param name="key">Three letter abbreviation of the month</param>
        /// <returns>The integer value of the month</returns>
        private static int GetMonthHashValue(string key)
        {
            System.Collections.Hashtable hashMonth = new System.Collections.Hashtable();
            hashMonth.Add("Jan", 1);
            hashMonth.Add("Feb", 2);
            hashMonth.Add("Mar", 3);
            hashMonth.Add("Apr", 4);
            hashMonth.Add("May", 5);
            hashMonth.Add("Jun", 6);
            hashMonth.Add("Jul", 7);
            hashMonth.Add("Aug", 8);
            hashMonth.Add("Sep", 9);
            hashMonth.Add("Oct", 10);
            hashMonth.Add("Nov", 11);
            hashMonth.Add("Dec", 12);

            return (int)hashMonth[key];
        }

        /// <summary>
        /// The value pair of the index of week
        /// </summary>
        /// <param name="key">English text of the week number</param>
        /// <returns>The integer part of the week number</returns>
        private static int GetIndexHashValue(string key)
        {
            System.Collections.Hashtable hashIndex = new System.Collections.Hashtable();
            hashIndex.Add("First", 1);
            hashIndex.Add("Second", 2);
            hashIndex.Add("Third", 3);
            hashIndex.Add("Forth", 4);
            hashIndex.Add("Fifth", 5);
            hashIndex.Add("Last", 5);

            return (int)hashIndex[key];
        }

        /// <summary>
        /// The value pair of the week days
        /// </summary>
        /// <param name="key">Three letter abbreviation of the week days</param>
        /// <returns>The integer value of the week days</returns>
        private static DayOfWeek GetDayHashValue(string key)
        {
            System.Collections.Hashtable hashMonth = new System.Collections.Hashtable();
            hashMonth.Add("Sun", DayOfWeek.Sunday);
            hashMonth.Add("Mon", DayOfWeek.Monday);
            hashMonth.Add("Tue", DayOfWeek.Tuesday);
            hashMonth.Add("Wed", DayOfWeek.Wednesday);
            hashMonth.Add("Thu", DayOfWeek.Thursday);
            hashMonth.Add("Fri", DayOfWeek.Friday);
            hashMonth.Add("Sat", DayOfWeek.Saturday);

            return (DayOfWeek)hashMonth[key];
        }

        /// <summary>
        /// To find the DST start date and end date from the DST rule string
        /// </summary>
        /// <param name="startYearVal">The start year value - to determine the DST start date</param>
        /// <param name="endYearVal">The end year value - to determine the DST end date</param>
        /// <param name="dstStart">DST rule string in resource</param>
        /// <param name="dstEnd">DST rule string in resource</param>
        /// <param name="dstStartDate">Returned the DST start date</param>
        /// <param name="dstEndDate">Returned the DST end date</param>
        /// <returns></returns>
        private static bool InterpretDSTStartEndDateTime(int startYearVal, int endYearVal, string dstStart, string dstEnd, out DateTime dstStartDate, out DateTime dstEndDate)
        {
            string sIndex, sMonth, sDay;
            string eIndex, eMonth, eDay;
            bool startRet = UnWrapDSTString(dstStart, out sIndex, out sMonth, out sDay);
            bool endRet = UnWrapDSTString(dstEnd, out eIndex, out eMonth, out eDay);

            DateTime dstTargetStartMonth = new DateTime(startYearVal, GetMonthHashValue(sMonth), 1);
            DateTime dstTargetEndMonth = new DateTime(endYearVal, GetMonthHashValue(eMonth), 1);

            DateTime dstStartDateval;
            bool ret1 = dstTargetStartMonth.TryGetDayOfMonth(GetDayHashValue(sDay), GetIndexHashValue(sIndex), out dstStartDateval);

            DateTime dstEndDateval;
            bool ret2 = dstTargetEndMonth.TryGetDayOfMonth(GetDayHashValue(eDay), GetIndexHashValue(eIndex), out dstEndDateval);

            dstStartDate = dstStartDateval.AddDays(1);
            dstEndDate = dstEndDateval;
            return true;
        }

        /// <summary>
        /// To find out the actual UTC offset value of the UTC date
        /// </summary>
        /// <param name="mainDate">The UTC date</param>
        /// <param name="offsetString">The whole offset string in the resource</param>
        /// <param name="dstStartDate">The actual DST Start Date of the region</param>
        /// <param name="dstEndDate">The actual DST End Date of the region</param>
        /// <returns>The actual offset value</returns>
        private static string DetermineUTCOffset(DateTime mainDate, string offsetString, DateTime dstStartDate, DateTime dstEndDate)
        {
            string[] dstoffsetArr = offsetString.Split(',');
            string retunOffset;
            if (mainDate >= dstStartDate && mainDate <= dstEndDate)
            {
                retunOffset = dstoffsetArr[1];
            }
            else
            {
                retunOffset = dstoffsetArr[0];
            }
            return retunOffset;
        }

        /// <summary>
        /// The unwrap DST starting day and ending day string
        /// </summary>
        /// <param name="dstString">Actual DST string</param>
        /// <param name="index">Index value of the week</param>
        /// <param name="monthVal">The month value</param>
        /// <param name="dayVal">The day value</param>
        /// <returns></returns>
        private static bool UnWrapDSTString(string dstString, out string index, out string monthVal, out string dayVal)
        {
            string mVal = dstString.Substring(0, 3);
            string dVal = dstString.Substring(dstString.Length - 3);
            string iVal = dstString.Substring(3, (dstString.Length - 6));
            index = iVal;
            monthVal = mVal;
            dayVal = dVal;
            return true;
        }

        /// <summary>
        /// Get the actual date of particular day of a specific week
        /// </summary>
        /// <param name="instance">Starting date of the month</param>
        /// <param name="dayOfWeek">Week day need to consider</param>
        /// <param name="occurance">The week number</param>
        /// <param name="dateOfMonth">The calculated date returned</param>
        /// <returns>Whether the function executed successfully</returns>
        private static bool TryGetDayOfMonth(this DateTime instance, DayOfWeek dayOfWeek, int occurance, out DateTime dateOfMonth)
        {
            bool result;
            dateOfMonth = new DateTime();

            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            if (occurance <= 0 || occurance > 5)
            {
                result = false;
            }

            // Change to first day of the month
            DateTime dayOfMonth = instance.AddDays(1 - instance.Day);

            // Find first dayOfWeek of this month;
            if (dayOfMonth.DayOfWeek > dayOfWeek)
            {
                dayOfMonth = dayOfMonth.AddDays(7 - (int)dayOfMonth.DayOfWeek + (int)dayOfWeek);
            }
            else
            {
                dayOfMonth = dayOfMonth.AddDays((int)dayOfWeek - (int)dayOfMonth.DayOfWeek);
            }

            // add 7 days per occurance
            dayOfMonth = dayOfMonth.AddDays(7 * (occurance - 1));

            // make sure this occurance is within the original month
            result = dayOfMonth.Month == instance.Month;

            if (result)
            {
                dateOfMonth = dayOfMonth;
            }

            return result;
        }

        #endregion

    }
}
