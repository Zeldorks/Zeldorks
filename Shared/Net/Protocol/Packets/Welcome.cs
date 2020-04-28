namespace NetGameShared.Net.Protocol.Packets
{
    // Sent to a client when it connects
    public class Welcome
    {
        // The ID that the client has been assigned
        public int ClientId { get; set; }
    }
}
