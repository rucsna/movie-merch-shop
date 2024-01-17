import React, { useState, useEffect } from "react";
import MerchandiseSection from "../Components/MerchandiseSection"

const MerchandiseList = ({ movie }) => {
  const [posters, setPosters] = useState([]);
  const [mugs, setMugs] = useState([]);
  const [shirts, setShirts] = useState([]);
  const [loading, setLoading] = useState(true);
  const [cart, setCart] = useState([]);
  const [selectedTypes, setSelectedTypes] = useState({
    mug: true,
    poster: true,
    shirt: true,
  });

  const typeMappings = {
    mug: "Mug",
    poster: "Poster",
    shirt: "Shirt",
  };

  const getItemTypeValue = (type) => {
    switch (type) {
      case "Mug":
        return "0";
      case "Poster":
        return "1";
      case "Shirt":
        return "2";
      default:
        return "-1";
    }
  };

  const fetchMerchandise = async (movieId, itemType) => {
    try {
      if (!movieId || !itemType) {
        throw new Error("Invalid parameters");
      }

      const response = await fetch(
        `/api/MerchItem/ItemsByMovie/${movieId}/${itemType}`
      );

      if (!response.ok) {
        throw new Error(
          `Failed to fetch merchandise. Status: ${response.status}`
        );
      }

      const data = await response.json();
      return data.$values;
    } catch (error) {
      console.error("Error fetching merchandise:", error.message);
      return [];
    }
  };

  const handleCheckboxChange = (type) => {
    setSelectedTypes((prevSelectedTypes) => ({
      ...prevSelectedTypes,
      [type]: !prevSelectedTypes[type],
    }));
  };

  const fetchAllMerchandise = async () => {
    if (movie && movie.id) {
      try {
        setLoading(true);

        const selectedTypeKeys = Object.keys(selectedTypes);

        const fetchPromises = selectedTypeKeys.map((key) =>
          fetchMerchandise(movie.id, getItemTypeValue(typeMappings[key]))
        );

        const responses = await Promise.all(fetchPromises);

        // Update the state for each type
        responses.forEach((data, index) => {
          const type = selectedTypeKeys[index].toLowerCase();
          switch (type) {
            case "mug":
              setMugs(data);
              break;
            case "poster":
              setPosters(data);
              break;
            case "shirt":
              setShirts(data);
              break;
            default:
              break;
          }
        });
      } catch (error) {
        console.error("Error fetching merchandise:", error.message);
      } finally {
        setLoading(false);
      }
    }
  };

  useEffect(() => {
    fetchAllMerchandise();
  }, [movie, selectedTypes]);

  const addToCart = (item) => {
    setCart([...cart, item]);
    localStorage.setItem("cart", JSON.stringify([...cart, item]));
  };

  return (
    <div>
      <h2>Available merchandise for {movie.title}:</h2>
      <div>
        {Object.entries(selectedTypes).map(([type, isChecked]) => (
          <label key={type}>
            <input
              type="checkbox"
              checked={isChecked}
              onChange={() => handleCheckboxChange(type)}
            />
            {type.charAt(0).toUpperCase() + type.slice(1)}
          </label>
        ))}
      </div>
      {loading ? (
        <p>Loading merchandise...</p>
      ) : (
        <div className="merchandise-container">
          {selectedTypes.mug && mugs.length > 0 && (
            <MerchandiseSection type="Mug" items={mugs} addToCart={addToCart} />
          )}
          {selectedTypes.poster && posters.length > 0 && (
            <MerchandiseSection
              type="Poster"
              items={posters}
              addToCart={addToCart}
            />
          )}
          {selectedTypes.shirt && shirts.length > 0 && (
            <MerchandiseSection
              type="Shirt"
              items={shirts}
              addToCart={addToCart}
            />
          )}
        </div>
      )}
    </div>
  );
};


export default MerchandiseList;
