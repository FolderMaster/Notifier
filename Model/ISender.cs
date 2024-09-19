namespace Model
{
    public interface ISender
    {
        public Task SendMessage(IMessage message, IUser user);
    }
}
