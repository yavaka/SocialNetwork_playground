﻿namespace SocialMedia.Data.Models
{
    public class Friendship
    {
        public string RequesterId { get; set; }

        public string AddresseeId { get; set; }

        public Status Status { get; set; }

        public virtual User Addressee { get; set; }

        public virtual User Requester { get; set; }
    }

    public enum Status
    {
        Pending,
        Accepted
    }
}
