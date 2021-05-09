namespace AspNetCoreFactory.CQRS.Core.Domain
{
    public partial class Booking
    {
        public bool ShouldSerializeTraveler()
        {
            return false;
        }
        public bool ShouldSerializeSeat()
        {
            return false;
        }
        public bool ShouldSerializeFlight()
        {
            return false;
        }
    }
}
