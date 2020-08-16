namespace BuildingBlocks.Application
{
    public class ComboBoxDto<TValue> where TValue : struct
    {
        public TValue Value { get; set; }
        public string DisplayText { get; set; }

        public ComboBoxDto() { }
        public ComboBoxDto(TValue value, string displayText)
        {
            Value = value;
            DisplayText = displayText;
        }
    }
}