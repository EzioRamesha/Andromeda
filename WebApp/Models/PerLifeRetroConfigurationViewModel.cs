using PagedList;

namespace WebApp.Models
{
    public class PerLifeRetroConfigurationViewModel
    {
        //Search criteria
        //public PerLifeRetroConfigurationTreatyViewModel Treaty { get; set; }
        //public PerLifeRetroConfigurationRatioViewModel Ratio { get; set; }

        ////Search results
        //public int? SearchResultsTreaty { get; set; }
        //public int? SearchResultsRatio { get; set; }
        //public IPagedList<PerLifeRetroConfigurationTreatyViewModel> Treaties { get; set; }
        //public IPagedList<PerLifeRetroConfigurationRatioViewModel> Ratios { get; set; }
        public int? ActiveTab { get; set; }

        public PerLifeRetroConfigurationViewModel() { }
    }
}