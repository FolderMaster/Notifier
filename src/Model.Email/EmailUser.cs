namespace Model.Email
{
    public record EmailUser : IUser
    {
        public object Id { get; set; }

        public EmailUser(string address) => Id = address;
    }
}
