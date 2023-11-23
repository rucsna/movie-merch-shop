import React from "react";
import { Outlet, Link } from "react-router-dom";

import "./Layout.css";

const Layout = () => (
  <div className="Layout">
    <nav>
      <ul>
        <li className="grow">
          <Link to="/">Movie Merch Shop</Link>
        </li>
      </ul>
    </nav>
    <Outlet />
  </div>
);

export default Layout;
