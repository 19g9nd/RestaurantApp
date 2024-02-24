using RestaurauntApp.Models.Other;

namespace RestaurauntApp.Services.Classes
{
    public static class EnumOrderStateHelper
    {
        public static EnumOrderState GetLastStatus()
        {
            return Enum.GetValues(typeof(EnumOrderState)).Cast<EnumOrderState>().Last();
        }
    }

}