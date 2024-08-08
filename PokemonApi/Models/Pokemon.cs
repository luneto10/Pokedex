using System.Text;

namespace PokemonApi;

public class Pokemon
{
    public int Id { get; init; }
    private readonly string _name;

    public string Name
    {
        get => char.ToUpper(_name[0]) + _name[1..].ToLower();
        init => _name = value;
    }
    public int BaseExperience { get; init; }
    public int Height { get; init; }
    public int Weight { get; init; }
    public string[] Types { get; init; }
    

    public override string ToString()
    {
        string[] lines =
        {
            $"Name: {Name}",
            $"ID: {Id}",
            $"Base Experience: {BaseExperience}",
            $"Height: {Height}",
            $"Weight: {Weight}",
            $"Types: {string.Join(", ", Types)}"
        };

        // Determine the width of the box based on the longest line
        int width = lines.Max(line => line.Length) + 4; // Add padding

        string horizontalBorder = new string('*', width);
        StringBuilder boxedText = new StringBuilder();
        boxedText.AppendLine(horizontalBorder);

        foreach (string line in lines)
        {
            boxedText.AppendLine($"* {line.PadRight(width - 3)}*");
        }

        boxedText.AppendLine(horizontalBorder);
        return boxedText.ToString();
    }

    
}