using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ttTVAdmin
{
    public static class DateTimeExtensions
    {
        public static string ToTimespanString(this DateTime dt)
        {
            string display = dt.ToString();

            TimeSpan ts = DateTime.Now.Subtract(dt);
            if (ts.TotalMilliseconds > 0) // Past
            {
                if (ts.TotalDays > 365)
                {
                    int years = Convert.ToInt32(Math.Floor(ts.TotalDays / 365));
                    display = string.Format("{0} years ago", years);
                }
                else if (ts.TotalDays > 30)
                {
                    int months = Convert.ToInt32(Math.Floor(ts.TotalDays / 30));
                    display = string.Format("{0} months ago", months);
                }
                else if (ts.TotalHours > 24)
                {
                    int days = Convert.ToInt32(Math.Floor(ts.TotalHours / 24));
                    display = string.Format("{0} days ago", days);
                }
                else if (ts.TotalMinutes > 60)
                {
                    int mins = Convert.ToInt32(Math.Floor(ts.TotalMinutes / 60));
                    display = string.Format("{0} hours ago", mins);
                }
                else if (ts.TotalSeconds > 60)
                {
                    int seconds = Convert.ToInt32(Math.Floor(ts.TotalSeconds / 60));
                    display = string.Format("{0} minutes ago", seconds);
                }
                else
                    display = string.Format("{0} seconds ago", Convert.ToInt32(Math.Floor(ts.TotalSeconds)));
            }
            else // Future
            {
                if (ts.TotalDays > -365)
                {
                    int years = Convert.ToInt32(Math.Floor(ts.TotalDays / 365));
                    display = string.Format("{0} years later", years);
                }
                else if (ts.TotalDays > -30)
                {
                    int months = Convert.ToInt32(Math.Floor(ts.TotalDays / 30));
                    display = string.Format("{0} months later", months);
                }
                else if (ts.TotalHours > -24)
                {
                    int days = Convert.ToInt32(Math.Floor(ts.TotalHours / 24));
                    display = string.Format("{0} days later", days);
                }
                else if (ts.TotalMinutes > -60)
                {
                    int mins = Convert.ToInt32(Math.Floor(ts.TotalMinutes / 60));
                    display = string.Format("{0} hours later", mins);
                }
                else if (ts.TotalSeconds > -60)
                {
                    int seconds = Convert.ToInt32(Math.Floor(ts.TotalSeconds / 60));
                    display = string.Format("{0} minutes later", seconds);
                }
                else
                    display = string.Format("{0} seconds later", Convert.ToInt32(Math.Floor(ts.TotalSeconds)));
            }

            return display;
        }

        public static string ToShortTimespanString(this DateTime dt)
        {
            string display = dt.ToString();

            TimeSpan ts = DateTime.Now.Subtract(dt);
            if (ts.TotalMilliseconds > 0) // Past
            {
                if (ts.TotalDays > 365)
                {
                    int years = Convert.ToInt32(Math.Floor(ts.TotalDays / 365));
                    display = string.Format("{0} yr ago", years);
                }
                else if (ts.TotalDays > 30)
                {
                    int months = Convert.ToInt32(Math.Floor(ts.TotalDays / 30));
                    display = string.Format("{0} mth ago", months);
                }
                else if (ts.TotalHours > 24)
                {
                    int days = Convert.ToInt32(Math.Floor(ts.TotalHours / 24));
                    display = string.Format("{0} day ago", days);
                }
                else if (ts.TotalMinutes > 60)
                {
                    int mins = Convert.ToInt32(Math.Floor(ts.TotalMinutes / 60));
                    display = string.Format("{0} hr ago", mins);
                }
                else if (ts.TotalSeconds > 60)
                {
                    int seconds = Convert.ToInt32(Math.Floor(ts.TotalSeconds / 60));
                    display = string.Format("{0} min ago", seconds);
                }
                else
                    display = string.Format("{0} sec ago", Convert.ToInt32(Math.Floor(ts.TotalSeconds)));
            }
            else // Future
            {
                if (ts.TotalDays > -365)
                {
                    int years = Convert.ToInt32(Math.Floor(ts.TotalDays / 365));
                    display = string.Format("{0} yr later", years);
                }
                else if (ts.TotalDays > -30)
                {
                    int months = Convert.ToInt32(Math.Floor(ts.TotalDays / 30));
                    display = string.Format("{0} mth later", months);
                }
                else if (ts.TotalHours > -24)
                {
                    int days = Convert.ToInt32(Math.Floor(ts.TotalHours / 24));
                    display = string.Format("{0} day later", days);
                }
                else if (ts.TotalMinutes > -60)
                {
                    int mins = Convert.ToInt32(Math.Floor(ts.TotalMinutes / 60));
                    display = string.Format("{0} hr later", mins);
                }
                else if (ts.TotalSeconds > -60)
                {
                    int seconds = Convert.ToInt32(Math.Floor(ts.TotalSeconds / 60));
                    display = string.Format("{0} min later", seconds);
                }
                else
                    display = string.Format("{0} sec later", Convert.ToInt32(Math.Floor(ts.TotalSeconds)));
            }

            return display;
        }

        public static string ToDisplayString(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd hh:mm");
        }
    }
}