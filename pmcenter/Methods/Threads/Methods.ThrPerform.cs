namespace pmcenter
{
    public static partial class Methods
    {
        public static void ThrPerform()
        {
            if (Vars.IsPerformanceTestExecuting) return;
            Vars.IsPerformanceTestExecuting = true;
            Vars.PerformanceScore = 0;
            while (!(Vars.IsPerformanceTestEndRequested || Vars.IsShuttingDown))
            {
                Vars.PerformanceScore += 1;
            }
            Vars.IsPerformanceTestExecuting = false;
            Vars.IsPerformanceTestEndRequested = false;
        }
    }
}
