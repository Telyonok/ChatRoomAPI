namespace ChatRoomAPI.Middleware
{
    public static class UseApiKeyExtension
    {
        public static IApplicationBuilder UseApiKey(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UseApiKey>();
        }
    }
}
