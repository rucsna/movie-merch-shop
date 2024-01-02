import React from "react";
import "../index.css";

const MovieCard = ({ title, year, posterUrl, onClick }) => {
  return (
    <div className="movie-card" onClick={onClick}>
      <img src={posterUrl} alt={`${title} Poster`} className="poster" />
      <div className="details">
        <h3 className="title">{title}</h3>
        <p className="year">{year}</p>
      </div>
    </div>
  );
};

export default MovieCard;
