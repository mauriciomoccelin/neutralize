namespace Neutralize.Application
{
    public interface IEntityDto<TId> where TId: struct
    {
        public TId Id { get; set; }
    }
}
