
namespace io.radston12.fakerank.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using CommandSystem;


    using Exiled.API.Features;
    using Exiled.API.Features.Pools;
    using Exiled.API.Features.Pickups;

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
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            StringBuilder stringBuilder = StringBuilderPool.Pool.Get();

            stringBuilder.AppendLine("Available commands: ");
            stringBuilder.AppendLine("Note: \"<>\"-Arguments are required, while \"[]\"-Arguments are optional!");
            stringBuilder.AppendLine("- FAKERANK CLEAR [RA-User-ID] - Nukes any badge.");
            stringBuilder.AppendLine("- FAKERANK SELF <COLOR> <FAKEBADGE> - Sets a fakerank for the executor.");
            stringBuilder.AppendLine("- FAKERANK SET <RA-User-ID> <COLOR> <FAKEBADGE> - Sets a fakerank.");

            response = StringBuilderPool.Pool.ToStringReturn(stringBuilder);
            return false;
        }
        
    }

}