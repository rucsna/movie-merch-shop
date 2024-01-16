import React, { useState, useEffect } from "react";

const MerchandiseList = ({ movie }) => {
  const [merchandise, setMerchandise] = useState([]);
  const [loading, setLoading] = useState(true);
  const [cart, setCart] = useState([]);

  useEffect(() => {
    const fetchMerchandise = async () => {
      try {
        const response = await fetch(
          `/api/MerchItem/ItemsByMovie/${movie.id}`
        );
        const data = await response.json();

        console.log(data)
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
              <li key={item.id}>
                {movie.title}: type:{item.type} - ${item.price}
                <button type="button" onClick={() => addToCart(item)}>Add to cart</button>
              </li>
            ))}
          </ul>
        </div>
      )}
    </div>
  );
};

export default MerchandiseList;
