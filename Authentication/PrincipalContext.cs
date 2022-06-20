namespace TSJ_CRI.Authentication
{
    internal class PrincipalContext
    {
        private object domain;
        private string v1;
        private string v2;

        public PrincipalContext(object domain, string v1, string v2)
        {
            this.domain = domain;
            this.v1 = v1;
            this.v2 = v2;
        }
    }
}