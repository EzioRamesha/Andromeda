using System;
using System.Linq;

namespace Shared
{
    public class QuarterObject
    {
        public string Value { get; set; }
        public string Quarter { get; set; }
        public int MonthStart { get; set; }
        public int MonthEnd { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public DateTime? EndDate { get; set; }
        public string Error { get; set; }
        public bool Valid { get; set; } = false;

        public QuarterObject() { }

        public QuarterObject(string value)
        {
            Value = value;
            ParseEndDate();
        }

        public QuarterObject(int month, int year)
        {
            Month = month;
            Year = year;
            ParseEndDateByMonthYear();
        }

        public void ParseEndDate()
        {
            Quarter = null;
            MonthStart = 0;
            MonthEnd = 0;
            Year = 0;
            EndDate = null;
            Error = null;

            if (string.IsNullOrEmpty(Value))
            {
                SetError("Value is null or empty");
                return;
            }

            string[] values = Value.Split(' ').Select(v => v.Trim()).ToArray();

            if (values.IsNullOrEmpty())
            {
                SetError("The value can not be split into string[]");
                return;
            }
            else if (values.Length != 2)
            {
                SetError("The value split into string[] and the length is not equal 2");
                return;
            }
            else
            {
                try
                {
                    if (values[0].Contains("Q"))
                    {
                        Quarter = values[0];
                        Year = int.Parse(values[1]);
                    }
                    else if (values[1].Contains("Q"))
                    {
                        Quarter = values[1];
                        Year = int.Parse(values[0]);
                    }
                    else
                    {
                        SetError("The value not contain \"Q\" wording");
                        return;
                    }
                }
                catch (Exception e)
                {
                    SetError(e.Message);
                    return;
                }
            }

            try
            {
                switch (Quarter)
                {
                    case "Q1":
                        MonthStart = 1;
                        MonthEnd = 3;
                        EndDate = DateTime.Parse("31 March " + Year);
                        break;
                    case "Q2":
                        MonthStart = 4;
                        MonthEnd = 6;
                        EndDate = DateTime.Parse("30 June " + Year);
                        break;
                    case "Q3":
                        MonthStart = 7;
                        MonthEnd = 9;
                        EndDate = DateTime.Parse("30 September " + Year);
                        break;
                    case "Q4":
                        MonthStart = 10;
                        MonthEnd = 12;
                        EndDate = DateTime.Parse("31 December " + Year);
                        break;
                    default:
                        SetError("Invalid quarter: " + Quarter);
                        break;
                }
            }
            catch (Exception e)
            {
                SetError(e.Message);
                return;
            }

            // Finally format the quater string
            Quarter = Year + " " + Quarter;
        }

        public void ParseEndDateByMonthYear(int? month = null, int? year = null)
        {
            if (month.HasValue)
                Month = month.Value;
            if (year.HasValue)
                Year = year.Value;

            Value = null;
            if (Month >= 1 && Month <= 3)
                Value = Year + " Q1";
            if (Month >= 4 && Month <= 6)
                Value = Year + " Q2";
            if (Month >= 7 && Month <= 9)
                Value = Year + " Q3";
            if (Month >= 10 && Month <= 12)
                Value = Year + " Q4";
            ParseEndDate();
        }

        public bool IsValid()
        {
            Valid = string.IsNullOrEmpty(Error);
            return Valid;
        }

        public void SetError(string msg)
        {
            Error = msg;
            IsValid();
        }
    }
}
