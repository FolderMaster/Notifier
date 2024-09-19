namespace Model.Email
{
    internal class EmailUser : IUser
    {
        public object Id { get; }

        public EmailUser(string address)
        {
            Id = address;
        }
    }
}
