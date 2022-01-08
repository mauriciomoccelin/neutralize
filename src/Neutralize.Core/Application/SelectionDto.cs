using System;

namespace Neutralize.Application
{
    public class SelectionDto<TValue> where TValue : struct
    {
        public TValue Value { get; set; }
        public string Text { get; set; }
        
        public string Additional { get; set; }

        public SelectionDto() { }

        public SelectionDto(TValue value, string displayText)
        {
            Value = value;
            Text = displayText;
            Additional = null;
        }
    }

    public class SelectionInt64Dto : SelectionDto<long>
    {
        public SelectionInt64Dto()
        {
        }
    }
    
    public class SelectionGuidDto : SelectionDto<Guid>
    {
        public SelectionGuidDto()
        {
        }
    }
}
