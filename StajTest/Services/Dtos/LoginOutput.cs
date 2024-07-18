using Newtonsoft.Json;

namespace StajTest.Services.Dtos
{
    public class LoginOutput
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string UserName { get; set; }

        public string Token { get; set; }

        public string DefaultPage { get; set; }

        [JsonProperty("isAuth", DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool isAuth { get; set; }

        public List<string> Roles { get; set; } = new List<string>();

        public List<LoginPermission> Permissions { get; set; } = new List<LoginPermission>();
    }

    public class LoginPermission
    {
        public string Name { get; set; }
        public string RoleName { get; set; }
        [JsonProperty("Granted", DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool Granted { get; set; }
    }
}
