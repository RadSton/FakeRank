namespace FakeRank.Helpers
{
    using System;
    using CommandSystem;
    using LabApi.Features.Permissions;
    using LabApi.Features.Wrappers;

    public static class LabApiPermissions
    {

        public static bool checkCommandSender(ICommandSender sender, string permission)
        {
            return PermissionExtensions.HasPermissions(sender, permission);
        }
    }
}