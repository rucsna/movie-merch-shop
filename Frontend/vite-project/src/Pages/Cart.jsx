import React, { useState, useEffect } from "react";


const Cart = () => {
  const [cart, setCart] = useState([]);
  const [sum, setSum] = useState(0);
  const [order, setOrder] = useState([]);
    const getCurrentUserId = () => {
  const token = localStorage.getItem("accessToken");
  console.log(token)
  if (token) {

    const tokenParts = token.split(".");
    
    if (tokenParts.length === 3) {
     
      const payload = atob(tokenParts[1]);
      const decodedPayload = JSON.parse(payload);
      console.log(decodedPayload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"])
      return decodedPayload[
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
      ];
    }
  
  }}
  const userId = getCurrentUserId();

  useEffect(() => {
    const storedCart = localStorage.getItem("cart");

if (storedCart) {
  setCart(JSON.parse(storedCart));
  const totalPrice = cart.reduce(
    (acc, cartItem) => acc + (cartItem.price || 0),0);
  setSum(totalPrice);
}
  }, []);

   const addOrder = async () => {
    try {
    const orderData = cart.map((cartItem) => ( {id: cartItem.id || 1,}))

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
            orderSum: sum
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
        localStorage.setItem("cart", JSON.stringify([]));

        navigate("/profile");
      } catch (error) {
        console.error("Error during login:", error);
      }
    };

   // localStorage.setItem("cart", JSON.stringify([cart]));
   // console.log(localStorage.getItem)
  


  return (
    <div>
      <h2>Shopping Cart</h2>
      {cart.length === 0 ? (
        <p>Your cart is empty</p>
      ) : (
        <div>
          {" "}
          <ul>
            {cart.map((cartItem, index) => (
              <li key={index}>
                {cartItem.type} - ${cartItem.price}
              </li>
            ))}
          </ul>
          <h1>Sum: {sum} </h1>
          <button type="button" onClick={addOrder}>
           Order
          </button>
        </div>
      )}
    </div>
  );
};

export default Cart;
