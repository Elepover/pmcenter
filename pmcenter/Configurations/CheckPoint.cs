namespace pmcenter
{
    public class CheckPoint
    {
        public CheckPoint(long tick = 0, string name = "Unspecified")
        {
            Tick = tick;
            Name = name;
        }
        public long Tick;
        public string Name;
    }
}
