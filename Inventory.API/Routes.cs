namespace Inventory.API;

public static class Routes
{
    public const string BasePath = "/api";

    public static class Users
    {
        public const string PostLogin = BasePath + "/Users/Login";
        public const string PostUser = BasePath + "/{SlugTenant}/Users";
    }

    public static class Organizations
    {
        public const string PostOrganization = BasePath + "/Organizations";
    }

    public static class Products
    {
        public const string PostProduct = BasePath + "/{SlugTenant}/Products";
        public const string PutProduct = BasePath + "/{SlugTenant}/Products/{id}";
        public const string DeleteProduct = BasePath + "/{SlugTenant}/Products/{id}";
        public const string GetProduct = BasePath + "/{SlugTenant}/Products/{id}";
        public const string GetProducts = BasePath + "/{SlugTenant}/Products";
    }
}