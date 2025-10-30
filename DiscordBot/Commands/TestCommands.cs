using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Bot.Commands {
    public class TestCommands : BaseCommandModule {

        [Command("forsaken")]
        public async Task testCommand(CommandContext ctx) {
            await ctx.Channel.SendMessageAsync("ninguem vai jogar midsaken");
        }

        [Command("dandy")]
        public async Task midWorld(CommandContext ctx) {
            await ctx.Channel.SendMessageAsync("jogo ruim");
        }


    }
}
