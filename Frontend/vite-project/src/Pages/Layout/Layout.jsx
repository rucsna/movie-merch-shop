import React from "react";
import { Outlet, Link } from "react-router-dom";
import mmwLogo from "../../Pictures/MMWlogo.png";
import "./Layout.css";

const Layout = () => (
  <div className="Layout">
    <header>
      <img src={mmwLogo} alt="Webshop Logo" className="logo" />
    
    </header>
    <nav>
      <ul>
        <li>
          <a href="/products">Products</a>
        </li>
        <li>
          <a href="/signin">Sign in</a>
        </li>
        <li>
          <a href="/registration">Registration</a>
        </li>
        <li>
          <a href="/cart">Cart</a>
        </li>
       </ul>
    </nav>
    <div className="search-bar">
      <input type="text" placeholder="Search products..." />
      <button>Search</button>
    </div>

  </div>
);

export default Layout;
