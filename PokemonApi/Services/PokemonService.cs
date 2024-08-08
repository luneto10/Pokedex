using Newtonsoft.Json.Linq;

namespace PokemonApi.Services;

public class PokemonService
{
    private const string BaseUrl = "https://pokeapi.co/api/v2/";
    
    public static async Task<Pokemon> GetPokemonAsync(string name)
    {
        try
        {
            using HttpClient httpClient = new HttpClient();
            if (string.IsNullOrEmpty(name))
            {
                throw new InvalidPokemonException();
            }
            var response = await httpClient.GetStringAsync($"{BaseUrl}/pokemon/{name.ToLower()}");
            var pokemonJson = JObject.Parse(response);

            var pokemon = new Pokemon
            {
                Id = (int?)pokemonJson["id"] ?? 0,
                Name = (string?)pokemonJson["name"] ?? String.Empty,
                BaseExperience = (int?)pokemonJson["base_experience"] ?? 0,
                Height = (int?)pokemonJson["height"] ?? 0,
                Weight = (int?)pokemonJson["weight"] ?? 0,
                Types = pokemonJson["types"]?.Select(t => (string)t["type"]["name"]).ToArray() ?? Array.Empty<string>()
            };
            return pokemon;
        }
        catch (Exception e)
        {
            throw new InvalidPokemonException();
        }
    }
}