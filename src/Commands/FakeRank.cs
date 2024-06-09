
namespace io.radston12.fakerank.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using CommandSystem;


    using Exiled.API.Features;
    using Exiled.API.Features.Pools;
    using Exiled.API.Features.Pickups;
    using Exiled.Permissions.Extensions;

    using io.radston12.fakerank.Extensions;
    using io.radston12.fakerank.Commands.FakeRankChildren;

    /// <summary>
    /// FakeRank command via RemoteAdmin
    /// </summary>
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class FakeRank : ParentCommand
    {

        public FakeRank() => LoadGeneratedCommands();

        public override string Command { get; } = "fakerank";
        public override string[] Aliases { get; } = new[] { "frank", "fakebadge", "fbadge" };
        public override string Description { get; } = "Sets a fake rank for players.";


        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new Clear());
            RegisterCommand(new Self());
            RegisterCommand(new Set());
            RegisterCommand(new Ban());
            RegisterCommand(new Pardon());
            RegisterCommand(new Reload());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);


            StringBuilder stringBuilder = StringBuilderPool.Pool.Get();

            bool self = Permissions.CheckPermission(player, "fakerank.self");
            bool set = Permissions.CheckPermission(player, "fakerank.set");
            bool all = Permissions.CheckPermission(player, "fakerank.all");

            if (!self && !set && !all)
            {
                stringBuilder.AppendLine("You do not have enough permissions to execute this command");

                response = StringBuilderPool.Pool.ToStringReturn(stringBuilder);
                return false;
            }

            stringBuilder.AppendLine("Available commands: ");
            stringBuilder.AppendLine("Note: \"<>\"-Arguments are required, while \"[]\"-Arguments are optional!");

            if (!all)
            {
                if (self && !set)
                {
                    stringBuilder.AppendLine("- FAKERANK CLEAR - Removes your badge.");
                    stringBuilder.AppendLine("- FAKERANK SELF <COLOR> <BADGENAME> - Sets your fakerank.");
                    response = StringBuilderPool.Pool.ToStringReturn(stringBuilder);
                    return false;
                }
                else if (!self && set)
                {
                    stringBuilder.AppendLine("- FAKERANK CLEAR <RA-User-ID> - Nukes badge of player.");
                    stringBuilder.AppendLine("- FAKERANK SET <RA-User-ID> <COLOR> <FAKEBADGE> - Sets a fakerank for a player.");
                    response = StringBuilderPool.Pool.ToStringReturn(stringBuilder);
                    return false;
                }
            }

            if (all)
                stringBuilder.AppendLine("Note: -> You can execute every fakerank command!");
            stringBuilder.AppendLine("- FAKERANK CLEAR [RA-User-ID] - Nukes any badge.");
            stringBuilder.AppendLine("- FAKERANK SELF <COLOR> <FAKEBADGE> - Sets a fakerank for the executor.");
            stringBuilder.AppendLine("- FAKERANK SET <RA-User-ID> <COLOR> <FAKEBADGE> - Sets a fakerank.");
            if (all)
            {
                stringBuilder.AppendLine("- FAKERANK RELOAD - Reloads data changed from the custom config.");
                stringBuilder.AppendLine("- FAKERANK BAN <RA-User-ID> - Bans someone from using .setrank in player console.");
                stringBuilder.AppendLine("- FAKERANK PARDON <RA-User-ID> - Pardons someone from using .setrank in player console.");
            }

            response = StringBuilderPool.Pool.ToStringReturn(stringBuilder);
            return false;
        }

    }

}