namespace BuildingBlocks.Core.Application
{
    public interface IEntityDto<TDto, TId> where TDto: class where TId: struct
    {
        TDto Empty();
        bool IsEmpty(TDto dto);
    }
}