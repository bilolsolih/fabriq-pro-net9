﻿using System.Net;
using System.Net.Http.Headers;
using System.Text;
using FabriqPro.Features.Products.DTOs;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Newtonsoft.Json;
using Xunit.Sdk;

namespace FabriqPro.Tests;

public class MaterialTypeTests(ApplicationFactory factory) : IClassFixture<ApplicationFactory>, IAsyncLifetime
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

  public Task DisposeAsync()
  {
    return Task.CompletedTask;
  }

  [Fact]
  public async Task Getting_Material_Types_Returns_Ok()
  {
    var response = await _client.GetAsync("api/v1/materials/types");
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    var contents = await response.Content.ReadAsStringAsync();
    var materialTypes = JsonConvert.DeserializeObject<List<MaterialTypeListDto>>(contents);
  }
  
  
}