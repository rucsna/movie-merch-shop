import React, { useState, useEffect } from "react";

const Cart = () => {
  const [cart, setCart] = useState([]);
  const [sum, setSum] = useState(0);
  const [order, setOrder] = useState([]);
  const [groupedCart, setGroupedCart] = useState({});
  const [merchandise, setMerchandise] = useState([]); // New state for merchandise

  const getCurrentUserId = () => {
    const token = localStorage.getItem("accessToken");
    console.log(token);
    if (token) {
      const tokenParts = token.split(".");
      if (tokenParts.length === 3) {
        const payload = atob(tokenParts[1]);
        const decodedPayload = JSON.parse(payload);
        console.log(
          decodedPayload[
            "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
          ]
        );
        return decodedPayload[
          "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
        ];
      }
    }
  };

  const userId = getCurrentUserId();

  const getItemName = (type) => {
    switch (type) {
      case "0":
        return "Mug";
      case "1":
        return "Poster";
      case "2":
        return "Shirt";
      default:
        return "Unknown";
    }
  };

  // Function to fetch merchandise for a specific movie
  const fetchMerchandise = async (movieId) => {
    try {
      const response = await fetch(`/api/MerchItem/ItemsByMovie/${movieId}`);
      const data = await response.json();

      console.log(data);
      setMerchandise(data);
    } catch (error) {
      console.error("Error fetching merchandise:", error);
    }
  };

  useEffect(() => {
    const storedCart = localStorage.getItem("cart");

    if (storedCart) {
      const parsedCart = JSON.parse(storedCart);
      setCart(parsedCart);

      const grouped = parsedCart.reduce((acc, item) => {
        const itemType = item.type.toString();
        if (!acc[itemType]) {
          acc[itemType] = [];
        }
        acc[itemType].push(item);
        return acc;
      }, {});

      setGroupedCart(grouped);

      const totalPrice = parsedCart.reduce(
        (acc, cartItem) => acc + (cartItem.price || 0),
        0
      );
      setSum(totalPrice);
    }
  }, []);

  const addOrder = async () => {
    try {
      const orderData = cart.map((cartItem) => ({ id: cartItem.id || 1 }));

      setOrder([...order, ...orderData]);

      setCart([]);

      const response = await fetch("/api/Order/AddOrder", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          userId: userId,
          orderedItemIds: orderData.map((item) => item.id),
          orderSum: sum,
        }),
      });

      if (!response.ok) {
        console.log(JSON.stringify(order));
        console.error("Add order failed:", response.statusText);
        return;
      }

      const data = await response.json();
      console.log("Order successful");
      console.log("Order length:", order.length);

      setCart([]);
      setGroupedCart({});
      localStorage.setItem("cart", JSON.stringify([]));
    } catch (error) {
      console.error("Error during login:", error);
    }
  };

  return (
    <div>
      <h2>Shopping Cart</h2>
      {cart.length === 0 ? (
        <p>Your cart is empty</p>
      ) : (
        <div>
          <ul>
            {Object.entries(groupedCart).map(([type, items]) => (
              <li key={type}>
                {getItemName(type)} - {items.length} pieces, price: $
                {items.reduce((acc, item) => acc + item.price, 0)}
              </li>
            ))}
          </ul>
          <h1>Sum: {sum} </h1>
          <button type="button" onClick={addOrder}>
            Order
          </button>
        </div>
      )}

      <ul>
        {merchandise.map((item) => (
          <li key={item.id}>
            {/* Display merchandise details */}
            {item.type} - ${item.price}
          </li>
        ))}
      </ul>
    </div>
  );
};

export default Cart;
