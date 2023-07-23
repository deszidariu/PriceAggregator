namespace PriceAggregator.Api.Helpers
{
    public static class DateTimeExtension
    {
        public static string ConvertFromDateToUnixTimeSecondsString(this DateTime dateTime)
        {
            return new DateTimeOffset(dateTime).ToUnixTimeSeconds().ToString();
        }

        public static string ConvertFromDateToUnixTimeMillisecondsString(this DateTime dateTime)
        {
            return new DateTimeOffset(dateTime).ToUnixTimeMilliseconds().ToString();
        }

        public static DateTime SetDateTimeWithZeroMinutesAndSeconds(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year,
                                dateTime.Month,
                                dateTime.Day,
                                dateTime.Hour,
                                0,
                                0);
        }
    }
}
