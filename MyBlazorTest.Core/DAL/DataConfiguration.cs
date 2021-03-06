﻿namespace MyBlazorTest.Core.DAL
{
    public class DataConfiguration
    {
        public bool Use { get; set; }
        public bool OrderAsc { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }

        public DataConfiguration(bool use = true, bool orderAsc = true, int pageNo = 0, int pageSize = 10)
        {
            Use = use;
            OrderAsc = orderAsc;
            PageNo = pageNo;
            PageSize = pageSize;
        }

        public override string ToString()
        {
            return $"Use: {Use}. OrderAsc: {OrderAsc}. PageNo: {PageNo}. PageSize: {PageSize}.";
        }
    }
}
