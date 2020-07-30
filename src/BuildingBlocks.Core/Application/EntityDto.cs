using System;

namespace BuildingBlocks.Core.Application
{
    public class EntityDto<TDto, TId> : IEntityDto<TDto, TId> where TDto : class where TId : struct
    {
        private readonly TDto empty = Activator.CreateInstance<TDto>(); 
        
        public TId Id { get; set; }

        public EntityDto() { }
        
        public TDto Empty() => empty;
        public bool IsEmpty(TDto dto) => empty.Equals(dto);
    }
}