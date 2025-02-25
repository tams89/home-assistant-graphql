namespace home_assistant_graphql.Types
{
    using HotChocolate.Types;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class Query
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _config;

        public Query(IHttpClientFactory clientFactory, IConfiguration config)
        {
            _clientFactory = clientFactory;
            _config = config;
        }

        public async Task<string> GetEntityState(string entityId)
        {

            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_config["token"]}");

            var response = await client.GetAsync($"http://homeassistant.lan:8123/api/states/{entityId}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }

    public class QueryType : ObjectType<Query>
    {
        protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
        {
            descriptor.Field(q => q.GetEntityState(default))
                      .Argument("entityId", a => a.Type<NonNullType<StringType>>())
                      .Type<StringType>();
        }
    }
}
