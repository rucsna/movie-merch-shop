import React, { useState, useEffect } from "react";

const MerchandiseList = ({ movie }) => {
  const [merchandise, setMerchandise] = useState([]);
  const [loading, setLoading] = useState(true);
  const [cart, setCart] = useState([]);

  useEffect(() => {
    const fetchMerchandise = async () => {
      try {
        if (!movie || !movie.id) {
          throw new Error("No valid movie selected");
        }

        const response = await fetch(`/api/MerchItem/ItemsByMovie/${movie.id}`);
        if (!response.ok) {
          throw new Error(`Failed to fetch merchandise. Status: ${response.status}`);
        }

        const data = await response.json();
        setMerchandise(data);
      } catch (error) {
        console.error("Error fetching merchandise:", error.message);
      } finally {
        setLoading(false);
      }
    };

    if (movie && movie.id) {
      fetchMerchandise();
    }
  }, [movie]);

  const addToCart = (item) => {
    setCart([...cart, item]);
    localStorage.setItem("cart", JSON.stringify([...cart, item]));
  };

  const getCartFromLocalStorage = () => {
    const storedCart = localStorage.getItem("cart");

    if (storedCart) {
      setCart(JSON.parse(storedCart));
    }
  };

  useEffect(() => {
    getCartFromLocalStorage();
  }, []);

  if (!movie || !movie.id) {
    return <p>No valid movie selected</p>;
  }

  // Map item types to names
  const getItemName = (type) => {
    switch (type) {
      case 0:
        return "Mug";
      case 1:
        return "Poster";
      case 2:
        return "Shirt";
      default:
        return "Unknown";
    }
  };

  // Group merchandise items by name and calculate total quantity
  const groupedMerchandise = merchandise.reduce((acc, item) => {
    const itemName = getItemName(item.type);
    if (!acc[itemName]) {
      acc[itemName] = { totalQuantity: 0, items: [] };
    }
    acc[itemName].totalQuantity += item.quantity;
    acc[itemName].items.push(item);
    return acc;
  }, {});

  return (
    <div>
      <h2>Available merchandise for {movie.title}:</h2>
      {loading ? (
        <p>Loading merchandise...</p>
      ) : Object.keys(groupedMerchandise).length === 0 ? (
        <p>No merchandise available for this movie</p>
      ) : (
        <div>
          {Object.entries(groupedMerchandise).map(([name, { totalQuantity, items }]) => (
            <div key={name}>
              <p>{name}</p>
              <p>{totalQuantity} pieces, price: ${items[0].price}</p>
              <button type="button" onClick={() => addToCart(items[0])}>Add to cart</button>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default MerchandiseList;
