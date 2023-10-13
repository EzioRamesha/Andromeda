using Shared.Trails;

namespace Shared.DataAccess
{
    public class Result
    {
        public MessageBag MessageBag { get; set; }

        public DataTrail DataTrail { get; set; }

        public bool Valid { get; set; } = true;

        public string Table { get; set; }

        public string Controller { get; set; }

        public Result()
        {
            MessageBag = new MessageBag();
        }

        public bool IsValid()
        {
            Valid = true;
            if (MessageBag == null)
            {
                Valid = false;
                return Valid;
            }

            Valid = !(MessageBag.Errors != null && MessageBag.Errors.Count > 0);
            return Valid;
        }

        public void AddSuccess(string success)
        {
            MessageBag.Success.Add(success);
            IsValid();
        }

        public void AddSuccessRange(string[] success)
        {
            MessageBag.Success.AddRange(success);
            IsValid();
        }

        public void AddError(string error)
        {
            MessageBag.Errors.Add(error);
            IsValid();
        }

        public void AddErrorRange(string[] errors)
        {
            MessageBag.Errors.AddRange(errors);
            IsValid();
        }

        public void AddWarning(string warning)
        {
            MessageBag.Warnings.Add(warning);
            IsValid();
        }

        public void AddWarningRange(string[] warnings)
        {
            MessageBag.Warnings.AddRange(warnings);
            IsValid();
        }

        public void AddErrorRecordInUsed()
        {
            MessageBag.Errors.Add(MessageBag.UnableDeleteRecordInUsed);
            IsValid();
        }

        public void AddTakenError(string key, string value)
        {
            MessageBag.AddTakenError(key, value);
            IsValid();
        }

        public void AddUnableInsert()
        {
            MessageBag.AddUnableInsert();
            IsValid();
        }

        public void AddUnableUpdate()
        {
            MessageBag.AddUnableUpdate();
            IsValid();
        }

        public void AddUnableDelete()
        {
            MessageBag.AddUnableDelete();
            IsValid();
        }

        public string[] ToSuccessArray()
        {
            return MessageBag.Success.ToArray();
        }

        public string[] ToErrorArray()
        {
            return MessageBag.Errors.ToArray();
        }

        public string[] ToWarningArray()
        {
            return MessageBag.Warnings.ToArray();
        }
    }
}