using Discord;
using Discord.WebSocket;

namespace YT_DISCORD_DEMO;

internal class Program {
	private readonly DiscordSocketClient client;
	private string token;

	public Program() {
		client = new DiscordSocketClient();
		client.MessageReceived += MessageHandler;
		client.Log += Log;
		token = LoadToken();
	}

	private string LoadToken() {
		try {
			string tokenFilePath = "Token.txt";
			if (File.Exists(tokenFilePath)) {
				return File.ReadAllText(tokenFilePath).Trim();
			}
			else {
				Console.WriteLine("Token.txt file not found!");
				return null;
			}
		}
		catch (Exception ex) {
			Console.WriteLine($"Error loading token: {ex.Message}");
			return null;
		}
	}

	public async Task StartBotAsync() {
		await client.LoginAsync(TokenType.Bot, token);
		await this.client.StartAsync();
		await Task.Delay(-1);
	}

	private async Task MessageHandler(SocketMessage message) {
		if (message.Author.IsBot)
			return;

		await ReplaySubject(message, "C# response works!");
	}

	private static Task Log(LogMessage msg) {
		Console.WriteLine(msg.ToString());
		return Task.CompletedTask;
	}

	private async Task ReplaySubject(SocketMessage message, string response) {
		await message.Channel.SendMessageAsync(response);
	}

	public static async Task Main(string[] args) {
		var myBot = new Program();
		await myBot.StartBotAsync();
	}
}
