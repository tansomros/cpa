namespace BigLion.CPA.Application.Common.Security;

public static class Permissions
{
    public const string ClaimType = "permission";

    public static class Pharmacies
    {
        public const string View = "pharmacies.view";
        public const string Create = "pharmacies.create";
        public const string Update = "pharmacies.update";
    }
}
