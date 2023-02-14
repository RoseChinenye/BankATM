namespace ATM_DAL.Domain
{
    public abstract class User
    {
        protected User()
        {

        }

        public static string Pin { get; set; }
        public static string CardNumber { get; set; }

    }

}