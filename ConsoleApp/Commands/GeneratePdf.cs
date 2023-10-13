using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Commands
{
    public class GeneratePdf : Command
    {
        public GeneratePdf()
        {
            Title = "GeneratePdf";
            Description = "Generate a Pdf for testing";
        }

        public override void Run()
        {
            Pdf document = new Pdf("C:/Users/camkk/Desktop/Andromeda/PDF Samples & Template/ExGratia_Template.pdf");

            document.SectionHeader("EMPLOYEE DETAILS");
            document.AddTextBox("Employee Name");
            document.AddTextBox("Username", "cameron");
            document.AddTextBox("Email");
            document.AddVerticalSpace(30);
            
            document.SectionHeader("DEPARTMENT & REQUEST TYPE");
            document.AddTextBox("Department");
            document.AddTextBox("Request Type");
            document.AddTextBox("Requested By");
            document.AddVerticalSpace(30);


            document.SectionHeader("ACCESS POWER");
            document.AddText("Refer Appendix A", false);
            document.AddVerticalSpace(30);

            document.ExtendedSectionLabel("IT OFFICE USE ONLY");

            //document.AddPage();

            //document.AddText("Treaty & Pricing", true, false);
            //document.AddVerticalSpace(5);
            //document.DrawPowerTable("Quotation", "Action", "Create, Read, Update");
            //document.AddVerticalSpace(10);
            //document.DrawPowerTable("Treaty", "Action", "Create, Read, Update");

            document.AddTextBox("Treaty & Pricing", "Testing");
            //document.AddTextBox("Claim ID", "CL/2020/11/0000007", inputLength: 150, newLine: false);
            //document.AddTextBox("Age Next Birth", "7", labelLength: 80, inputLength: 150);
            //document.AddText("Comments on Chronology of Events", false);
            //document.AddVerticalSpace(5);
            //document.AddTextBox(input: "CL/2020/11/0000007dasdddddddddddddddddddddasfasfa afvsffffffb afsaffffffffffffffffffq qwrfqwfwqqqqqqqqqqqq asudaosndansdasd", inputLength: 510, rows: 3);
            //document.AddTextBox("Recommendation by Claim Assessor", "", labelLength: 200, inputLength: 310);


            //document.AddText("Comments", false);
            //document.AddVerticalSpace(5);
            //document.AddTextBox(input: "CL/2020/11/0000007dasdddddddddddddddddddddasfasfa afvsffffffb afsaffffffffffffffffffq qwrfqwfwqqqqqqqqqqqq asudaosndansdasd", inputLength: 300, rows: 5, newLine: false);
            //document.SignatureSection(labelLength: 80, dateFieldName: "Date Sign-Off", name: "Rocky", height: 40);
            //document.SignatureSection(name: "Rocky 2");

            //Pdf document = new Pdf(Server.MapPath("~/Document/User_Request_Form_Template.pdf"));
            document.TextBoxHeight = 15;
            document.AddPage();

            document.CreateRequestedDate(DateTime.Today.ToString(Util.GetDateFormat()), false);
            document.AddVerticalSpace(5);

            document.SectionHeader("DETAILS");
            document.AddTextBox("Ceding Company", "", 130);
            document.AddTextBox("Insured Name", "", 130);
            document.AddTextBox("Claim ID", "", 130, inputLength: 150, newLine: false);
            document.AddTextBox("Claim Type", "", labelLength: 80, inputLength: 150);
            document.AddTextBox("Date of Birth", "", 130, inputLength: 150, newLine: false);
            document.AddTextBox("Age Next Birth", "7", labelLength: 80, inputLength: 150);
            document.AddTextBox("Gender", "", 130, inputLength: 150, newLine: false);
            document.AddTextBox("Policy No", "", labelLength: 80, inputLength: 150);
            document.AddTextBox("Date of Commencement", "", 130, inputLength: 150, newLine: false);
            document.AddTextBox("Date of Event", "", labelLength: 80, inputLength: 150);
            document.AddTextBox("Benefit Code", "", 130, inputLength: 150, newLine: false);
            document.AddTextBox("TreatyCode", "", labelLength: 80, inputLength: 150);
            document.AddTextBox("Cause of Event / Diagnosis", "", 130);
            document.AddTextBox("Claim Amount (RM)", "", 130);
            document.AddVerticalSpace(5);

            document.SectionHeader("COMMENTS & RECOMMENDATION");
            document.AddText("Comments on Chronology of Events", false);
            document.AddVerticalSpace(5);
            document.AddTextBox(input: "", inputLength: 510, rows: 2);
            document.AddText("Recommendation by Claim Assessor", false);
            document.AddVerticalSpace(5);
            document.AddTextBox(input: "", inputLength: 510, rows: 2);
            document.AddVerticalSpace(5);

            document.SectionHeader("APPROVAL BY CLAIMS COMMITTEE");
            document.AddText("Comments", false);
            document.AddVerticalSpace(5);
            document.AddTextBox(input: "", inputLength: 300, rows: 4, newLine: false);
            document.SignatureSection(labelLength: 80, fontSize: 8, textHeight: 20, dateFieldName: "Date Sign-Off", name: "", height: 20);
            document.AddVerticalSpace(5);
            document.DrawLine();
            document.AddText("Comments", false);
            document.AddVerticalSpace(5);
            document.AddTextBox(input: "", inputLength: 300, rows: 4, newLine: false);
            document.SignatureSection(labelLength: 80, fontSize: 8, textHeight: 20, dateFieldName: "Date Sign-Off", name: "", height: 20);
            document.AddVerticalSpace(5);
            document.DrawLine();
            document.AddText("Comments", false);
            document.AddVerticalSpace(5);
            document.AddTextBox(input: "", inputLength: 300, rows: 4, newLine: false);
            document.SignatureSection(labelLength: 80, fontSize: 8, textHeight:20, dateFieldName: "Date Sign-Off", name: Util.GetConfig("ExGratiaCeoName"), height: 20);


            string filename = "C:/Users/camkk/Desktop/Andromeda/PDF Samples & Template/Test.pdf";
            document.Document.Save(filename);
        }
    }
}
