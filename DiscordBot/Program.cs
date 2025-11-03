
using DSharpPlus;
using DSharpPlus.CommandsNext;
using MyDiscordBot.Commands;
using SeuProjeto.Commands;
using System.Threading.Tasks;

namespace Discord_Bot {
    internal class Program {
        private static DiscordClient Client { get; set; }
        private static CommandsNextExtension Commands { get; set; }
        static async Task Main(string[] args) {

            var jsonReader = new jsonReader();
            await jsonReader.ReadJson();

            var discordConfig = new DiscordConfiguration {

                Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents,
                Token = jsonReader.token,
                TokenType = TokenType.Bot,
                AutoReconnect = true

            };

            Client = new DiscordClient(discordConfig);

            Client.Ready += Client_Ready;

            var commandsConfig = new CommandsNextConfiguration() {

                StringPrefixes = new string[] { jsonReader.prefix },
                EnableMentionPrefix = true,
                EnableDms = false,
                EnableDefaultHelp = false
            };

            Commands = Client.UseCommandsNext(commandsConfig);

            Commands.RegisterCommands<KickCommand>();
            Commands.RegisterCommands<GifCommand>();

            await Client.ConnectAsync();
            await Task.Delay(-1);

        }
        private static Task Client_Ready(DiscordClient sender, DSharpPlus.EventArgs.ReadyEventArgs args) {

            return Task.CompletedTask;

        }
    }
}