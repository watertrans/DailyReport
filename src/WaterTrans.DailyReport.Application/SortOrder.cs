using System.Collections.Generic;

namespace WaterTrans.DailyReport.Application
{
    /// <summary>
    /// 並び順の指定
    /// </summary>
    public class SortOrder : List<SortOrderItem>
    {
        /// <summary>
        /// 並び順の指定文字列を解析します。
        /// </summary>
        /// <param name="sort">並び順の指定文字列を指定します。</param>
        /// <returns><see cref="SortOrder"/></returns>
        public static SortOrder Parse(string sort)
        {
            var result = new SortOrder();

            if (string.IsNullOrEmpty(sort))
            {
                return result;
            }

            foreach (string sortItem in sort.Split(','))
            {
                var item = new SortOrderItem();

                string sortItemTrim = sortItem.Trim();
                if (sortItemTrim.StartsWith("-"))
                {
                    item.Field = sortItemTrim.Substring(1).Trim();
                    item.SortType = SortType.DESC;
                }
                else
                {
                    item.Field = sortItemTrim;
                    item.SortType = SortType.ASC;
                }

                result.Add(item);
            }

            return result;
        }
    }
}
