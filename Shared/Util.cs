using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Shared
{
    public static class Util
    {
        public static string DateTimeFormat = "dd MMM yyyy HH:mm:ss";
        public static string Null = "<null>";

        public static T Clone<T>(this T source)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source));
        }

        public static bool IsNullOrEmpty(this IEnumerable<object> o)
        {
            if (o == null)
                return true;
            if (o.Count() == 0)
                return true;
            return false;
        }

        public static string PadBoth(this string s, int width, char c = ' ')
        {
            int spaces = width - s.Length;
            int padLeft = spaces / 2 + s.Length;
            int padRight = width;
            return s.PadLeft(padLeft, c).PadRight(padRight, c);
        }

        public static string RemoveNewLines(this string s)
        {
            s = s.Replace("\r", "");
            s = s.Replace("\n", "");
            return s;
        }

        public static int CountOccurence(this string s, string pattern)
        {
            int count = 0;
            if (!s.Contains(pattern))
                return count;

            int index = 0;
            while ((index = s.IndexOf(pattern, index)) != -1)
            {
                index += pattern.Length;
                count++;
            }

            return count;
        }

        public static string[] ToArguments(this string commandLine)
        {
            commandLine = commandLine.Trim();

            List<char> newChars = new List<char>() { };
            char[] chars = commandLine.ToCharArray();
            bool inQuote = false;
            bool inHyphen = false;

            int index;
            for (index = 0; index < chars.Length; index++)
            {
                char currentChar = chars[index];
                char previousChar = (index - 1) >= 0 ? chars[index - 1] : '\n';

                switch (currentChar)
                {
                    case ' ':
                        if (previousChar == ' ')
                            continue;
                        if (!inQuote)
                            currentChar = '\n';
                        if (inHyphen)
                            inHyphen = false;
                        break;

                    case '"':
                        inQuote = !inQuote;
                        break;

                    case '-':
                        inHyphen = true;
                        break;
                }

                newChars.Add(currentChar);
            }
            return (new string(newChars.ToArray())).Split('\n');
        }

        public static string[] ToColumns(this string line, char delimiter = ',')
        {
            Regex regx = new Regex(delimiter + "(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
            if (!regx.IsMatch(line))
                line = line.TrimStart('\"').TrimEnd('\"');

            line = line.Trim();

            List<char> newChars = new List<char>() { };
            char[] chars = line.ToCharArray();
            bool inQuote = false;

            int index;
            for (index = 0; index < chars.Length; index++)
            {
                char currentChar = chars[index];

                if (currentChar == delimiter)
                {
                    if (!inQuote)
                        currentChar = '\n';
                }
                else if (currentChar == '"')
                {
                    inQuote = !inQuote;
                    continue;
                }

                newChars.Add(currentChar);
            }
            return (new string(newChars.ToArray())).Split('\n');
        }

        public static string ToPascalCase(this string s)
        {
            // If there are 0 or 1 characters, just return the string.
            if (s == null) return s;
            if (s.Length < 2) return s.ToUpper();

            // Split the string into words.
            string[] words = s.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);

            // Combine the words.
            string result = "";
            foreach (string word in words)
            {
                result += word.Substring(0, 1).ToUpper() + word.Substring(1);
            }

            return result;
        }

        public static string ToCamelCase(this string s)
        {
            // If there are 0 or 1 characters, just return the string.
            if (s == null || s.Length < 2)
                return s;

            // Split the string into words.
            string[] words = s.Split(
                new char[] { },
                StringSplitOptions.RemoveEmptyEntries);

            // Combine the words.
            string result = words[0].ToLower();
            for (int i = 1; i < words.Length; i++)
            {
                result +=
                    words[i].Substring(0, 1).ToUpper() +
                    words[i].Substring(1);
            }

            return result;
        }

        public static string ToProperCase(this string s, bool spaceNumber = false)
        {
            // If there are 0 or 1 characters, just return the string.
            if (s == null) return s;
            if (s.Length < 2) return s.ToUpper();

            // Start with the first character.
            string result = s.Substring(0, 1).ToUpper();

            // Add the remaining characters.
            for (int i = 1; i < s.Length; i++)
            {
                if (char.IsUpper(s[i])) result += " ";
                else if (spaceNumber && char.IsNumber(s[i])) result += " ";
                result += s[i];
            }

            return result;
        }

        public static string[] ToArraySplitTrim(this string s, char delimter = ',', bool emptyString = true)
        {
            if (s == null && emptyString)
                return new string[] { "" };
            if (s == null)
                return new string[] { };
            return s.Split(delimter).Select(q => q.Trim()).ToArray();
        }

        public static string AppendDateFileName(this string s, string ext)
        {
            return string.Format("{0}_{1}{2}", s, DateTime.Now.ToString("yyyyMMdd"), ext);
        }

        public static string AppendDateTimeFileName(this string s, string ext)
        {
            return string.Format("{0}_{1}{2}", s, DateTime.Now.ToString("yyyyMMdd_HHmmss"), ext);
        }

        public static bool IsValidEmail(this string email)
        {
            try
            {
                var mailAddress = new MailAddress(email);
                return mailAddress.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static string GenerateRandomString(int minLength, int maxLength, bool alphanumericCharactersOnly = false)
        {
            // Make sure that input parameters are valid.
            if (minLength <= 0 || maxLength <= 0 || minLength > maxLength)
                return null;

            var lowerCaseCharacters = "abcdefgijkmnpqrstwxyz";
            var upperCaseCharacters = "ABCDEFGHJKLMNPQRSTWXYZ";
            var numbers = "123456789";
            var specialCharacters = "*$-+?_&=!%{}/";

            string characterSet = lowerCaseCharacters + upperCaseCharacters + numbers;
            int characterSetLength = characterSet.Length - 1;
            int specialCharacterLength = specialCharacters.Length - 1;

            // Use a 4-byte array to fill it with random bytes and convert it then
            // to an integer value.
            byte[] randomBytes = new byte[4];

            // Generate 4 random bytes.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);

            // Convert 4 bytes into a 32-bit integer value.
            int seed = BitConverter.ToInt32(randomBytes, 0);

            // Now, this is real randomization.
            Random random = new Random(seed);

            // This array will hold password characters.
            char[] password = null;

            // Allocate appropriate memory for the password.
            int passwordLength = minLength;
            if (minLength < maxLength)
                passwordLength = random.Next(minLength, maxLength + 1);

            password = new char[passwordLength];

            int scCount = alphanumericCharactersOnly ? 0 : ((passwordLength % 2 == 0) ? 2 : 3);

            int[] scPositions = new int[scCount];
            for (int pos = 0; pos < scCount; pos++)
            {
                int nextPos;
                do
                {
                    nextPos = random.Next(passwordLength);
                } while (scPositions.Contains(nextPos));

                scPositions[pos] = nextPos;
            }

            // Generate password characters one at a time.
            for (int position = 0; position < password.Length; position++)
            {
                char Last1Char = ' ';
                char Last2Char = ' ';
                if (position > 1)
                {
                    Last1Char = password[position - 1];
                    Last2Char = password[position - 2];
                }

                char nextChar;
                do
                {
                    if (scPositions.Contains(position))
                    {
                        nextChar = specialCharacters[random.Next(specialCharacterLength)];
                    }
                    else
                    {
                        nextChar = characterSet[random.Next(characterSetLength)];
                    }
                } while (nextChar == Last1Char || nextChar == Last2Char);

                // Add this character to the password.
                password[position] = nextChar;
            }

            // Convert password characters into a string and return the result.
            return new string(password);
        }

        public static StringBuilder CatchDbEntityValidationException(DbEntityValidationException e)
        {
            var output = new StringBuilder();
            output.AppendLine("DbEntityValidationException");
            int i = 0;
            foreach (var dbEntityValidationResult in e.EntityValidationErrors.ToList())
            {
                int j = 0;
                foreach (var dbValidationError in dbEntityValidationResult.ValidationErrors)
                {
                    output.AppendLine(string.Format("{0}> [{1}][{2}]: {3}", "".PadRight(4, ' '), i, j, dbValidationError.ErrorMessage));
                    j++;
                }
                i++;
            }
            return output;
        }

        public static string GetLineDelimiter(int width = 90, char delimiter = '-')
        {
            return "".PadLeft(width, delimiter);
        }

        public static bool GetSubStringParams(this string s, out int startIndex, out int lengthIndex)
        {
            startIndex = 0;
            lengthIndex = 0;

            if (string.IsNullOrEmpty(s)) return false;

            List<char> startChars = new List<char>() { };
            List<char> lengthChars = new List<char>() { };
            char[] chars = s.ToCharArray();

            bool comma = false;

            int index;
            for (index = 0; index < chars.Length; index++)
            {
                char currentChar = chars[index];

                if (Char.IsDigit(currentChar))
                {
                    if (comma)
                        lengthChars.Add(currentChar);
                    else
                        startChars.Add(currentChar);
                }
                else if (currentChar == ',')
                {
                    comma = true;
                }
            }

            string startIndexStr = new string(startChars.ToArray());
            string lengthIndexStr = new string(lengthChars.ToArray());

            bool startIndexTry = Int32.TryParse(startIndexStr, out startIndex);
            Int32.TryParse(lengthIndexStr, out lengthIndex);

            return startIndexTry;
        }

        public static bool IsProd()
        {
            if (GetEnviroment() == "production")
                return true;
            return false;
        }

        public static bool IsUat()
        {
            if (GetEnviroment() == "uat")
                return true;
            return false;
        }

        public static bool IsLocal()
        {
            if (GetEnviroment() == "local")
                return true;
            return false;
        }

        public static string GetEnviroment(string def = null)
        {
            return GetConfig("Environment", def);
        }

        public static string GetConfig(string name, string def = null)
        {
            string value = ConfigurationManager.AppSettings[name];
            if (string.IsNullOrEmpty(value))
                return def;
            return value;
        }

        public static bool GetConfigBoolean(string name, bool value = false)
        {
            var str = GetConfig(name);
            if (!string.IsNullOrEmpty(str) && bool.TryParse(str, out bool result))
            {
                return result;
            }
            return value;
        }

        public static int GetConfigInteger(string name, int value = 0)
        {
            var str = GetConfig(name);
            if (!string.IsNullOrEmpty(str) && int.TryParse(str, out int result))
            {
                return result;
            }
            return value;
        }

        public static string GetDateFormat()
        {
            return GetConfig("DateFormat");
        }

        public static string GetExportDateFormat()
        {
            return GetConfig("ExportDateFormat");
        }

        public static string GetDateTimeFormat()
        {
            return GetConfig("DateTimeFormat");
        }

        public static string GetDateTimeConsoleFormat()
        {
            return GetConfig("DateTimeConsoleFormat");
        }

        public static string GetDateFormatDatePickerJs()
        {
            return GetConfig("DateFormatDatePickerJs");
        }

        public static string GetDateFormatMomentJs()
        {
            return GetConfig("DateFormatMomentJs");
        }

        public static string GetDateTimeFormatMomentJs()
        {
            return GetConfig("DateTimeFormatMomentJs");
        }

        public static string GetDaysBeforeInactiveUserSuspension()
        {
            return GetConfig("DaysBeforeInactiveUserSuspension");
        }

        public static string GetDaysBeforeInactiveUserReport()
        {
            return GetConfig("DaysBeforeInactiveUserReport");
        }

        public static string GetDefaultEndDate()
        {
            return GetConfig("DefaultEndDate");
        }

        public static bool TryParseDateTime(string value, out DateTime? datetime, out string error)
        {
            datetime = null;
            error = null;

            List<string> formats = new List<string>
            {
                GetDateFormat(),
                GetDateTimeFormat(),
                GetExportDateFormat(),
                "d-MMM-yy",
                "d-MMM-yy H:m:s",
                "d-MMM-yy h:m:s tt",
                "d-MMM-yyyy",
                "d-MMM-yyyy H:m:s",
                "d-MMM-yyyy h:m:s tt",
                "dd MMM yyyy HH:mm",
                "d-M-yy",
                "d-MM-yy",
                "d-M-yyyy",
                "d-M-yy H:m",
                "d-M-yyyy H:m",
                "d/M/yy",
                "dd/M/yy",
                "d/M/yyyy",
                "d/M/yy H:m",
                "d/M/yyyy H:m",
                "M/d/yyyy",
                "M/d/yyyy h:m tt",
                "M/d/yyyy h:m:s tt",
                "M/d/yyyy hh:mm:ss tt",
                "d.M.yyyy",
                "d.M.yy",
                "yyyy-MM-d",
                "yyyy.MM.dd",
                "yyyy.M.d",
            };
            foreach (string format in formats)
            {
                if (ParseDateTime(value, format, out datetime, out error))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool ParseDateTime(string value, string format, out DateTime? datetime, out string error)
        {
            datetime = null;
            error = "";
            try
            {
                CultureInfo provider = CultureInfo.InvariantCulture;
                datetime = DateTime.ParseExact(value, format, provider);
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
            return true;
        }

        public static DateTime? GetParseDateTime(string value)
        {
            if (TryParseDateTime(value, out DateTime? dateTime, out string _))
            {
                return dateTime;
            }
            return null;
        }

        public static DateTime? GetParseDateTime(string date, string time)
        {
            if (string.IsNullOrEmpty(date) || string.IsNullOrEmpty(time))
                return null;

            string value = date + " " + DateTime.Parse(time).ToString("HH:mm");
            return GetParseDateTime(value);
        }

        public static int? GetParseInt(string value)
        {
            if (int.TryParse(value, out int intValue))
            {
                return intValue;
            }
            return null;
        }

        public static string FormatWithQuotation(this string s, char mark = '"')
        {
            return string.Format("{0}{1}{0}", mark, s);
        }

        public static string FormatDetail(string name, object value, string mark = "", int width = 30)
        {
            name = string.Format("{0}{1}", mark, name);
            return string.Format("{0}: {1}", name.PadRight(width, ' '), value ?? Util.Null);
        }

        public static string FormatDetailWithArrow(string name, object value, string mark = " > ", int width = 30)
        {
            return FormatDetail(name, value, mark, width);
        }

        public static string FormatExport(object v, bool doubleQuote = true)
        {
            if (v == null)
                return null;

            string value = v.ToString();
            if (v is string vs && !string.IsNullOrEmpty(vs))
                value = v.ToString();
            if (v is DateTime vdt)
                value = vdt.ToString(GetExportDateFormat());
            if (v is double vd)
                value = DoubleToString(vd);

            if (doubleQuote)
                value = value.Replace("\"", "\"\""); // replace " to "" for csv format

            return value;
        }

        public static string FormatExportLine(List<string> values)
        {
            if (!values.IsNullOrEmpty())
                return string.Join(",", values.Select(p => p.FormatWithQuotation()));
            return "";
        }

        public static string FormatSubFolderPath(string path, string subFolder = null)
        {
            string output;
            if (string.IsNullOrEmpty(subFolder))
                output = path;
            else if (subFolder.StartsWith("/"))
                output = string.Format("{0}{1}", path, subFolder);
            else
                output = string.Format("{0}/{1}", path, subFolder);

            return new DirectoryInfo(output).FullName;
        }

        public static string FormatPath(string folder, string subFolder = null)
        {
            string path = string.Format("{0}/{1}", GetConfig("RootPath"), folder);
            return FormatSubFolderPath(path, subFolder);
        }

        public static string FormatConsoleAppPath(string folder, string subFolder = null)
        {
            string path = string.Format("{0}/{1}", GetConfig("ConsoleAppPath"), folder);
            return FormatSubFolderPath(path, subFolder);
        }

        public static string FormatWebAppPath(string folder, string subFolder = null)
        {
            string path = string.Format("{0}/{1}", GetConfig("WebAppPath"), folder);
            return FormatSubFolderPath(path, subFolder);
        }

        public static string GetWebAppDocumentPath()
        {
            return FormatWebAppPath("Document");
        }

        public static string GetWebAppDocumentFilePath(string filename)
        {
            return Path.Combine(GetWebAppDocumentPath(), filename);
        }

        public static string GetLogPath(string subFolder = null)
        {
            string path = GetConfig("LogPath");
            if (!string.IsNullOrEmpty(path))
            {
                return FormatSubFolderPath(path, subFolder);
            }
            return FormatPath("Log", subFolder);
        }

        public static string GetUploadPath(string subFolder = null)
        {
            string path = GetConfig("UploadPath");
            if (!string.IsNullOrEmpty(path))
            {
                return FormatSubFolderPath(path, subFolder);
            }
            return FormatPath("Upload", subFolder);
        }

        public static string GetTemporaryPath(string subFolder = null)
        {
            return FormatPath("Temporary", subFolder);
        }

        public static string GetDocumentPath(string subFolder = null)
        {
            string path = GetUploadPath("Document");
            return FormatSubFolderPath(path, subFolder);
        }

        public static string GetExportPath(string subFolder = null)
        {
            string path = GetConfig("ExportPath");
            if (!string.IsNullOrEmpty(path))
            {
                return FormatSubFolderPath(path, subFolder);
            }
            return FormatPath("Export", subFolder);
        }

        public static string GetRawFilePath(string subFolder = null)
        {
            string path = GetUploadPath("RawFile");
            return FormatSubFolderPath(path, subFolder);
        }

        public static string GetMfrs17ReportingPath(string subFolder = null)
        {
            // Temporary until FTP server provided
            return FormatPath(GetConfig("Mfrs17ReportingDirectory", "Mfrs17Reporting"), subFolder);
        }

        public static string GetE1Path(string subFolder = null)
        {
            return FormatPath("E1", subFolder);
        }

        public static string GetE2Path(string subFolder = null)
        {
            return FormatPath("E2", subFolder);
        }

        public static string GetE3Path(string subFolder = null)
        {
            return FormatPath("E3", subFolder);
        }

        public static string GetE4Path(string subFolder = null)
        {
            return FormatPath("E4", subFolder);
        }

        public static string GetSanctionPath(string subFolder = null)
        {
            return FormatPath(GetConfig("SanctionDirectory", "Sanction"), subFolder);
        }

        public static string GetRetroStatementDownloadPath(string subFolder = null)
        {
            return FormatPath("RetroStatement", subFolder);
        }

        public static string GetRetroStatementPath(string subFolder = null)
        {
            string path = GetUploadPath("RetroStatement");
            return FormatSubFolderPath(path, subFolder);
        }

        public static string GetSanctionUploadPath(string subFolder = null)
        {
            string path = GetUploadPath("SanctionUpload");
            return FormatSubFolderPath(path, subFolder);
        }

        public static string GetTreatyPricingRateTableGroupUploadPath(string subFolder = null)
        {
            string path = GetUploadPath("TreatyPricingRateTableGroupUpload");
            return FormatSubFolderPath(path, subFolder);
        }

        public static string GetTreatyPricingCustomOtherUploadPath(string subFolder = null)
        {
            string path = GetUploadPath("TreatyPricingCustomOtherUpload");
            return FormatSubFolderPath(path, subFolder);
        }

        public static string GetTreatyPricingUwQuestionnaireUploadPath(string subFolder = null)
        {
            string path = GetUploadPath("TreatyPricingUwQuestionnaireUpload");
            return FormatSubFolderPath(path, subFolder);
        }

        public static string GetTreatyPricingMedicalTableUploadPath(string subFolder = null)
        {
            string path = GetUploadPath("TreatyPricingMedicalTableUpload");
            return FormatSubFolderPath(path, subFolder);
        }

        public static string GetTreatyPricingFinancialTableUploadPath(string subFolder = null)
        {
            string path = GetUploadPath("TreatyPricingFinancialTableUpload");
            return FormatSubFolderPath(path, subFolder);
        }

        public static string GetTreatyPricingQuotationWorkflowUploadPath(string subFolder = null)
        {
            string path = GetUploadPath("TreatyPricingQuotationWorkflowUpload");
            return FormatSubFolderPath(path, subFolder);
        }

        public static string GetTreatyPricingGroupReferralUploadPath(string subFolder = null)
        {
            string path = GetUploadPath("TreatyPricingGroupReferralUpload");
            return FormatSubFolderPath(path, subFolder);
        }

        public static string GetTreatyPricingReportGenerationPath(string subFolder = null)
        {
            string path = GetUploadPath("TreatyPricingReportGeneration");
            return FormatSubFolderPath(path, subFolder);
        }

        public static string GetTreatyBenefitCodeMappingPath(string subFolder = null)
        {
            string path = GetUploadPath("TreatyBenefitCodeMapping");
            return FormatSubFolderPath(path, subFolder);
        }

        public static string GetRateTableMappingPath(string subFolder = null)
        {
            string path = GetUploadPath("RateTableMapping");
            return FormatSubFolderPath(path, subFolder);
        }

        public static string GetFacMasterListingPath(string subFolder = null)
        {
            string path = GetUploadPath("FacMasterListing");
            return FormatSubFolderPath(path, subFolder);
        }

        public static string GetRateDetailPath(string subFolder = null)
        {
            string path = GetUploadPath("RateDetail");
            return FormatSubFolderPath(path, subFolder);
        }

        public static string GetSoaDataExportGenerationPath(string subFolder = null)
        {
            string path = GetUploadPath("SoaDataExportGenerationPath");
            return FormatSubFolderPath(path, subFolder);
        }

        public static void MakeDir(string path, bool withFileName = true)
        {
            if (!Directory.Exists(path) && !File.Exists(path))
            {
                if (withFileName)
                    new FileInfo(path).Directory.Create();
                else
                    new DirectoryInfo(path).Create();
            }
        }

        public static void DeleteFiles(string path, string fileToDelete)
        {
            string[] files = Directory.GetFiles(path, fileToDelete);
            foreach (string file in files)
            {
                try
                {
                    if (File.Exists(file))
                        File.Delete(file);
                }
                catch (Exception)
                {
                }
            }
        }

        /*public static bool Dollar(object value, out int? dollar, out string error, int multiplier = 100)
        {
            dollar = null;
            error = null;
            try
            {
                double d = Convert.ToDouble(value);
                d /= multiplier;
                dollar = Convert.ToInt32(d);
                return true;
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
        }

        public static string DollarFromDatabase(object value, int multiplier = 100)
        {
            if (DollarFromDatabase(value, out string dollar, out string _, multiplier))
            {
                return dollar;
            }
            return null;
        }

        public static bool DollarFromDatabase(object value, out string dollar, out string error, int multiplier = 100)
        {
            dollar = null;
            error = null;
            try
            {
                if (value == null)
                {
                    return true;
                }
                double d = Convert.ToDouble(value);
                d /= multiplier;
                dollar = d.ToString("N2");
                return true;
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
        }

        public static int? DollarToDatabase(object value, int multiplier = 100)
        {
            if (DollarToDatabase(value, out int? dollar, out string _, multiplier))
            {
                return dollar;
            }
            return null;
        }

        public static bool DollarToDatabase(object value, out int? dollar, out string error, int multiplier = 100)
        {
            dollar = null;
            error = null;
            try
            {
                if (value == null)
                {
                    return true;
                }
                double d = Convert.ToDouble(value);
                d *= multiplier;
                dollar = Convert.ToInt32(d);
                return true;
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
        }*/

        public static bool HasProperty(this object obj, string name)
        {
            if (obj is ExpandoObject)
                return ((IDictionary<string, object>)obj).ContainsKey(name);

            return obj.GetType().GetProperty(name) != null;
        }

        public static object GetProperty(this object obj, string name)
        {
            return obj.GetType().GetProperty(name);
        }

        public static object GetPropertyValue(this object obj, string name, BindingFlags bindingAttr)
        {
            var prop = obj.GetType().GetProperty(name, bindingAttr);
            if (null == prop)
                return null;
            return prop.GetValue(obj);
        }

        public static object GetPropertyValue(this object obj, string name)
        {
            var prop = obj.GetType().GetProperty(name);
            if (null == prop)
                return null;
            return prop.GetValue(obj);
        }

        public static bool SetPropertyValue(this object obj, string name, object value, BindingFlags bindingAttr = BindingFlags.Public | BindingFlags.Instance)
        {
            var prop = obj.GetType().GetProperty(name, bindingAttr);
            if (null == prop || !prop.CanWrite)
                return false;

            if (value == null)
                prop.SetValue(obj, value, null);
            else
                prop.SetValue(obj, value, null);

            return true;
        }

        public static T GetAttributeFrom<T>(this object instance, string propertyName) where T : Attribute
        {
            var attrType = typeof(T);
            var property = instance.GetType().GetProperty(propertyName);

            if (property == null)
                return null;

            return (T)property.GetCustomAttributes(attrType, false).FirstOrDefault();
        }



        public static void ValidateNullAndMaxLength(this object instance, string property, string name, ref List<string> errors)
        {
            string value = instance.GetPropertyValue(property)?.ToString();
            if (string.IsNullOrEmpty(value))
            {
                errors.Add(string.Format(MessageBag.Required, name));
                return;
            }

            var maxLengthAttr = instance.GetAttributeFrom<MaxLengthAttribute>(property);
            if (maxLengthAttr == null)
                return;

            if (value.Length > maxLengthAttr.Length)
                errors.Add(string.Format(MessageBag.MaxLength, name, maxLengthAttr.Length));
        }

        public static string GetTruncatedValue(string value, int length = 32)
        {
            if (!string.IsNullOrEmpty(value) && value.Length > length)
            {
                return string.Format("{0}...", value.Substring(0, length));
            }
            return value;
        }

        public static bool IsValidDouble(object value, out double? output, out string error, bool roundValue = false, int? precision = null)
        {
            output = null;
            error = null;
            try
            {
                if (value == null)
                {
                    return true;
                }
                double d = Convert.ToDouble(value);
                if (roundValue)
                {
                    d = RoundValue(d, precision);
                }

                if (double.IsNaN(d))
                    output = null;
                else if (double.IsInfinity(d))
                    output = null;
                else if (double.IsPositiveInfinity(d))
                    output = null;
                else if (double.IsNegativeInfinity(d))
                    output = null;
                else
                    output = d;

                return true;
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
        }

        public static double? StringToDouble(object value, bool roundValue = true, int? precision = null)
        {
            if (IsValidDouble(value, out double? output, out string _, roundValue, precision))
            {
                return output;
            }
            return null;
        }

        public static string DoubleToString(object value, int? precisionOverwrite = null)
        {
            if (IsValidDouble(value, out double? output, out string _))
            {
                int precision = precisionOverwrite.HasValue ? precisionOverwrite.Value : GetConfigInteger("DoublePrecision", 5);
                return output.HasValue ? output.Value.ToString(string.Format("N{0}", precision)) : null;
            }
            return null;
        }

        public static double RoundValue(double value, int? precision = null)
        {
            if (!precision.HasValue)
            {
                precision = GetConfigInteger("DoublePrecision", 5);
            }

            return (double)decimal.Round((decimal)value, precision.Value, MidpointRounding.AwayFromZero);
        }

        public static double? RoundNullableValue(double? value, int? precision = null)
        {
            if (!value.HasValue)
                return null;

            return RoundValue(value.Value, precision);
        }

        public static string BoolToString(bool value = false, string t = "Y", string f = "N")
        {
            return value ? t : f;
        }

        public static bool StringToBool(string value, out bool result)
        {
            string v = value?.Trim().ToUpper();
            switch (v)
            {
                case "Y":
                case "YES":
                case "TRUE":
                    result = true;
                    return true;
                case "N":
                case "NO":
                case "FALSE":
                    result = false;
                    return true;
                default:
                    result = false;
                    return false;
            }
        }

        public static string MonthYearToQuarter(int year, int month)
        {
            int[] Q1 = { 1, 2, 3 };
            int[] Q2 = { 4, 5, 6 };
            int[] Q3 = { 7, 8, 9 };
            int[] Q4 = { 10, 11, 12 };

            int quarter = 0;
            if (Q1.Contains(month))
            {
                quarter = 1;
            }
            else if (Q2.Contains(month))
            {
                quarter = 2;
            }
            else if (Q3.Contains(month))
            {
                quarter = 3;
            }
            else if (Q4.Contains(month))
            {
                quarter = 4;
            }
            else
            {
                return null;
            }

            return string.Format("{0} Q{1}", year, quarter);
        }

        public static string GetCurrentQuarter()
        {
            DateTime today = DateTime.Today;
            return MonthYearToQuarter(today.Year, today.Month);
        }

        public static string GetQuarterFromDate(DateTime date)
        {
            return MonthYearToQuarter(date.Year, date.Month);
        }

        public static bool ValidateQuarter(string quarter)
        {
            if (string.IsNullOrEmpty(quarter))
                return false;

            Regex rgx = new Regex(@"^[0-9]{4}\s[Q]{1}[1-4]{1}$");
            if (!rgx.IsMatch(quarter))
                return false;

            return true;
        }

        public static bool GetStartEndMonthFromQuarter(string quarter, ref int year, ref int? startMonth, ref int? endMonth)
        {
            if (!ValidateQuarter(quarter))
                return false;

            var quarterArr = ToArraySplitTrim(quarter, 'Q');
            if (quarterArr.Length != 2)
                return false;

            int.TryParse(quarterArr[0], out year);
            string quarterNo = quarterArr[1];

            switch (quarterNo)
            {
                case "1":
                    startMonth = 1;
                    endMonth = 3;
                    break;
                case "2":
                    startMonth = 4;
                    endMonth = 6;
                    break;
                case "3":
                    startMonth = 7;
                    endMonth = 9;
                    break;
                case "4":
                    startMonth = 10;
                    endMonth = 12;
                    break;
                default:
                    return false;
            }

            return true;
        }

        public static string FormatQuarter(string quarter)
        {
            if (string.IsNullOrEmpty(quarter))
                return "";

            if (ValidateQuarter(quarter))
                return quarter;

            string currentYear = DateTime.Today.Year.ToString(); // 2021
            string trimYear = currentYear.Substring(0, 2); // 2021 -> 20

            // Possible Quarter
            // 2021Q1 // 21Q1 // 021Q1 // 021Q12

            var loweredQuarter = quarter.ToLower();
            var quarterArr = ToArraySplitTrim(loweredQuarter, 'q');

            if (quarterArr != null && quarterArr.Count() == 2)
            {
                Regex yearRgx = new Regex(@"^[0-9]{4}$");
                Regex quarterRgx = new Regex(@"^[1-4]{1}$");

                var defaultYear = quarterArr[0];
                var defaultQuarter = quarterArr[1];

                if (yearRgx.IsMatch(defaultYear) && quarterRgx.IsMatch(defaultQuarter))
                    return defaultYear + " Q" + defaultQuarter;

                if (!yearRgx.IsMatch(defaultYear) && defaultYear.Length >= 2 && quarterRgx.IsMatch(defaultQuarter))
                {
                    var year = defaultYear.Substring(defaultYear.Length - 2);
                    return trimYear + year + " Q" + defaultQuarter;
                }
            }

            // retrun initial value as incorrect quarter given
            return quarter;
        }

        public static string FormatIntWithPrecision(int value, int precision = 1)
        {
            string output = 0.ToString().PadLeft(precision, '0');

            return string.Format("{0}.{1}", value, output);
        }

        public static string EncodeString(string s)
        {
            if (string.IsNullOrEmpty(s))
                return null;

            return Uri.EscapeDataString(s);
        }

        public static string DecodeString(string s)
        {
            if (string.IsNullOrEmpty(s))
                return null;

            return Uri.UnescapeDataString(s);
        }

        public static string ReplaceLastOccurrence(string s, string search, string replace)
        {
            int index = s.LastIndexOf(search);
            if (index == -1)
                return s;

            string result = s.Remove(index, search.Length).Insert(index, replace);

            return result;
        }

        public static IQueryable<TResult> LeftOuterJoin<TOuter, TInner, TKey, TResult>(
        this IQueryable<TOuter> outer,
        IQueryable<TInner> inner,
        Expression<Func<TOuter, TKey>> outerKeySelector,
        Expression<Func<TInner, TKey>> innerKeySelector,
        Expression<Func<TOuter, TInner, TResult>> resultSelector)
        {
            // generic methods
            var selectManies = typeof(Queryable).GetMethods()
                .Where(x => x.Name == "SelectMany" && x.GetParameters().Length == 3)
                .OrderBy(x => x.ToString().Length)
                .ToList();
            var selectMany = selectManies.First();
            var select = typeof(Queryable).GetMethods().First(x => x.Name == "Select" && x.GetParameters().Length == 2);
            var where = typeof(Queryable).GetMethods().First(x => x.Name == "Where" && x.GetParameters().Length == 2);
            var groupJoin = typeof(Queryable).GetMethods().First(x => x.Name == "GroupJoin" && x.GetParameters().Length == 5);
            var defaultIfEmpty = typeof(Queryable).GetMethods().First(x => x.Name == "DefaultIfEmpty" && x.GetParameters().Length == 1);

            // need anonymous type here or let's use Tuple
            // prepares for:
            // var q2 = Queryable.GroupJoin(db.A, db.B, a => a.Id, b => b.IdA, (a, b) => new { a, groupB = b.DefaultIfEmpty() });
            var tuple = typeof(Tuple<,>).MakeGenericType(typeof(TOuter), typeof(IQueryable<>).MakeGenericType(typeof(TInner)));
            var paramOuter = Expression.Parameter(typeof(TOuter));
            var paramInner = Expression.Parameter(typeof(IEnumerable<TInner>));
            var groupJoinExpression = Expression.Call(
                null,
                groupJoin.MakeGenericMethod(typeof(TOuter), typeof(TInner), typeof(TKey), tuple),
                new Expression[]
                {
                    Expression.Constant(outer),
                    Expression.Constant(inner),
                    outerKeySelector,
                    innerKeySelector,
                    Expression.Lambda(
                        Expression.New(
                            tuple.GetConstructor(tuple.GetGenericArguments()),
                            new Expression[]
                            {
                                paramOuter,
                                Expression.Call(
                                    null,
                                    defaultIfEmpty.MakeGenericMethod(typeof (TInner)),
                                    new Expression[]
                                    {
                                        Expression.Convert(paramInner, typeof (IQueryable<TInner>))
                                    }
                                )
                            },
                            tuple.GetProperties()
                        ),
                        new[] {paramOuter, paramInner}
                    )
                }
            );

            // prepares for:
            // var q3 = Queryable.SelectMany(q2, x => x.groupB, (a, b) => new { a.a, b });
            var tuple2 = typeof(Tuple<,>).MakeGenericType(typeof(TOuter), typeof(TInner));
            var paramTuple2 = Expression.Parameter(tuple);
            var paramInner2 = Expression.Parameter(typeof(TInner));
            var paramGroup = Expression.Parameter(tuple);
            var selectMany1Result = Expression.Call(
                null,
                selectMany.MakeGenericMethod(tuple, typeof(TInner), tuple2),
                new Expression[]
                {
                    groupJoinExpression,
                    Expression.Lambda(
                        Expression.Convert(Expression.MakeMemberAccess(paramGroup, tuple.GetProperty("Item2")),
                        typeof (IEnumerable<TInner>)),
                        paramGroup
                    ),
                    Expression.Lambda(
                        Expression.New(
                            tuple2.GetConstructor(tuple2.GetGenericArguments()),
                            new Expression[]
                            {
                                Expression.MakeMemberAccess(paramTuple2, paramTuple2.Type.GetProperty("Item1")),
                                paramInner2
                            },
                            tuple2.GetProperties()
                        ),
                        new[]
                        {
                            paramTuple2,
                            paramInner2
                        }
                    )
                }
            );

            // prepares for final step, combine all expressinos together and invoke:
            // var q4 = Queryable.SelectMany(db.A, a => q3.Where(x => x.a == a).Select(x => x.b), (a, b) => new { a, b });
            var paramTuple3 = Expression.Parameter(tuple2);
            var paramTuple4 = Expression.Parameter(tuple2);
            var paramOuter3 = Expression.Parameter(typeof(TOuter));
            var selectManyResult2 = selectMany
                .MakeGenericMethod(
                    typeof(TOuter),
                    typeof(TInner),
                    typeof(TResult)
                )
                .Invoke(
                    null,
                    new object[]
                    {
                        outer,
                        Expression.Lambda(
                            Expression.Convert(
                                Expression.Call(
                                    null,
                                    select.MakeGenericMethod(tuple2, typeof(TInner)),
                                    new Expression[]
                                    {
                                        Expression.Call(
                                            null,
                                            where.MakeGenericMethod(tuple2),
                                            new Expression[]
                                            {
                                                selectMany1Result,
                                                Expression.Lambda(
                                                    Expression.Equal(
                                                        paramOuter3,
                                                        Expression.MakeMemberAccess(paramTuple4, paramTuple4.Type.GetProperty("Item1"))
                                                    ),
                                                    paramTuple4
                                                )
                                            }
                                        ),
                                        Expression.Lambda(
                                            Expression.MakeMemberAccess(paramTuple3, paramTuple3.Type.GetProperty("Item2")),
                                            paramTuple3
                                        )
                                    }
                                ),
                                typeof(IEnumerable<TInner>)
                            ),
                            paramOuter3
                        ),
                        resultSelector
                    }
                );

            return (IQueryable<TResult>)selectManyResult2;
        }

        public static string StringToNumber(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            return Regex.IsMatch(value, @"^[a-zA-Z]+") ? value : string.Format("{0:n}", float.Parse(value));
        }

        public static List<string> SplitStringByLength(this string str, int maxLength)
        {
            List<string> stringList = new List<string>();

            for (int index = 0; index < str.Length; index += maxLength)
            {
                stringList.Add(str.Substring(index, Math.Min(maxLength, str.Length - index)));
            }

            return stringList;
        }
    }
}

namespace System.IO
{
    public static class ExtendedMethod
    {
        public static void RenameTxtFile(this FileInfo fileInfo, string newName)
        {
            fileInfo.MoveTo(newName + ".txt");
        }
    }

}