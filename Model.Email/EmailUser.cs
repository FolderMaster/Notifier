namespace Model.Email
{
    public class EmailUser : IUser
    {
        public object Id { get; private set; }

        public EmailUser(string address) => Id = address;
    }
}
