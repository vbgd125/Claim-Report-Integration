namespace TSJ_CRI.Authentication
{
    public class UserSession
    {
        public int userid { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string roles { get; set; }
        public string org_id { get; set; }
        public string status { get; set; }
    }
}