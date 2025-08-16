namespace RakbnyMa_aak.CQRS.Queries.GetMessagesByTripId
{
    public class MessageDto
    {
        public int TripId { get; set; }
        public string SenderId { get; set; }
        public string SenderName { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
    }

}
