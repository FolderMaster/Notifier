namespace Model
{
    public interface ISender : IUserCollector
    {
        public Task SendMessage(IMessage message, IUser user);
    }
}
