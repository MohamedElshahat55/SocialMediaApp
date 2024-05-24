namespace SocialMediaApp.Extensions
{
    public static class DateTimeExtensions
    {
        public static int CalculateAge(this DateOnly birthDate)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var age = today.Year - birthDate.Year;  // 2024 - 1990 = 34 years
            if (birthDate > today.AddYears(-age)) // 2024 - (-34) = 2058
            {
                age--;
            }
            return age;
        }
    }
}
