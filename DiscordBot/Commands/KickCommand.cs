using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Threading.Tasks;

namespace MyDiscordBot.Commands {
    public class KickCommand : BaseCommandModule {

        [Command("kick")]
        [Description("Expulsa um membro do servidor. Uso: !kick @usuario [motivo]")]
        [RequirePermissions(DSharpPlus.Permissions.KickMembers)]
        public async Task KickAsync(CommandContext ctx, DiscordMember member, [RemainingText] string reason = "Sem motivo.") {

            if (member == ctx.Member) {
                await ctx.RespondAsync(":x: Por que está tentando se expulsar?");
                return;
            }

            var bot = await ctx.Guild.GetMemberAsync(ctx.Client.CurrentUser.Id);
            if (bot.Hierarchy <= member.Hierarchy) {
                await ctx.RespondAsync(":x: Meu cargo é mais baixo que a desse membro!");
                return;
            }

            await member.RemoveAsync(reason);

            var embed = new DiscordEmbedBuilder {
                Title = "MEMBRO EXPULSO",
                Color = DiscordColor.Blue,
                Timestamp = DateTime.UtcNow
            };

            embed.AddField(" Usuário: ", $"{member.Username}#{member.Discriminator}", true);
            embed.AddField(" Expulso por: ", ctx.Member.Mention, true);
            embed.AddField(" Motivo: ", reason);

            await ctx.RespondAsync(embed);

        }
    }
}
