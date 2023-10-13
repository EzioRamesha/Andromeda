using Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using BaseWord = Microsoft.Office.Interop.Word;

namespace Shared.ProcessFile
{
    public class Word
    {
        public BaseWord.Application XApp { get; set; }
        public BaseWord.Document XDocument { get; set; }
        public BaseWord.Range XRange { get; set; }

        public BaseWord.WdLineStyle XLineStyle { get; set; }

        public BaseWord.WdParagraphAlignment ParagraphAlignCenter { get; set; }
        public BaseWord.WdParagraphAlignment ParagraphAlignLeft { get; set; }
        public BaseWord.WdParagraphAlignment ParagraphAlignRight { get; set; }
        public BaseWord.WdParagraphAlignment ParagraphAlignJustified { get; set; }

        public string FilePath { get; set; }

        public Word()
        {
        }

        public Word(string filepath)
        {
            var finfo = new FileInfo(filepath);
            if (!Directory.Exists(finfo.Directory.FullName))
            {
                throw new Exception(string.Format(MessageBag.DirectoryNotExists, finfo.Directory.FullName));
            }

            FilePath = filepath;
            XApp = new BaseWord.Application();

            XApp.Visible = false;
            XApp.DisplayAlerts = BaseWord.WdAlertLevel.wdAlertsNone; // Disable showing alert message of overwritting of existing file

            if (File.Exists(filepath))
            {
                XDocument = XApp.Documents.Open(filepath, ReadOnly: false);
                XDocument.Activate();
                XDocument.ActiveWindow.View.Type = BaseWord.WdViewType.wdNormalView;
                XRange = XDocument.Range();
                XLineStyle = BaseWord.WdLineStyle.wdLineStyleSingle;

                ParagraphAlignCenter = BaseWord.WdParagraphAlignment.wdAlignParagraphCenter;
                ParagraphAlignLeft = BaseWord.WdParagraphAlignment.wdAlignParagraphLeft;
                ParagraphAlignRight = BaseWord.WdParagraphAlignment.wdAlignParagraphRight;
                ParagraphAlignJustified = BaseWord.WdParagraphAlignment.wdAlignParagraphJustify;
            }
            else
            {
                //
            }
        }

        public void Close()
        {
            if (XDocument != null)
            {
                XDocument.Close();
                Marshal.ReleaseComObject(XDocument);
            }

            if (XApp != null)
            {
                XApp.Quit();
                Marshal.ReleaseComObject(XApp);
            }
        }

        public void Save()
        {
            XDocument.SaveAs2(FilePath);
            Close();
        }

        public void FindAndReplace(object findText, object replaceWithText)
        {
            object matchCase = false;
            object matchWholeWord = true;
            object matchWildCards = false;
            object matchSoundsLike = false;
            object matchAllWordForms = false;
            object forward = true;
            object format = false;
            object matchKashida = false;
            object matchDiacritics = false;
            object matchAlefHamza = false;
            object matchControl = false;
            object replace = 2;
            object wrap = 1;

            //execute find and replace
            int maxLength = 150;

            if (replaceWithText != null && replaceWithText.ToString().Length > maxLength)
            {
                var subs = Util.SplitStringByLength(replaceWithText.ToString(), maxLength);
                object shortReplaceWith;
                int i = 0;

                foreach (string sub in subs)
                {
                    shortReplaceWith = sub + findText.ToString();

                    XRange.Find.Execute(ref findText, ref matchCase, ref matchWholeWord,
                        ref matchWildCards, ref matchSoundsLike, ref matchAllWordForms, ref forward, ref wrap, ref format, ref shortReplaceWith, ref replace,
                        ref matchKashida, ref matchDiacritics, ref matchAlefHamza, ref matchControl);
                }

                //Remove last findText from completed replacement
                shortReplaceWith = "";
                XRange.Find.Execute(ref findText, ref matchCase, ref matchWholeWord,
                    ref matchWildCards, ref matchSoundsLike, ref matchAllWordForms, ref forward, ref wrap, ref format, ref shortReplaceWith, ref replace,
                    ref matchKashida, ref matchDiacritics, ref matchAlefHamza, ref matchControl);
            }
            else
            {
                XRange.Find.Execute(ref findText, ref matchCase, ref matchWholeWord,
                    ref matchWildCards, ref matchSoundsLike, ref matchAllWordForms, ref forward, ref wrap, ref format, ref replaceWithText, ref replace,
                    ref matchKashida, ref matchDiacritics, ref matchAlefHamza, ref matchControl);
            }
        }
    }
}
