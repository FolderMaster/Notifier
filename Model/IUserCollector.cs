namespace Model
{
    public interface IUserCollector
    {
        public Task<bool> CheckUserId(object userId);
    }
}
