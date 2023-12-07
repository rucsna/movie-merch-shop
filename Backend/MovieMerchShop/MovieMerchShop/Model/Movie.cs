using System;
using System.Collections.Generic;

namespace MovieMerchShop.Model
{
    public class Movie
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Year { get; init; }
        public string Poster { get; set; }
        public string Plot { get; set; }
        public ICollection<string> _actors;
        public ICollection<string> _genres;
        public ICollection<MerchItem> _merch;

        public Movie()
        {
        }

        public Movie(string year, string title)
        {
            Title = title;
            Year = year;
            Id = Guid.NewGuid();
            _actors = new List<string>();
            _merch = new List<MerchItem>();
            _genres = new List<string>();
        }
        
        public void AddActor(string actor)
        {
            _actors.Add(actor);
        }

        public IEnumerable<string> GetActors()
        {
            return _actors;
        }
        
        public void AddGenre(string genre)
        {
            _genres.Add(genre);
        }

        public IEnumerable<string> GetGenres()
        {
            return _genres;
        }
        
        public void AddMerchItem(MerchItem merchItem)
        {
            _merch.Add(merchItem);
        }

        public IEnumerable<MerchItem> GetMerchItems()
        {
            return _merch;
        }
    }
}