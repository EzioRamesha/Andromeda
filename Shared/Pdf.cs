using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Shared
{
    public class Pdf
    {
        public PdfPage BasePage;
        public PdfPage CurrentPage;
        public PdfDocument Document;
        private XGraphics Gfx;
        private XBrush Brush;
        private XBrush SubBrush;
        private XFont Font;
        private XFont BoldFont;
        private XFont BoldLargeFont;

        private double CurrentX;
        private double CurrentY;
        private XColor PrimaryColor;
        private XColor SecondaryColor;
        private XColor InputBorderColor;

        public string LogoPath;

        const double X = 26;
        const double Y = 40;
        const double HeaderHeight = 80;
        const double PageWidth = 535;
        const double LPadding = 10;
        const double SectionHeaderHeight = 25;
        const double TableRowHeight = 25;

        public double TextBoxHeight { get; set; } = 18;

        public Pdf(string templatePath = null)
        {
            BasePage = null;
            if (!string.IsNullOrEmpty(templatePath))
            {
                PdfDocument baseDocument = PdfReader.Open(templatePath, PdfDocumentOpenMode.Import);
                BasePage = baseDocument.Pages[0];
            }

            Document = new PdfDocument();
            AddPage();

            PrimaryColor = XColor.FromArgb(1, 80, 159);
            SecondaryColor = XColor.FromArgb(232, 241, 251);
            InputBorderColor = XColor.FromArgb(70, 1, 80, 159);

            Brush = new XSolidBrush(PrimaryColor);
            SubBrush = new XSolidBrush(SecondaryColor);
            Font = new XFont("Segoe UI", 10, XFontStyle.Regular);
            BoldFont = new XFont("Segoe UI", 10, XFontStyle.Bold);
            BoldLargeFont = new XFont("Segoe UI", 12, XFontStyle.Bold);
        }

        public double GetPageWidth()
        {
            return PageWidth;
        }

        public void AddPage()
        {
            if (BasePage == null)
            {
                CurrentPage = Document.AddPage();
                CurrentY = Y;
            } else
            {
                CurrentPage = Document.AddPage(BasePage);
                CurrentY = Y + HeaderHeight;
            }

            Gfx = XGraphics.FromPdfPage(CurrentPage);

            CurrentX = X;
        }

        public void CreateRequestedDate(string date, bool dateBottom = true)
        {
            double height = dateBottom ? 25 : 15;
            XRect cont = new XRect(CurrentX, CurrentY, 100, height);

            string text = "Requested Date";
            if (!dateBottom)
                text += "   " + date;

            Gfx.DrawString(text, Font, Brush, cont, XStringFormats.TopLeft);

            if (dateBottom)
                Gfx.DrawString(date, BoldFont, Brush, cont, XStringFormats.BottomLeft);

            AddVerticalSpace(dateBottom ? 25 : 20);
        }

        public void CenterTitle(string title)
        {
            XRect cont = new XRect(CurrentX, CurrentY, PageWidth, 15);
            Gfx.DrawString(title, BoldLargeFont, Brush, cont, XStringFormats.TopCenter);

            AddVerticalSpace(20);
        }

        public void SectionHeader(string title)
        {
            XRect container = new XRect(X, CurrentY, PageWidth, SectionHeaderHeight);
            XRect text = new XRect(X + LPadding, CurrentY, PageWidth - LPadding, SectionHeaderHeight);
            Size ellipseSize = new Size(5, 5);
            Gfx.DrawRoundedRectangle(SubBrush, container, ellipseSize);
            Gfx.DrawString(title, BoldFont, Brush, text, XStringFormats.CenterLeft);

            AddVerticalSpace(SectionHeaderHeight + 10);
        }

        public void SignatureSection(string title = null, bool newLine = true, double labelLength = 30, double height = 60, int fontSize = 10, double textHeight = 25, string nameFieldName = "Name", string dateFieldName = "Date", string name = null, string date = null) // true = bottom, false = right
        {
            double staticX = CurrentX + LPadding;
            double startY = CurrentY;
            if (!string.IsNullOrEmpty(title))
            {
                XRect text = new XRect(staticX, CurrentY, 200, TextBoxHeight);
                Gfx.DrawString(title, Font, Brush, text, XStringFormats.CenterLeft);
            }

            AddVerticalSpace(TextBoxHeight + height);

            XPen pen = new XPen(PrimaryColor, 1);
            pen.DashStyle = XDashStyle.Dot;
            Gfx.DrawLine(pen, staticX, CurrentY, staticX + 200, CurrentY);
            AddVerticalSpace(5);

            if (string.IsNullOrEmpty(name))
                name = "";

            if (string.IsNullOrEmpty(date))
                date = "";

            //string nameField = nameFieldName.PadRight(20, ' ');
            //string dateField = dateFieldName.PadRight(20, ' ');
            XFont font = new XFont("Segoe UI", fontSize, XFontStyle.Regular);

            XRect fieldCont = new XRect(staticX, CurrentY, labelLength, textHeight);
            Gfx.DrawString(nameFieldName, font, Brush, fieldCont, XStringFormats.TopLeft);
            Gfx.DrawString(dateFieldName, font, Brush, fieldCont, XStringFormats.BottomLeft);

            double valueLength = 200 - labelLength;
            XRect valueCont = new XRect(staticX + labelLength, CurrentY, valueLength, textHeight);
            Gfx.DrawString(string.Format(":  {0}", name), font, Brush, valueCont, XStringFormats.TopLeft);
            Gfx.DrawString(string.Format(":  {0}", date), font, Brush, valueCont, XStringFormats.BottomLeft);
            AddVerticalSpace(25);

            if (newLine)
            {
                CurrentX = X;
            } 
            else
            {
                CurrentY = startY;
                CurrentX += 300;
            }
        }

        public void ExtendedSectionLabel(string title)
        {
            XBrush whiteBrush = XBrushes.White;
            XRect container = new XRect(0, CurrentY, CurrentPage.Width, SectionHeaderHeight);
            XRect text = new XRect(X + LPadding, CurrentY, PageWidth, SectionHeaderHeight);
            Gfx.DrawRectangle(Brush, container);
            Gfx.DrawString(title, BoldFont, whiteBrush, text, XStringFormats.CenterLeft);

            AddVerticalSpace(SectionHeaderHeight + 5);
        }

        public void AddTextBox(string label = null, string input = null, double labelLength = 120, double inputLength = 390, int rows = 1, bool newLine = true) // true = bottom, false = right
        {
            if (string.IsNullOrEmpty(input))
                input = "";

            double textBoxHeight = TextBoxHeight * rows;

            CurrentX += LPadding;
            // Label
            if (!string.IsNullOrEmpty(label))
            {
                XRect labelCont = new XRect(CurrentX, CurrentY, labelLength, textBoxHeight);
                Gfx.DrawString(label, Font, Brush, labelCont, XStringFormats.CenterLeft);
                CurrentX += labelLength;
            }

            // Input Container
            XPen pen = new XPen(InputBorderColor, 0.5);
            XRect inputCont = new XRect(CurrentX, CurrentY, inputLength, textBoxHeight);
            Size ellipseSize = new Size(5, 5);
            Gfx.DrawRoundedRectangle(pen, inputCont, ellipseSize);

            // Input
            CurrentX += LPadding;
            if (rows == 1)
            {
                XRect text = new XRect(CurrentX, CurrentY, inputLength - LPadding, textBoxHeight);
                Gfx.DrawString(input, Font, Brush, text, XStringFormats.CenterLeft);
            }
            else
            {
                WriteMultipleLines(input, rows, inputLength - (LPadding + 5));
            }

            // SetPosition
            if (newLine)
            {
                CurrentX = X;
                CurrentY += textBoxHeight + 6;
            }
            else
            {
                CurrentX += inputLength - LPadding;
            }
        }

        public void WriteMultipleLines(string fullText, int rows, double width)
        {
            string[] splitTexts = fullText.Split(' ');
            string combinedString = "";
            double currentWidth = 0;
            int currentRow = 1;
            double y = CurrentY;
            foreach (string text in splitTexts)
            {
                XSize size = Gfx.MeasureString(text, Font);
                double textWidth = size.Width;
                if (currentWidth + textWidth > width)
                {
                    XRect container = new XRect(CurrentX, y, width, TextBoxHeight);
                    Gfx.DrawString(combinedString, Font, Brush, container, XStringFormats.CenterLeft);
                    combinedString = "";
                    currentWidth = 0;
                    currentRow++;
                    y += TextBoxHeight;
                    if (currentRow > rows)
                        return;
                }

                currentWidth += textWidth;
                combinedString += text + " ";
            }

            if (!string.IsNullOrEmpty(combinedString))
            {
                XRect container = new XRect(CurrentX, y, width, TextBoxHeight);
                Gfx.DrawString(combinedString, Font, Brush, container, XStringFormats.CenterLeft);
            }

        }

        public void AddVerticalSpace(double height)
        {
            CurrentY += height;
        }

        public void DrawLine()
        {
            XPen pen = new XPen(SecondaryColor, 0.5);
            Gfx.DrawLine(pen, X, CurrentY, X + PageWidth, CurrentY);
        }

        public void AddText(string text, bool bold = false, bool withPadding = true, bool large = false, double width = PageWidth, double height = 0)
        {
            if (string.IsNullOrEmpty(text))
                text = "";

            if (height == 0)
                height = TextBoxHeight;

            if (withPadding)
                CurrentX += LPadding;

            XFont font = large ? BoldLargeFont : bold ? BoldFont : Font;
            XRect rect = new XRect(CurrentX, CurrentY, width, height);
            Gfx.DrawString(text, font, Brush, rect, XStringFormats.CenterLeft);

            CurrentX += width;
            if (CurrentX >= PageWidth)
            {
                CurrentY += height;
                CurrentX = X;
            }
        }

        public void DrawPowerTable(string module, string type, string power)
        {
            if ((TableRowHeight * 3) + CurrentY > CurrentPage.Height)
                AddPage();

            Size ellipseSize = new Size(1, 1);
            XRect headerContainer = new XRect(CurrentX, CurrentY, 105, TableRowHeight * 3);
            Gfx.DrawRoundedRectangle(SubBrush, headerContainer, ellipseSize);

            double textWidth = 105 - LPadding;
            //CurrentX = X;
            AddText("Module", false, true, false, textWidth, TableRowHeight);
            AddText(module, false, true, false, 450, TableRowHeight);

            DrawLine();

            //CurrentX = X;
            AddText("Type", false, true, false, textWidth, TableRowHeight);
            AddText(type, false, true, false, 450, TableRowHeight);

            //CurrentX = X;
            AddText("Power", false, true, false, textWidth, TableRowHeight);
            AddText(power, false, true, false, 450, TableRowHeight);
        }

        public void DrawITSection()
        {
            ExtendedSectionLabel("IT OFFICE USE ONLY");
            AddVerticalSpace(10);
            SignatureSection("Processed By");
            //AddTextBox("Receive by IT Liaison");
            //AddTextBox("Date", "", 120, 200);
            //AddVerticalSpace(15);

            //AddTextBox("IT System Administrator");
            //AddTextBox("Date", "", 120, 200);
            //AddVerticalSpace(15);

            //AddTextBox("Released by IT Liaison");
            //AddTextBox("Date", "", 120, 200);
            //AddVerticalSpace(15);
        }

        public void PrintTable(Table table)
        {
            if (table.HasHeader)
            {
                XRect headerRect = new XRect(CurrentX, CurrentY, table.Width, TextBoxHeight);
                Gfx.DrawRectangle(SubBrush, headerRect);

                double RowX = CurrentX;
                foreach (Cell cell in table.Header)
                {
                    XRect textRect = new XRect(RowX + LPadding, CurrentY, cell.Width - LPadding, TextBoxHeight);
                    Gfx.DrawString(cell.Content, BoldFont, Brush, textRect, XStringFormats.CenterLeft);
                    RowX += cell.Width;
                }
                AddVerticalSpace(TextBoxHeight);

                XPen pen = new XPen(InputBorderColor, 0.5);
                foreach (Row row in table.Rows)
                {
                    RowX = CurrentX;
                    double rowHeight = row.Height != 0 ? row.Height : TextBoxHeight;
                    //XRect rowRect = new XRect(CurrentX, CurrentY, table.Width, SectionHeaderHeight);
                    //Gfx.DrawRectangle(SubBrush, rowRect);

                    foreach (Cell cell in row.Cells)
                    {
                        XRect textRect = new XRect(RowX + LPadding, CurrentY, cell.Width - LPadding, rowHeight);
                        Gfx.DrawString(cell.Content, Font, Brush, textRect, XStringFormats.CenterLeft);
                        RowX += cell.Width;
                    }

                    AddVerticalSpace(rowHeight);
                    Gfx.DrawLine(pen, CurrentX, CurrentY, CurrentX + table.Width, CurrentY);
                }
            }

        }
    }

    public class Table
    {
        public List<Row> Rows;
        public List<Cell> Header;
        public bool HasHeader;
        public double Width;

        public Table(double width)
        {
            Width = width;
            Rows = new List<Row>();
        }

        public void AddHeader(List<Cell> cells)
        {
            int cellWithoutWidth = cells.Where(c => c.Width == 0).Count();
            double totalCellWidth = cells.Sum(c => c.Width);
            //int totalCells = cells.Count();
            double remainingWidth = Width - totalCellWidth;
            double defaultWidth = remainingWidth / cellWithoutWidth;

            foreach (Cell cell in cells)
            {
                if (cell.Width != 0)
                    continue;

                cell.Width = defaultWidth;
            }
            //List<Cell> formattedCells = cells.Where(c => c.Width == 0).Select(c => { c.Width = defaultWidth; return c; }).ToList();
            Header = cells;

            HasHeader = true;
        }

        public void AddRow(Row row)
        {
            if (HasHeader)
            {
                for(int i = 0; i < Math.Min(Header.Count(), row.Cells.Count()); i++)
                {
                    row.Cells[i].Width = Header[i].Width;
                }
            }
            Rows.Add(row);
        }
    }

    public class Row
    {
        public double Height;
        public List<Cell> Cells;

        public Row(List<Cell> cells, double height = 0)
        {
            Cells = cells;
            Height = height;
        }
    }
    
    public class Cell
    {
        public string Content;
        public double Width;

        public Cell(string content, double width = 0)
        {
            Content = content;
            if (width != 0)
                Width = width;
        }
    }
}
