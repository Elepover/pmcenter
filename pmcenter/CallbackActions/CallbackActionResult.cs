namespace pmcenter.CallbackActions
{
    public class CallbackActionResult
    {
        public CallbackActionResult(string status, bool showAsAlert = false, bool succeeded = true)
        {
            Status = status;
            Succeeded = succeeded;
            ShowAsAlert = showAsAlert;
        }
        public string Status;
        public bool Succeeded;
        public bool ShowAsAlert;
    }
}
