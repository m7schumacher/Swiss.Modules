using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Swiss.API.Finance
{
    public partial class Stock
    {
        public string Name { get; set; }
        public string Symbol { get; set; }

        public double Change { get; set; }
        public double ChangePercent { get; set; }

        public double High { get; set; }
        public double Low { get; set; }
    }

    public class StockHistory
    {
        public string Symbol { get; set; }
        public List<Closing> Prices { get; set; }

        public override string ToString()
        {
            return Symbol;
        }
    }

    public class Closing
    {
        public double Price { get; set; }
        public DateTime Date { get; set; }

        public override string ToString()
        {
            return Price.ToString();
        }
    }

    public class StockAPI
    {
        public enum Commodities
        {
            Corn,
            Wheat,
            Soy,
            Oil,
            Cattle,
            Pork
        };

        private string StockPerformanceURL = "http://finance.yahoo.com/webservice/v1/symbols/{0}/quote?format=xml&view=detail";
        private string SymbolURL = "http://d.yimg.com/autoc.finance.yahoo.com/autoc?query={0}&region=2&lang=en";
        private string StockHistoryURL = "https://www.quandl.com/api/v3/datasets/WIKI/{0}.xml?start_date={1}&end_date={2}&api_key={3}";
        private string CommodityURL = "https://www.quandl.com/api/v3/datasets/{0}.xml?start_date={1}&end_date={2}&api_key={3}";

        private string API_KEY;

        #region Private Methods

        private Stock ExtractInformation(XDocument doc)
        {
            var targetNode = doc.Element("list")
                                .Element("resources")
                                .Element("resource");

            var fields = targetNode.Elements("field")
                                   .Select(fld => new KeyValuePair<string, string>(fld.Attribute("name").Value, fld.Value))
                                   .ToDictionary();

            return new Stock()
            {
                Name = fields["name"],
                Symbol = fields["symbol"],
                Change = fields["change"].ToDouble(),
                ChangePercent = fields["chg_percent"].ToDouble(),
                High = fields["day_high"].ToDouble(),
                Low = fields["day_low"].ToDouble()
            };
        }

        private static Dictionary<string[], string> CommonCompanies = new Dictionary<string[], string>()
        {
            { new string[] { "microsoft" }, "MSFT" },
            { new string[] { "apple" }, "AAPL" },
            { new string[] { "google", "alphabet" }, "GOOG" },
        };

        private string GetSymbol(string companyName)
        {
            var symbol = string.Empty;

            var lucky = CommonCompanies.Keys.FirstOrDefault(terms => terms.AnyEqual(companyName));

            if (lucky != null)
            {
                symbol = CommonCompanies[lucky];
            }
            else
            {
                var fullURL = string.Format(SymbolURL, companyName);
                var resultingXML = InternetUtility.DownloadXML(fullURL);

                var target = resultingXML.Branches.FirstOrDefault(branch => branch.Children.Any(nd => nd.Value.EqualsIgnoreCase("nasdaq")));

                if (target != null)
                {
                    symbol = target.GetChildByName("symbol").Value;
                }
            }

            return symbol;
        }

        private StockHistory GetCommodityHistory(string code, string name)
        {
            var yesterday = DateTime.Now.AddDays(-1);
            var threeMonths = yesterday.AddMonths(-3);

            var yesterString = yesterday.ToString("yyyy-MM-dd");
            var monthString = threeMonths.ToString("yyyy-MM-dd");

            var fullURL = string.Format(CommodityURL, code, monthString, yesterString, API_KEY);

            var sheet = InternetUtility.DownloadXML(fullURL);

            return GenerateHistory(sheet, name, 1);
        }

        private StockHistory GetStockHistory(string symbol)
        {
            var yesterday = DateTime.Now.AddDays(-1);
            var threeMonths = yesterday.AddMonths(-3);

            var yesterString = yesterday.ToString("yyyy-MM-dd");
            var monthString = threeMonths.ToString("yyyy-MM-dd");

            string fullURL = string.Format(StockHistoryURL, symbol, monthString, yesterString, API_KEY);

            var sheet = InternetUtility.DownloadXML(fullURL);

            return GenerateHistory(sheet, symbol);
        }

        private StockHistory GenerateHistory(XmlSheet sheet, string symbol, int target = 4)
        {
            var datums = sheet.NodesByName("datum").ToList();
            var arrays = datums.Where(nd => nd.Attributes["type"].Equals("array")).ToList();

            var closings = arrays.Select(array => new Closing()
            {
                Date = array.Children.ElementAt(0).Value.ToDate(),
                Price = array.Children.ElementAt(target).Value.ToDouble()
            }).ToList();

            return new StockHistory()
            {
                Symbol = symbol.ToUpper(),
                Prices = closings
            };
        }

        #endregion

        public StockAPI(string apiKey = "")
        {
            API_KEY = apiKey;
        }

        public Stock GetStockPerformanceByName(string companyName)
        {
            var symbol = GetSymbol(companyName);
            return GetStockInfo(symbol);
        }

        public Stock GetStockPerformanceBySymbol(string symbol)
        {
            return GetStockInfo(symbol);
        }

        private Stock GetStockInfo(string symbol)
        {
            string fullURL = string.Format(StockPerformanceURL, symbol);
            XDocument result = InternetUtility.DownloadXML(fullURL).Document;
            return ExtractInformation(result);
        }

        public StockHistory GetCommodityHistory(Commodities commodity)
        {
            string target = string.Empty;

            switch(commodity)
            {
                case Commodities.Corn:
                    target = "CHRIS/CME_C1|CORN";
                    break;
                case Commodities.Wheat:
                    target = "CHRIS/CME_W1|WHEAT";
                    break;
                case Commodities.Soy:
                    target = "CHRIS/CME_S1|SOY BEANS";
                    break;
                case Commodities.Oil:
                    target = "CHRIS/CME_CL1|OIL";
                    break;
                case Commodities.Cattle:
                    target = "CHRIS/CME_LC1|CATTLE";
                    break;
            }

            StockHistory result = null;

            if(!string.IsNullOrEmpty(target))
            {
                string[] bits = target.Split("|");

                result = GetCommodityHistory(bits[0], bits[1]);
            }

            return result;
        }
    }
}
