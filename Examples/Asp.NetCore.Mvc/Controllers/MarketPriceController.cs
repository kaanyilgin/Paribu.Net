using Microsoft.AspNetCore.Mvc;
using Paribu.Net.Contracts;

namespace Asp.NetCore.Mvc.Controllers;

[ApiController]
[Route("[controller]")]
public class MarketPriceController : ControllerBase
{
    private readonly IParibuClient _paribuClient;

    public MarketPriceController(IParibuClient paribuClient)
    {
        _paribuClient = paribuClient;
    }

    [HttpGet]
    public decimal Get()
    {
        return _paribuClient.GetMarketData("btc-tl").Data.MarketMatches.First().Price;
    }
}