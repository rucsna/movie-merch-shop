import React, { useEffect, useState } from "react";
import "../index.css";
import { useNavigate } from "react-router-dom";
const Balance = ({ title, year, imdbID, onClick }) => {
  const [amount, setAmount] = useState("");

  const navigate = useNavigate();

  const handleBalance = async (e) => {
    e.preventDefault();
    console.log("ide jön majd egy fetch");
  };

  return (
    <div>
      <h2>Up to balance!</h2>
      <form onSubmit={handleBalance}>
        <label>
          Amount:
          <input
            type="string"
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
