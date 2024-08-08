using Newtonsoft.Json;
using PokemonApi.Services;

namespace PokemonApi.Utils;

public class ConsoleUtils
{
    enum Choices
    {
        SeePokemonStats = 1,
        AddPokemon = 2,
        SeeTrainerList,
        SeeAmanitasPC,
        Exit,
    }
    public static void PrintHeader()
    {
        Console.Clear();
        Console.WriteLine("**************************************************");
        Console.WriteLine("*                                                *");
        Console.WriteLine("*                 Pokedex Console                *");
        Console.WriteLine("*          Your Ultimate Pokemon Database        *");
        Console.WriteLine("*                                                *");
        Console.WriteLine("**************************************************");
        Console.WriteLine();
        Console.WriteLine("Welcome to the Pokedex Application!");
        Console.WriteLine("Discover and explore information about your favorite Pokemon.");
        Console.WriteLine("Developed by Luciano Carvalho");
        Console.WriteLine();
    }
    
    public static void WelcomeHeader(string trainerName)
    {
        Console.Clear();
        string headerLine = new string('*', 50);
        string welcomeMessage = $"*   Welcome Trainer {trainerName}   *";

        int padding = (headerLine.Length - welcomeMessage.Length) / 2;
        string paddedMessage = welcomeMessage.PadLeft(welcomeMessage.Length + padding).PadRight(headerLine.Length);

        Console.WriteLine(headerLine);
        Console.WriteLine(paddedMessage);
        Console.WriteLine(headerLine);
        Console.WriteLine();
    }

    public static void StartApplication()
    {
        PrintHeader();
        Console.WriteLine("Name your trainer");
        string name = Console.ReadLine()!;
        Trainer trainer = new Trainer(name);
        bool end = false;
        while (!end)
        {
            WelcomeHeader(trainer.Name);
            Console.WriteLine("1 - See Pokemon Stats");
            Console.WriteLine("2 - Add a Pokemon");
            Console.WriteLine($"3 - See {trainer.Name}'s Pokemon List");
            Console.WriteLine($"4 - See {trainer.Name}'s Amenitas PC");
            Console.WriteLine("5 - Exit");

            string userInput = Console.ReadLine()!;
            bool parsed = int.TryParse(userInput, out int choice);

            if (parsed == false)
            {
                continue;
            }

            switch ((Choices) choice)
            {
                case Choices.SeePokemonStats:
                    Console.Clear();
                    SeePokemonStats().Wait();
                    break;
                case Choices.AddPokemon:
                    Console.Clear();
                    AddPokemonToTrainer(trainer).Wait();
                    break;
                case Choices.SeeTrainerList:
                    GetPokemonList(trainer);
                    break;
                case Choices.SeeAmanitasPC:
                    GetAmanitasPC(trainer);
                    break;
                case Choices.Exit:
                    end = true;
                    Console.WriteLine("... Finishing Program ...");
                    Console.ReadLine();
                    break;
                default:
                    Console.WriteLine("... Please, selected a valid option ...");
                    break;
            }
        }
        
    }

    private static async Task SeePokemonStats()
    {
        bool pokemonFound = false;
        while (!pokemonFound)
        {
            Console.WriteLine("Name the pokemon: ");
            try
            {

                var pokemonName = Console.ReadLine();
                Pokemon pokemon = await PokemonService.GetPokemonAsync(pokemonName);
                Console.Clear();
                Console.WriteLine(pokemon);
                Console.ReadLine();
                Console.Clear();
                pokemonFound = true;
            }
            catch (InvalidPokemonException e)
            {
                Console.Clear();
                Console.WriteLine("... Pokemon Not Found ...");
                Console.WriteLine("1 - To Exit");
                string input = Console.ReadLine()!;
                bool parsed = int.TryParse(input, out int choice);
                if (parsed && choice == 1)
                {
                    Console.Clear();
                    break;
                }
                Console.Clear();
            }
        }
        
    }
    
    private static async Task AddPokemonToTrainer(Trainer trainer)
    {
        bool pokemonFound = false;
        while (!pokemonFound)
        {
            Console.WriteLine("Name a pokemon to add: ");
            try
            {
                var pokemonName = Console.ReadLine();
                await trainer.AddPokemon(pokemonName);
                Console.ReadLine();
                Console.Clear();
                pokemonFound = true;
            }
            catch (InvalidPokemonException e)
            {
                Console.WriteLine("1 - To Exit");
                string input = Console.ReadLine()!;
                bool parsed = int.TryParse(input, out int choice);
                if (parsed && choice == 1)
                {
                    Console.Clear();
                    break;
                }
                Console.Clear();
            }
        }
    }

    private static void GetPokemonList(Trainer trainer)
    {
        Console.Clear();
        if (trainer.PokemonList.Count == 0)
        {
            Console.WriteLine("... Your Pokemon List is Empty ...");
            Console.ReadLine();
            return;
        }
        
        foreach (Pokemon pokemon in trainer.PokemonList.Values)
        {
            Console.WriteLine(pokemon);
            Console.Write("> ");
            Console.ReadLine();
        }
        
    }
    
    enum AmanitasPC
    {
         seePokemons = 1,
         changePokemon,
         exit
    }
    private static void GetAmanitasPC(Trainer trainer)
    {
        Console.Clear();
        
        if (trainer.AmenitasPc.Count == 0)
        {
            Console.WriteLine("... Amenitas PC is Empty ...");
            Console.ReadLine();
            return;
        }
        
        bool exit = false;
        
        while (!exit)
        {
            Console.WriteLine("1 - See all Pokemon");
            Console.WriteLine("2 - Change Pokemon");
            Console.WriteLine("3 - Exit");
            
            string input = Console.ReadLine()!;
            bool parsed = int.TryParse(input, out int choice);
            if (parsed && choice == 1)
            {
                Console.Clear();
            }
            
            switch ((AmanitasPC) choice)
            { 
                case AmanitasPC.seePokemons:
                
                    foreach (Pokemon pokemon in trainer.AmenitasPc)
                    {
                        Console.WriteLine(pokemon);
                        Console.Write("> ");
                        Console.ReadLine();
                    }
                    break;
                case AmanitasPC.changePokemon:
                    Console.Clear();
                    ChangePokemonAmanitas(trainer);
                    break;
                case AmanitasPC.exit:
                    exit = true;
                    break;
                default:
                    break;
                   
            }
            
            Console.Clear();
        }
        
        
        
    }

    private static void ChangePokemonAmanitas(Trainer trainer)
    {
        
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Name of the Pokemon in Amenitas");
            string input = Console.ReadLine()!;

            Pokemon pokemonIn = trainer.AmenitasPc.FirstOrDefault(pokemon => pokemon.Name == input, null);
            if (pokemonIn == null)
            {
                Console.Clear();
                Console.WriteLine("... Pokemon not in Amenitas ...");
                Console.ReadLine();
                continue;
            }

            int slot = -1;
            bool parsed = false;
            bool found = false;
            Pokemon pokemonOut = new Pokemon();
            while (!(parsed && found))
            {
                Console.Clear();
                Console.WriteLine("Slot of the Pokemon to Trade");
                string toParse = Console.ReadLine()!;
                parsed = int.TryParse(toParse, out slot);
                
                found = trainer.PokemonList.TryGetValue(slot, out pokemonOut);
                if (!found)
                {
                    Console.Clear();
                    Console.WriteLine("... This slot is empty ...");
                    Console.ReadLine();
                }
                
            }
            trainer.AmenitasPc.Remove(pokemonIn);
            trainer.PokemonList[slot] = pokemonIn;
            trainer.AmenitasPc.Add(pokemonOut);
            Console.WriteLine($"... {pokemonOut.Name} went to Amenitas ...");
            Console.WriteLine($"... {pokemonIn.Name} is now in your team ...");
            Console.ReadLine();
            break;

        }
    }
}