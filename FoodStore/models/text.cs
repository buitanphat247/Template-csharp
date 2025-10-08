public class Text
{
    public string Content { get; set; }
    public string Language { get; set; }
}

public class TextRepository
{
    public List<Text> Texts { get; set; }
}

public class TextService
{
    public List<Text> GetTexts()
    {
        return TextRepository.Texts;
    }
}
