
import React from "react";
import LandingPage from "../LandingPage";

const Layout = ({ children }) => {
  return (
    <div className="Layout">
      <LandingPage />
      {children}
    </div>
  );
};

export default Layout;
