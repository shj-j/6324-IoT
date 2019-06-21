using System;
using System.Globalization;

namespace IOTStadiumWeb.Models
{
    /// <summary>
    /// This class contains utility methods that are used throughout the application
    /// </summary>
    public class Utility
    {
        /// <summary>
        /// This function determines where the object passed to it is a valid data
        /// </summary>
        /// <param name="obj"></param> The object that is tested
        /// <returns></returns> True if the object is a valid date; False if the object is not a valid date
        public bool IsDate(string obj)
        {
            CultureInfo myCI = new CultureInfo("en-AU");
            bool isDate;
            DateTime retDate = new DateTime();
            if (obj != null)
                isDate = DateTime.TryParse(obj, myCI, DateTimeStyles.None, out retDate);
            else
                isDate = false;

            return isDate;
        }

        /// <summary>
        /// Tests whether the object that is passed to this function is numeric     
        /// </summary>
        /// <param name="obj"></param> The object that is tested
        /// <returns></returns> True is the object is a number; False if the object is not a number
        public bool IsNumeric(object obj)
        {
            // Variable to collect the Return value of the TryParse method.
            bool isNum;

            // Define variable to collect out parameter of the TryParse method. If the conversion fails, the out parameter is zero.
            double retNum;

            // The TryParse method converts a string in a specified style and culture-specific format to its double-precision floating point number equivalent.
            // The TryParse method does not generate an exception if the conversion fails. If the conversion passes, True is returned. If it does not, False is returned.
            isNum = Double.TryParse(Convert.ToString(obj), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }

    }
}
