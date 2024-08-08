using PokemonApi.Services;

namespace PokemonApi;

public class Trainer
{
    public string Name { get; set; }
    public HashSet<Pokemon> AmenitasPc { get; private set; } 

    public Dictionary<int, Pokemon> PokemonList { get; private set; }

    public Trainer(string name)
    {
        Name = name;
        AmenitasPc = new HashSet<Pokemon>();
        PokemonList = new Dictionary<int, Pokemon>();
    }

    public async Task AddPokemon(string pokemonName)
    {
        try
        {
            Pokemon pokemon = await PokemonService.GetPokemonAsync(pokemonName);
            
            if (PokemonList.Count == 6)
            {
                AmenitasPc.Add(pokemon);
                Console.WriteLine($"... {pokemon.Name} Added to Amenintas PC ...");
            }
            else
            {
                PokemonList.Add(PokemonList.Count + 1, pokemon);
                Console.WriteLine($"... {pokemon.Name} Added to Pokemon Team at Slot {PokemonList.Count}! ...");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("... Pokemon not Found ...");
            throw new InvalidPokemonException();
        }
    }

}