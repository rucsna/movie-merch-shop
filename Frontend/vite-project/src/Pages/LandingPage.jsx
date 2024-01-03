// LandingPage.jsx
import React from "react";
import { Outlet, useNavigate } from "react-router-dom";
import MovieCard from "../Components/MovieCard";
import MerchandiseList from "../Components/MerchandiseList"; // Import MerchandiseList component
import mmwLogo from "../assets/MMWlogo.png";
import "../index.css";

const LandingPage = ({ children }) => {
  const [searchTerm, setSearchTerm] = React.useState("");
  const [movies, setMovies] = React.useState([]);
  const [selectedMovie, setSelectedMovie] = React.useState(null);
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
   
      const response = await fetch(
        `http://localhost:5257/api/Movie/GetMoviesByTitle/${term}`
      );
      const data = await response.json();
         console.log(data);
      if (data.length >0) {
        setMovies(data);
        
      } else {
        console.log("else")
        setMovies([]);
      }
      
      console.log(movies[0])
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
    <div className="landing-page">
      <header className="logo-header">
        <img src={mmwLogo} alt="Webshop Logo" className="logo" />
      </header>
      <nav className="nav-container">
        <ul className="nav-list">
          <li>
            <a href="/products">Products</a>
          </li>
          <li>
            <a href="/signin">Sign in</a>
          </li>
          <li>
            <a href="/registration">Registration</a>
          </li>
          <li>
            <a href="/cart">Cart</a>
          </li>
        </ul>
      </nav>
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
            {movies.map((movie) => {
              //console.log(movie.title);
              return <option key={movie.id} value={movie.title} />;
            })}
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
            posterUrl={movie.imdbID}
            onClick={() => handleMovieSelect(movie)}
          />
        ))}
      </div>
      <Outlet />
    </div>
  );
};

export default LandingPage;
