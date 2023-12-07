import React, { useState, useEffect } from "react";

const MerchandiseList = ({ movie }) => {
  const [merchandise, setMerchandise] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchMerchandise = async () => {
      try {
        // TODO: Replace the URL with the actual endpoint to fetch merchandise
        const response = await fetch(`https://api.example.com/merchandise?movieId=${movie.imdbID}`);
        const data = await response.json();

   
        setMerchandise(data);
      } catch (error) {
        console.error("Error fetching merchandise:", error);
      } finally {
        setLoading(false);
      }
    };

    if (movie) {
      fetchMerchandise();
    }
  }, [movie]);

  if (!movie) {
    return <p>No movie selected</p>;
  }

  return (
    <div>
      <h2>Merchandise for {movie.Title}</h2>
      {loading ? (
        <p>Loading merchandise...</p>
      ) : merchandise.length === 0 ? (
        <p>No merchandise available for this movie</p>
      ) : (
        <div>
          <p>Available merchandise:</p>
          <ul>
            {merchandise.map((item) => (
              <li key={item.id}>{item.name} - ${item.price}</li>
            ))}
          </ul>
        </div>
      )}
    </div>
  );
};

export default MerchandiseList;
