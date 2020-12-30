using System;
using System.Linq;

namespace Mastermind
{
	class Game
	{
		// define "global" variables and constants
		private static Random random = new Random();
		private static char[] separators = {' ', '\t'};

		// game instructions are stored as constant string
		private const string instructions = "\nMastermind: Guess the secret code in 10 or fewer tries."
			+ "\n\nHints:\n - The code contains 4 digits that are each between numbers 1 and 6, inclusive."
			+ "\n - The following indicates how close a guess is to the correct answer."
			+ "\n\tA plus (+) indicates a correct digit in the correct position."
			+ "\n\tA minus (-) indicates a correctly guessed digits in the wrong position."
			+ "\n\tNothing is printed to indicate a guessed digit is completely incorrect."
			+ "\n - Example guess: 1234\n - Example response: |--++|\n";

		static void Main(string[] args)
		{
			// allows user to play multiple times without exiting program
			bool play = true;
			while (play)
			{
				// clear terminal
				Console.Clear();

				// generate a random code for the user to guess
				string[] secretCode = new string[4];
				for (int i = 0; i < 4; i++)
					secretCode[i] = random.Next(1, 7).ToString();

				// print the game's instructions and begin
				Console.WriteLine(instructions);
				PlayGame(secretCode);

				// ask if user wishes to play again
				play = ConfirmReplay();
			}
		}

		private static void PlayGame(string[] code)
		{
			// player is limited to 10 guesses
			for (int i = 1; i <= 10; i++)
			{
				// retrieve and convert the guess into an array of characters
				Console.Write("Guess {0}: ", i);
				string input = String.Join(" ", Console.ReadLine().ToCharArray());
				string[] guess = input.Split(separators, StringSplitOptions.RemoveEmptyEntries);

				// indicate if the player wins the game.
				if (TestGuess(guess, code))
				{
					Console.WriteLine("You Win!\n");
					return;
				}
			}
			// indicate if the player loses the game and reveal the code.
			Console.WriteLine("You Lose!\nThe code was: {0}\n", String.Join("", code));
		}

		private static bool TestGuess(string[] guess, string[] code)
		{
			// get a deep copy of the code array
			string[] result = code.ToArray();

			// first check for exact matches in the guess and secret code
			for (int i = 0; i < 4; i++)
			{
				// one-to-one matching between array elements
				if (guess[i].Equals(result[i]))
				{
					guess[i] = " ";
					result[i] = "+";
				}
			}

			// then check for matching elements with different indices
			for (int j = 0; j < 4; j++) {
				if (guess.Contains(result[j]))
				{
					guess[Array.IndexOf(guess, result[j])] = " ";
					result[j] = "-";
				}
				else if (!result[j].Equals("+"))
					result[j] = "";
			}

			// print a response to the player and return the result
			string response = String.Join("", result.OrderBy(x => random.Next()));
			Console.WriteLine("Response: |{0}|\n", response);
			if (response.Equals("++++")) return true;
			return false;
		}

		private static bool ConfirmReplay()
		{
			// wait for specific input
			while (true)
			{
				// 'y' or 'yes' indicates play again where 'n' or 'no' indicates stop playing
				Console.Write("Play again (y/n)? ");
				string input = Console.ReadLine().ToLower();
				if (input.Equals("y") || input.Equals("yes"))
					return true;
				else if (input.Equals("n") || input.Equals("no"))
					return false;
			}
		}
	}
}
