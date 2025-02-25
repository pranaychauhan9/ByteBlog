﻿namespace PranayChauhanProjectAPI.Models.Domain
{
    public class Category
    {

        public  Guid Id { get; set; }
        public string Name { get; set; }
        public string UrlHandle { get; set; }

        public ICollection<BlogsPost> BlogPosts { get; set; }
    }
}
