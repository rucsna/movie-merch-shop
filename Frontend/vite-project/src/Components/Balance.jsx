import React, { useState } from "react";
import { useNavigate } from "react-router-dom";

const Balance = ({ email }) => {
  const [amount, setAmount] = useState("");
  const navigate = useNavigate();

  const handleBalance = async (e) => {
    e.preventDefault();

    try {
      const balanceResponse = await fetch("/api/Auth/UpBalance", {
        method: "PATCH",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          email: email,
          balance: parseFloat(amount),
        }),
      });

      if (!balanceResponse.ok) {
        console.error(
          `Failed to update balance. Status: ${balanceResponse.status}`
        );
        return;
      }

      console.log("Balance updated successfully");
       navigate("/profile");
    } catch (error) {
      console.error("Error during balance update:", error);
    }
  };

  return (
    <div>
      <h2>Up to balance!</h2>
      <form onSubmit={handleBalance}>
        <label>
          Amount:
          <input
            type="number" 
            value={amount}
            onChange={(e) => setAmount(e.target.value)}
            required
          />
        </label>
        <button type="submit">Up!</button>
      </form>
    </div>
  );
};

export default Balance;
