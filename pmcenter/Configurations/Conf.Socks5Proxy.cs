namespace pmcenter
{
    public static partial class Conf
    {
        public class Socks5Proxy
        {
            public Socks5Proxy()
            {
                ServerName = "example.com";
                ServerPort = 1080;
                Username = "username";
                ProxyPass = "password";
            }
            public string ServerName { get; set; }
            public int ServerPort { get; set; }
            public string Username { get; set; }
            public string ProxyPass { get; set; }
        }
    }
}
