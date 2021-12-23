namespace Kombicim.Data.Models.Arduino.Dtos
{
    public class ProfilePreferenceDto
    {
        public int ProfileId { get; set; }
        public int? DayOfWeek { get; set; }
        public int? Hour { get; set; }
        public int? Minute { get; set; }
    }
}
