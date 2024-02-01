import React, { useEffect, useState } from "react";
import { Outlet, Link } from "react-router-dom";
import MovieCard from "../Components/MovieCard";
import MerchandiseList from "../Components/MerchandiseList";
import mmwLogo from "../assets/MMWlogo.png";
import "../index.css";

const LandingPage = () => {
  const [isAuthenticated, setIsAuthenticated] = useState(false); 

  useEffect(() => {
    const accessToken = localStorage.getItem("accessToken");
    setIsAuthenticated(!!accessToken);
  }, [])
  
  return (
    <div className="landing-page">
      <header className="logo-header">
        <img src={mmwLogo} alt="Webshop Logo" className="logo" />
      </header>
      <nav className="nav-container">
        <ul className="nav-list">
          <li>
            <Link to="/">Products</Link>
          </li>
          <li>
            <Link to="/login">Login</Link>
          </li>
          <li>
            <Link to="/registration">Registration</Link>
          </li>
          {isAuthenticated ? (
              <li>
                <Link to="/profile">Profile</Link>
              </li>
          ) : null}
          {isAuthenticated ? (
            <li>
            <Link to="/cart">Cart</Link>
          </li>
          ) : null}
        </ul>
      </nav>
      <div className="movie-cards">
        <Outlet />
      </div>
    </div>
  );
};

export default LandingPage;
