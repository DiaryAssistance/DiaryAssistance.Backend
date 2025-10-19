namespace DiaryAssistance.Api.Contracts;

public static class Routes
{
    public static class V1
    {
        public const string DefaultRoute = "/api/v1";

        public static class Auth
        {
            public const string BaseAuthRoute = $"{DefaultRoute}/auth";
            public const string Login = "login";
            public const string Register = "register";
            public const string Refresh = "refresh";
            public const string SignOut = "sign-out";
        }
    }
}