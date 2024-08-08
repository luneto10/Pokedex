namespace PokemonApi;

public class InvalidPokemonException : Exception
{
    public InvalidPokemonException()
    {
    }

    public InvalidPokemonException(string message)
        : base(message)
    {
    }

    public InvalidPokemonException(string message, Exception inner)
        : base(message, inner)
    {
    }
}