using System.Text;
using Telegram.Bot.Types;

namespace pmcenter
{
    public static partial class Methods
    {
        public static string GetComposedUsername(User user, bool includeUsername = true, bool includeId = false)
        {
            var sb = new StringBuilder(user.FirstName);
            if (!(string.IsNullOrEmpty(user.LastName) || string.IsNullOrWhiteSpace(user.LastName)))
                sb.Append($" {user.LastName}");
            if (!includeUsername && !includeId) return sb.ToString();
            if (includeUsername && !includeId && string.IsNullOrEmpty(user.Username)) return sb.ToString();
            sb.Append('(');
            if (includeUsername && !string.IsNullOrEmpty(user.Username))
                sb.Append($"@{user.Username}");
            if (includeId) sb.Append($", {user.Id}");
            sb.Append(')');
            return sb.ToString();
        }
    }
}
