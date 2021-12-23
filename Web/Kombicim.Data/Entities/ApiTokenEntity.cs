namespace Kombicim.Data.Entities
{
    public class ApiTokenEntity : CreationTimestampedEntity
    {
        public int UserId { get; set; }
        public string Token { get; set; }

        public virtual UserEntity User { get; set; }
    }
}
