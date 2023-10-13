using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class DirectRetroStatusFileBo
    {
        public int Id { get; set; }

        public int DirectRetroId { get; set; }

        public DirectRetroBo DirectRetroBo { get; set; }

        public int StatusHistoryId { get; set; }

        public StatusHistoryBo StatusHistoryBo { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public string GetProcessFilePath()
        {
            return string.Format("{0}/DirectRetro.{1}.process.summary.log.txt", Util.GetLogPath("DirectRetroSummary"), Id);
        }

        public string GetFilePath()
        {
            if (StatusHistoryBo != null)
            {
                switch (StatusHistoryBo.Status)
                {
                    case DirectRetroBo.RetroStatusProcessing:
                        return GetProcessFilePath();
                }
            }
            return null;
        }
    }
}
