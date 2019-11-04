using System.Collections.Generic;

namespace pmcenter
{
    public partial class Conf
    {
        public class Update2
        {
            public Update2()
            {
                Latest = "0.0.0.0";
                UpdateLevel = UpdateLevel.Optional;
                UpdateCollection = new List<Update>();
            }
            public string Latest { get; set; }
            public UpdateLevel UpdateLevel { get; set; }
            public List<Update> UpdateCollection { get; set; }
        }
    }
}