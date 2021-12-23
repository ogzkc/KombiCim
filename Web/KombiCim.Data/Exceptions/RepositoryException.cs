namespace Kombicim.Data.Exceptions
{
    public class RepositoryException : Exception
    {
        public RepositoryException(string message) : base(message)
        {
        }
        public string RepositoryName { get; set; }
        public object Value { get; set; }
        public RepositoryExceptionType ExceptionType { get; set; }


        public static RepositoryException NotCenterDevice(string deviceId)
        {
            return new RepositoryException($"{deviceId} id'li device Center değil fakat aktif profili istendi.");
        }

    }
}
