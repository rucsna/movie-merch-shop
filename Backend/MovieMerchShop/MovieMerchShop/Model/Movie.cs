using System;
using System.Collections.Generic;

namespace MovieMerchShop.Model
{
    public class Movie
    {
        public Guid Id { get; }
        public string Title { get; set; }
        public int Year { get; init; }
        public string PosterLink { get; set; }
        public string Plot { get; set; }
        private ICollection<Actor> _actors;
        private ICollection<string> _genres;
        private ICollection<MerchItem> _merches;

        public Movie(int year, string title)
        {
            Title = title;
            Year = year;
            Id = Guid.NewGuid();
            _actors = new List<Actor>();
            _merches = new List<MerchItem>();
            _genres = new List<string>();
        }
        
        public void AddActor(Actor actor)
        {
            _actors.Add(actor);
        }

        public IEnumerable<Actor> GetActors()
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
            _merches.Add(merchItem);
        }

        public IEnumerable<MerchItem> GetMerchItems()
        {
            return _merches;
        }
    }
}