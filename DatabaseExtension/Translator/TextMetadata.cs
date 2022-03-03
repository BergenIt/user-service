namespace DatabaseExtension.Translator
{
    public class TextMetadata
    {
        public string ClassName { get; init; }
        public string ElementName { get; init; }
        public string Text { get; init; }

        public UserText UserText => new() { Text = Text };
    }
}
