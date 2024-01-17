import React from "react";
import { Outlet, Link } from "react-router-dom";
import MovieCard from "../Components/MovieCard";
import MerchandiseList from "../Components/MerchandiseList";
import mmwLogo from "../assets/MMWlogo.png";
import "../index.css";

const LandingPage = () => {
  return (
    <div className="landing-page">
      <header className="logo-header">
        <img src={mmwLogo} alt="Webshop Logo" className="logo" />
      </header>
      <nav className="nav-container">
        <ul className="nav-list">
          <li>
            <Link to="/products">Products</Link>
          </li>
          <li>
            <a href="/login">Login</a>
          </li>
          <li>
            <Link to="/registration">Registration</Link>
          </li>
          <li>
            <Link to="/profile">Profile</Link>
          </li>
          <li>
            <Link to="/cart">Cart</Link>
          </li>
        </ul>
      </nav>
      <div className="movie-cards">
        <Outlet />
      </div>
    </div>
  );
};

export default LandingPage;
