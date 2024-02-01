import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import Profile from "./Profile";

const Login = () => {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate();

  useEffect(() => {
    const accessToken = localStorage.getItem("accessToken");
    setIsLoggedIn(!!accessToken);
  }, [])
  

  const handleLogin = async (e) => {
    e.preventDefault();

    const loginData = {
      Email: email,
      Password: password,
    };

    try {
      const response = await fetch("/api/Auth/Login", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(loginData),
      });
      console.log(loginData);

      if (!response.ok) {
        console.error("Login failed:", response.statusText);
        return;
      }

      const data = await response.json();
      console.log("Login successful");
      console.log("User Email:", data.email);
      console.log("User Token:", data.token);

      localStorage.setItem("accessToken", data.token);
      localStorage.setItem("userEmail", data.email)
      navigate(`/profile`);
    } catch (error) {
      console.error("Error during login:", error);
    }
  };

  return (
    <div className="login-container">
      {isLoggedIn ? (
        <div>
          <h3>You are already logged in.</h3>
          <Profile />
        </div>
      ) : (
        <div>
      <h3>Please, log in!</h3>
      <form onSubmit={handleLogin}>
        <label>
          Email:
          <input
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />
        </label>
        <label>
          Password:
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
        </label>
        <button type="submit">Login</button>
      </form>
      </div>
      )}
    </div>
  );
};

export default Login;
