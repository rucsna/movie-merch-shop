import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import MovieCard from "../Components/MovieCard";
import MerchandiseList from "../Components/MerchandiseList";

const Products = () => {
  const [searchTerm, setSearchTerm] = useState("");
  const [movies, setMovies] = useState([]);
  const [selectedMovie, setSelectedMovie] = useState(null);
  const navigate = useNavigate();

  const handleSearchChange = async (e) => {
    const term = e.target.value;
    setSearchTerm(term);

    if (term.trim() === "") {
      setMovies([]);
      setSelectedMovie(null);
      return;
    }

    try {
      const response = await fetch(`/api/Movie/GetMoviesByTitle/${term}`);
      const data = await response.json();

      console.log(data);

      if (data.length > 0) {
        setMovies(data);
      } else {
        console.log("else");

        if (data.Search) {
          setMovies(data.Search);
        } else {
          setMovies([]);
        }
      }

      console.log(movies[0]);
      setSelectedMovie(null);
    } catch (error) {
      console.error("Error fetching movies:", error);
    }
  };

  const handleMovieSelect = (movie) => {
    setSelectedMovie(movie);
    setMovies([]);
  };

  return (
    <div className="products-page">
      <div className="search-bar-container">
        <div className="search-bar">
          <input
            type="text"
            placeholder="Search movies..."
            value={searchTerm}
            onChange={handleSearchChange}
            list="movies-list"
            style={{ width: "400px" }}
          />
          <datalist id="movies-list" style={{ width: "400px" }}>
            {movies.map((movie) => (
              <option key={movie.id} value={movie.title} />
            ))}
          </datalist>
        </div>
      </div>
      {selectedMovie && (
        <div>
          <MovieCard
            title={selectedMovie.title}
            year={selectedMovie.year}
            posterUrl={selectedMovie.imdbID}
          />
          <MerchandiseList movie={selectedMovie} />
        </div>
      )}
      <div className="movie-cards">
        {movies.map((movie) => (
          <MovieCard
            key={movie.id}
            title={movie.title}
            year={movie.year}
            imdbID={movie.imdbID}
            onClick={() => handleMovieSelect(movie)}
          />
        ))}
      </div>
    </div>
  );
};

export default Products;
