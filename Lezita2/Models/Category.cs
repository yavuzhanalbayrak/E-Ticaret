﻿namespace Lezita2.Models
{
    public class Category
    {
        public int Id { get; set; }
        public static int Count { get; private set; } = 0;
        public string? Name { get; set; }
        public string? Image { get; set; }
        public string? BackGroundImage { get; set; }
        public Category()
        {
            Id = ++Count;
        }
    }
}
