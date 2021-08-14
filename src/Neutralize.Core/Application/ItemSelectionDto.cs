namespace Neutralize.Application
{
    public class ItemSelectionDto<TValue> where TValue : struct
    {
        public TValue Value { get; set; }
        public string Text { get; set; }

        public string Additional { get; set; }

        public ItemSelectionDto() { }
        public ItemSelectionDto(TValue value, string text)
        {
            Value = value;
            Text = text;
        }
        
        public ItemSelectionDto(TValue value, string text, string additional)
        {
            Value = value;
            Text = text;
            Additional = additional;
        }
    }
}
