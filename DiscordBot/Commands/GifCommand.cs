using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace SeuProjeto.Commands {
    public class GifCommand : BaseCommandModule {
        private static readonly string _arquivoGifs = "gifs.json";
        private static Dictionary<string, string> _gifs = new Dictionary<string, string>();

        static GifCommand() {
            if (File.Exists(_arquivoGifs)) {
                try {
                    var texto = File.ReadAllText(_arquivoGifs);
                    var des = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(texto);
                    _gifs = des ?? new Dictionary<string, string>();
                }
                catch {
                    _gifs = new Dictionary<string, string>();
                }
            }
            else {
                _gifs = new Dictionary<string, string>();
            }
        }

        [Command("registrargif")]
        [Description("Registra um GIF associado a uma palavra-chave.")]
        public async Task RegistrarGifAsync(CommandContext ctx, string nome, string url) {
            if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(url)) {
                await ctx.RespondAsync("Uso: !registrargif <nome> <url>");
                return;
            }

            nome = nome.ToLowerInvariant();
            _gifs[nome] = url;

            await SalvarGifsAsync();

            await ctx.RespondAsync($"✅ GIF **{nome}** registrado com sucesso!");
        }

        [Command("gif")]
        [Description("Envia um GIF registrado: !gif <nome>")]
        public async Task EnviarGifAsync(CommandContext ctx, string nome) {
            nome = (nome ?? string.Empty).ToLowerInvariant();
            var url = ObterGif(nome);
            if (url != null)
                await ctx.RespondAsync(url);
            else
                await ctx.RespondAsync($"❌ Nenhum GIF registrado com o nome **{nome}**. ❌");
        }

        [Command("removegif")]
        [Description("Remove um GIF registrado.")]
        [RequirePermissions(DSharpPlus.Permissions.Administrator)]

        public async Task RemoverGifAsync(CommandContext ctx, string nome) {
            nome = (nome ?? string.Empty).ToLowerInvariant();

            if (_gifs.Remove(nome)) {
                await SalvarGifsAsync();
                await ctx.RespondAsync($"✅ O GIF **{nome}** foi removido com sucesso.");
            }
            else {
                await ctx.RespondAsync($"⚠️ Nenhum GIF encontrado com o nome **{nome}**. ⚠️");
            }
        }

        public static string ObterGif(string nome) {
            if (nome == null) return null;
            nome = nome.ToLowerInvariant();
            string url;
            if (_gifs.TryGetValue(nome, out url))
                return url;
            return null;
        }

        private static Task SalvarGifsAsync() {
            
            var json = System.Text.Json.JsonSerializer.Serialize(_gifs, new JsonSerializerOptions { WriteIndented = true });

            return Task.Run(() => {
                File.WriteAllText(_arquivoGifs, json);
            });
        }
    }
}
