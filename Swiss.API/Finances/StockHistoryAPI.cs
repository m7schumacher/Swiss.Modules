//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Swiss.API
//{
    

//    public class StockHistoryAPI
//    {
//        public StockHistoryAPI()
//        {

//        }

//        public StockHistory GetCornHistory()
//        {
//            return GetCommodityHistory("CHRIS/CME_C1", "CORN");
//        }

//        public StockHistory GetWheatHistory()
//        {
//            return GetCommodityHistory("CHRIS/CME_W1", "WHEAT");
//        }

//        public StockHistory GetSoyBeanHistory()
//        {
//            return GetCommodityHistory("CHRIS/CME_S1", "SOY BEANS");
//        }

//        public StockHistory GetOilPrices()
//        {
//            return GetCommodityHistory("CHRIS/CME_CL1", "OIL");
//        }

//        public StockHistory GetCattleHistory()
//        {
//            return GetCommodityHistory("CHRIS/CME_LC1", "CATTLE");
//        }

//        public StockHistory GetCommodityHistory(string code, string name)
//        {
//            var yesterday = DateTime.Now.AddDays(-1);
//            var threeMonths = yesterday.AddMonths(-3);

//            var yesterString = yesterday.ToString("yyyy-MM-dd");
//            var monthString = threeMonths.ToString("yyyy-MM-dd");

//            var url = "https://www.quandl.com/api/v3/datasets/" + code + ".xml?start_date=" + monthString
//                + "&end_date=" + yesterString;

//            var sheet = InternetUtility.DownloadXML(url);

//            return GenerateHistory(sheet, name, 1);
//        }

//        public StockHistory GetStockHistory(string symbol)
//        {
//            var yesterday = DateTime.Now.AddDays(-1);
//            var threeMonths = yesterday.AddMonths(-3);

//            var yesterString = yesterday.ToString("yyyy-MM-dd");
//            var monthString = threeMonths.ToString("yyyy-MM-dd");

//            var url = "https://www.quandl.com/api/v3/datasets/WIKI/" + symbol + ".xml?start_date=" + monthString
//                + "&end_date=" + yesterString;

//            var sheet = Swiss.InternetUtility.DownloadXML(url);

//            return GenerateHistory(sheet, symbol);
//        }

//        private StockHistory GenerateHistory(XmlSheet sheet, string symbol, int target = 4)
//        {
//            var datums = sheet.NodesByName("datum").ToList();
//            var arrays = datums.Where(nd => nd.Attributes["type"].Equals("array")).ToList();

//            var closings = arrays.Select(array => new Closing()
//            {
//                Date = array.Children.ElementAt(0).Value.ToDate(),
//                Price = array.Children.ElementAt(target).Value.ToDouble()
//            }).ToList();

//            return new StockHistory()
//            {
//                Symbol = symbol.ToUpper(),
//                Prices = closings
//            };
//        }
//    }
//}
