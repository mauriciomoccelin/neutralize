namespace Neutralize.Application
{
    public class EntityDto<TId> : IEntityDto<TId> where TId : struct
    {
        public TId Id { get; set; }
    }
}
