
namespace NearestVehiclePositions
{
    internal class Positions
    {
        internal int PositionId { get; set; }
        internal string? VehicleRegistration { get; set; }
        internal double Latitude { get; set; }
        internal double Longitude { get; set; }
        internal ulong RecordedTimeUTC { get; set; }
    }
}
