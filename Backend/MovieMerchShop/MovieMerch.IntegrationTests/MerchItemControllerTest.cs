namespace MovieMerch.IntegrationTests;

public class MerchItemControllerTest : IDisposable
{
    private readonly MovieMerchFactory _factory;
    private readonly HttpClient _client;

    public MerchItemControllerTest()
    {
        _factory = new MovieMerchFactory();
        _client = _factory.CreateClient();
    }

    // [Fact]
    // public async Task GetAllItems_ReturnsOk_AndItemList()
    // {
    //     var response = await _client.GetAsync("/api/MerchItem");
    //     var responseString = await response.Content.ReadAsStringAsync();
    // }
    
    public void Dispose()
    {
        _factory.Dispose();
        _client.Dispose();
    }
}