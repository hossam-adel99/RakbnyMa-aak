﻿namespace RakbnyMa_aak.DTOs.TripDTOs.ResponseDTOs
{
    public class TripCoordinatesResponseDto
    {
        public string FromCity { get; set; }
        public double FromLatitude { get; set; }
        public double FromLongitude { get; set; }

        public string ToCity { get; set; }
        public double ToLatitude { get; set; }
        public double ToLongitude { get; set; }
    }
}
