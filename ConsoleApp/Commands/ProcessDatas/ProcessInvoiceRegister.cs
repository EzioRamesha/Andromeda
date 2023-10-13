using BusinessObject;
using BusinessObject.InvoiceRegisters;
using BusinessObject.SoaDatas;
using DataAccess.Entities.Identity;
using DataAccess.Entities.InvoiceRegisters;
using DataAccess.EntityFramework;
using Services;
using Services.InvoiceRegisters;
using Shared;
using Shared.ProcessFile;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Web;

namespace ConsoleApp.Commands.ProcessDatas
{
    public class ProcessInvoiceRegister : Command
    {
        public List<Column> Columns { get; set; }

        public string FilePath { get; set; }

        public HttpPostedFileBase PostedFile { get; set; }

        public int? AuthUserId { get; set; }

        public TextFile TextFile { get; set; }

        public List<string> Errors { get; set; }

        public ProcessInvoiceRegister()
        {
            Title = "ProcessInvoiceRegister";
            Description = "To read Invoice No in Invoice Register csv file and insert into database";
            Arguments = new string[]
            {
                "filePath",
            };
            Hide = true;
            Errors = new List<string> { };
            GetColumns();
        }

        public override bool Validate()
        {
            string filepath = CommandInput.Arguments[0];
            if (!File.Exists(filepath))
            {
                PrintError(string.Format(MessageBag.FileNotExists, filepath));
                return false;
            }
            else
            {
                FilePath = filepath;
            }

            return base.Validate();
        }

        public override void Run()
        {
            PrintStarting();

            Process();

            PrintEnding();
        }

        public void Process()
        {
            if (PostedFile != null)
            {
                TextFile = new TextFile(PostedFile.InputStream);
            }
            else if (File.Exists(FilePath))
            {
                TextFile = new TextFile(FilePath);
            }
            else
            {
                throw new Exception("No file can be read");
            }

            TrailObject trail;
            while (TextFile.GetNextRow() != null)
            {
                if (TextFile.RowIndex == 1)
                    continue; // Skip header row

                if (PrintCommitBuffer())
                {
                }
                SetProcessCount();

                bool error = false;

                var entity = new InvoiceRegister();
                InvoiceRegisterBo b = null;
                try
                {
                    b = SetData();

                    if (string.IsNullOrEmpty(b.InvoiceReference))
                    {
                        SetProcessCount("Invoice Reference Empty");
                        Errors.Add(string.Format("Invoice Reference required at row {0}", TextFile.RowIndex));
                        error = true;
                    }
                    else
                    {
                        InvoiceRegisterBo invoiceBo = InvoiceRegisterService.FindByInvoiceReferenceIfrs4(b.InvoiceReference);
                        if (invoiceBo == null)
                        {
                            SetProcessCount("Invoice Reference Not Found");
                            Errors.Add(string.Format("The Invoice Reference doesn't exists: {0} at row {1}", b.InvoiceReference, TextFile.RowIndex));
                            error = true;
                        }
                    }

                    if (string.IsNullOrEmpty(b.InvoiceNumber))
                    {
                        SetProcessCount("Invoice Number Empty");
                        Errors.Add(string.Format("Invoice Number required at row {0}", TextFile.RowIndex));
                        error = true;
                    }
                    else
                    {
                        var maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>("InvoiceNumber");
                        if (maxLengthAttr != null && b.InvoiceNumber.Length > maxLengthAttr.Length)
                        {
                            SetProcessCount("Invoice Number exceeded max length");
                            Errors.Add(string.Format("Invoice Number length can not be more than {0} characters at row {1}", maxLengthAttr.Length, TextFile.RowIndex));
                            error = true;
                        }
                    }

                }
                catch (Exception e)
                {
                    SetProcessCount("Error");
                    Errors.Add(string.Format("Error Triggered: {0} at row {1}", e.Message, TextFile.RowIndex));
                    error = true;
                }

                if (error)
                {
                    continue;
                }

                trail = new TrailObject();
                InvoiceRegisterBo bDb = InvoiceRegisterService.FindByInvoiceReferenceIfrs4(b.InvoiceReference);  // MYR currency
                if (bDb != null)
                {
                    bDb.InvoiceNumber = b.InvoiceNumber;
                    bDb.UpdatedById = b.UpdatedById;
                    InvoiceRegisterService.Update(ref bDb, ref trail);

                    InvoiceRegisterBo iDb = InvoiceRegisterService.FindByInvoiceReferenceIfrs4(b.InvoiceReference, true);  // ORIGINAL currency
                    if (iDb != null)
                    {
                        iDb.InvoiceNumber = b.InvoiceNumber;
                        iDb.UpdatedById = b.UpdatedById;
                        InvoiceRegisterService.Update(ref iDb, ref trail);
                    }

                    var iDbs = InvoiceRegisterService.FindByInvoiceReferenceIfrs17(b.InvoiceReference, InvoiceRegisterBo.ReportingTypeIFRS17);  // IFRS17
                    if (!iDbs.IsNullOrEmpty())
                    {
                        foreach(var irDb in iDbs)
                        {
                            var bo = irDb;
                            bo.InvoiceNumber = b.InvoiceNumber;
                            bo.UpdatedById = b.UpdatedById;
                            InvoiceRegisterService.Update(ref bo, ref trail);
                        }
                    }

                    var iDbCNs = InvoiceRegisterService.FindByInvoiceReferenceIfrs17(b.InvoiceReference, InvoiceRegisterBo.ReportingTypeCNIFRS17);  // IFRS17 by Cell Name
                    if (!iDbCNs.IsNullOrEmpty())
                    {
                        foreach (var iDbCN in iDbCNs)
                        {
                            var bo = iDbCN;
                            bo.InvoiceNumber = b.InvoiceNumber;
                            bo.UpdatedById = b.UpdatedById;
                            InvoiceRegisterService.Update(ref bo, ref trail);
                        }
                    }

                    if (bDb.SoaDataBatchId.HasValue)
                    {
                        using (var db = new AppDbContext())
                        {
                            // Update status to 'Invoiced' into SoaDataBatch
                            db.Database.ExecuteSqlCommand("UPDATE [SoaDataBatches] SET [InvoiceStatus] = {0}, [UpdatedById] = {1}, [UpdatedAt] = {2} WHERE [Id] = {3}",
                                SoaDataBatchBo.InvoiceStatusInvoiced, User.DefaultSuperUserId, DateTime.Now, bDb.SoaDataBatchId);
                            db.SaveChanges();

                            // Update status to 'Offset Completed' into ClaimRegister by SoaDataBatchId
                            db.Database.ExecuteSqlCommand("UPDATE [ClaimRegister] SET [OffsetStatus] = {0}, [MlreInvoiceNumber] = {1}, [MlreInvoiceDate] = {2}, [UpdatedById] = {3}, [UpdatedAt] = {4} WHERE [SoaDataBatchId] = {5}", 
                                ClaimRegisterBo.OffsetStatusOffset, bDb.InvoiceNumber, bDb.InvoiceDate, User.DefaultSuperUserId, DateTime.Now, bDb.SoaDataBatchId);
                            db.SaveChanges();
                        }
                    }
                }
            }

            PrintProcessCount();

            TextFile.Close();

            foreach (string error in Errors)
            {
                PrintError(error);
            }
        }

        public InvoiceRegisterBo SetData()
        {
            var b = new InvoiceRegisterBo
            {
                Id = 0,
                InvoiceReference = TextFile.GetColValue(InvoiceRegisterBo.ColumnInvoiceRef),
                InvoiceNumber = TextFile.GetColValue(InvoiceRegisterBo.ColumnInvoiceNo),
                CreatedById = AuthUserId != null ? AuthUserId.Value : User.DefaultSuperUserId,
                UpdatedById = AuthUserId != null ? AuthUserId : User.DefaultSuperUserId,
            };

            string idStr = TextFile.GetColValue(InvoiceRegisterBo.ColumnId);
            if (!string.IsNullOrEmpty(idStr) && int.TryParse(idStr, out int id))
            {
                b.Id = id;
            }

            return b;
        }

        public List<Column> GetColumns()
        {
            Columns = InvoiceRegisterBo.GetColumns();
            return Columns;
        }
    }
}
