import React, { useState, useEffect } from "react";


  const MerchandiseList = ({ movie }) => {
  const [merchandise, setMerchandise] = useState([]);
  const [loading, setLoading] = useState(true);
  const [cart, setCart] = useState([]);
  const [selectedTypes, setSelectedTypes] = useState({
    mug: false,
    poster: false,
    shirt: false,
  });

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

  const handleCheckboxChange = (type) => {
    const typeMappings = {
      mug: "Mug",
      poster: "Poster",
      shirt: "Shirt",
    };

    setSelectedTypes({
      ...selectedTypes,
      [type]: !selectedTypes[type],
    });

    const numericTypes = Object.keys(selectedTypes)
      .filter((key) => selectedTypes[key])
      .map((key) => getItemTypeValue(typeMappings[key]));

    if (movie && movie.id) {
      fetchMerchandise(movie.id, numericTypes);
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
      setMerchandise(data);
    } catch (error) {
      console.error("Error fetching merchandise:", error.message);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    if (movie && movie.id) {
      fetchMerchandise(movie.id, selectedTypes);
    }
  }, [movie, selectedTypes]);

  const addToCart = (item) => {
    setCart([...cart, item]);
    localStorage.setItem("cart", JSON.stringify([...cart, item]));
  };

  /* // Group merchandise items by name and calculate total quantity
  const groupedMerchandise = merchandise.reduce((acc, item) => {
    const itemName = getItemName(item.type);
    if (!acc[itemName]) {
      acc[itemName] = { totalQuantity: 0, items: [] };
    }
    acc[itemName].totalQuantity += item.quantity;
    acc[itemName].items.push(item);
    return acc;
  }, {}); */

  return (
    <div>
      <h2>Available merchandise for {movie.title}:</h2>
      <div>
        <label>
          <input
            type="checkbox"
            checked={selectedTypes.mug}
            onChange={() => handleCheckboxChange("mug")}
          />
          Mug
        </label>
        <label>
          <input
            type="checkbox"
            checked={selectedTypes.poster}
            onChange={() => handleCheckboxChange("poster")}
          />
          Poster
        </label>
        <label>
          <input
            type="checkbox"
            checked={selectedTypes.shirt}
            onChange={() => handleCheckboxChange("shirt")}
          />
          Shirt
        </label>
      </div>
      {loading ? (
        <p>Loading merchandise...</p>
      ) : merchandise.length === 0 ? (
        <p>No merchandise available for this movie</p>
      ) : (
        <div>
          {merchandise.map((item) => (
            <div key={item.id}>
              <p>{item.name}</p>
              <p>
                Quantity: {item.quantity}, Price: ${item.price}
              </p>
              <button type="button" onClick={() => addToCart(item)}>
                Add to cart
              </button>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default MerchandiseList;
