namespace Model.Email
{
    public class EmailUser : IUser
    {
        public object Id { get; }

        public EmailUser(string address)
        {
            Id = address;
        }
    }
}
