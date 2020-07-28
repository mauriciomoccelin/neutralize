namespace BuildingBlocks.Application
{
    public class EntityDto<TDto, TId> : IEntityDto<TDto, TId> where TDto : class where TId : struct
    {
        public TId Id { get; set; }
    }
}