using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace FabriqPro.Tests;

public class MaterialTests(ApplicationFactory factory) : IClassFixture<ApplicationFactory>, IAsyncLifetime
{
  private readonly HttpClient _client = factory.CreateClient();

  public async Task InitializeAsync()
  {
    var loginPayload = new
    {
      login = "+998900000001",
      password = "storagemaster"
    };

    var content = new StringContent(JsonConvert.SerializeObject(loginPayload), Encoding.UTF8, "application/json");

    var response = await _client.PostAsync("api/v1/users/login", content);

    var responseBody = await response.Content.ReadAsStringAsync();
    var token = JsonConvert.DeserializeObject<AccessTokenResponse>(responseBody)?.AccessToken;
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
  }

  public async Task DisposeAsync()
  {
    using var scope = factory.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<FabriqDbContext>();
    await context.Database.EnsureDeletedAsync();
  }

  [Fact]
  public async Task Getting_Material_Types_Returns_Ok()
  {
    var response = await _client.GetAsync("api/v1/materials/types");
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
  }
}