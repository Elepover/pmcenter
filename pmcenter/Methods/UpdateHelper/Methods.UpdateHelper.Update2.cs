using System.Collections.Generic;

namespace pmcenter
{
    public static partial class Methods
    {
        public static partial class UpdateHelper
        {
            public class Update2
            {
                public Update2()
                {
                    Latest = "0.0.0.0";
                    UpdateLevel = UpdateLevel.Optional;
                    UpdateCollection = new List<Update>();
                }
                public string Latest;
                public UpdateLevel UpdateLevel;
                public List<Update> UpdateCollection;
            }
        }
    }
}
