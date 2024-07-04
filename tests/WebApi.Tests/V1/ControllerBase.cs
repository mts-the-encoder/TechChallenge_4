using Communication.Requests;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
using Xunit;

namespace WebApi.Tests.V1;

public class ControllerBase : IClassFixture<WebAppFactory<Program>>
{
    private readonly HttpClient _client;

    protected ControllerBase(WebAppFactory<Program> factory)
    {
        _client = factory.CreateClient();
        //ErrorMessages.Culture = CultureInfo.CurrentCulture;
    }

    protected async Task<HttpResponseMessage> PostRequest(string method, object body)
    {
        var jsonString = JsonConvert.SerializeObject(body);

        return await _client.PostAsync(method, new StringContent(jsonString, Encoding.UTF8, "application/json"));
    }

    protected async Task<HttpResponseMessage> PostRequest(string method, object body, string token = "")
    {
        AuthorizeRequest(token);

        var jsonString = JsonConvert.SerializeObject(body);

        return await _client.PostAsync(method,new StringContent(jsonString, Encoding.UTF8,"application/json"));
    }

    protected async Task<HttpResponseMessage> PutRequest(string method, object body, string token = "")
    {
        AuthorizeRequest(token);

        var jsonString = JsonConvert.SerializeObject(body);

        return await _client.PutAsync(method,new StringContent(jsonString,Encoding.UTF8,"application/json"));
    }

    protected async Task<string?> Login(string email, string password)
    {
        var request = new LoginRequest
        {
            Email = email,
            Password = password
        };

        var response = await PostRequest("login", request);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        return responseData.RootElement.GetProperty("token").GetString();
    }

    private void AuthorizeRequest(string token)
    {
        if (!string.IsNullOrWhiteSpace(token))
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
    }
}