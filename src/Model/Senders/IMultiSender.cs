namespace Model.Senders
{
    public interface IMultiSender : ISender
    {
        public Task SendMessage(IMessage message, IEnumerable<IUser> users);
    }
}
