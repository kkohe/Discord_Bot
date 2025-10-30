
using Discord_Bot.Commands;
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

            // <Leitura do arquivo de configuração JSON, aonde está o Token/Prefixo.>

            var jsonReader = new jsonReader();
            await jsonReader.ReadJson();

            // <Configurações do Discord.>

            var discordConfig = new DiscordConfiguration {

                Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents,
                Token = jsonReader.token,
                TokenType = TokenType.Bot,
                AutoReconnect = true

            };

            // <Inicializa o cliente do Discord.>

            Client = new DiscordClient(discordConfig);

            // <Evento disparado quando o bot estiver pronto.>

            Client.Ready += Client_Ready;

            // <Inicializa o CommandsNext/Configura os comandos.>

            var commandsConfig = new CommandsNextConfiguration() {

                StringPrefixes = new string[] { jsonReader.prefix },
                EnableMentionPrefix = true,
                EnableDms = false,
                EnableDefaultHelp = false
            };

            Commands = Client.UseCommandsNext(commandsConfig);

            // <Registro de comandso.>

            Commands.RegisterCommands<KickCommand>();
            Commands.RegisterCommands<TestCommands>();
            Commands.RegisterCommands<GifCommand>();

            // <Comentado para o bot não floodar ao enviar a palavra-chave do gif.>

            //Client.MessageCreated += async (s, e) =>
            //{

            //    if (e.Author.IsBot) return;

            //    var nome = e.Message.Content.Trim().ToLower();

            //    var gifUrl = GifCommand.ObterGif(nome);

            //    if (gifUrl != null) {
            //        await e.Message.RespondAsync(gifUrl);
            //    }
            //};

            // <Conecta o bot ao Discord.>

            await Client.ConnectAsync();
            await Task.Delay(-1);


        }
        private static Task Client_Ready(DiscordClient sender, DSharpPlus.EventArgs.ReadyEventArgs args) {

            return Task.CompletedTask;

        }
    }
}
