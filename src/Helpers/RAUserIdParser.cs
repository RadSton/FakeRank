namespace FakeRank.Helpers
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    using Exiled.API.Features;

    public static class RAUserIdParser
    {

        public static Player getByCommandArgument(string argument)
        {
            if (!char.IsDigit(argument[0])) return null;

            int num;

            if (!int.TryParse(argument, out num)) return null;

            ReferenceHub refHub;

            if (!ReferenceHub.TryGetHub(num, out refHub)) return null;

            Player player;

            if(!Player.TryGet(refHub, out player)) return null;

            return player;
        }
    }
}