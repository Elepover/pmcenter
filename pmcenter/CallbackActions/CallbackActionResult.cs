namespace pmcenter.CallbackActions
{
    public class CallbackActionResult
    {
        public CallbackActionResult(string status, bool succeeded = true)
        {
            Status = status;
            Succeeded = succeeded;
        }
        public string Status;
        public bool Succeeded;
    }
}
