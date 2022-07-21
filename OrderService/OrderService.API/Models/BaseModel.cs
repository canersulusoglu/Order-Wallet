﻿namespace OrderService.API.Models
{
    public class BaseModel
    {
        public int Id { get; set; }

        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedTime { get; set; } = DateTime.UtcNow;
    }
}
