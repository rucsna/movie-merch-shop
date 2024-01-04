import React from 'react'
import ReactDOM from 'react-dom/client'
import { createBrowserRouter, RouterProvider } from "react-router-dom";

import './index.css'


import Layout from './Pages/Layout';
import ErrorPage from "./Pages/ErrorPage";
import Products from './Pages/Products';
import Registration from './Pages/Registration';
import Login from './Pages/Login';


const router = createBrowserRouter([
  {
    path: "/",
    element: <Layout />,
    errorElement: <ErrorPage />,
    children: [
       {
        path: "/products",
        element: <Products />,
      },
      {
        path: "/registration", 
        element: <Registration />,
      },
      {
        path: "/login",
        element: <Login />,
      },
     /*  {
        path: "/:movieId/products",
        element: <ProductsByMovie />,
      },
      {
        path: "/:movieId/products/:filter",
        element: <ProductsByCategories />,
      },
      {
        path: "/:itemId",
        element: <Item />,
      },
      {
        path: "/:userId/cart",
        element: <Cart />,
      },
      {
        path: "/signin",
        element: <SignIn />,
      },
      */
    ],
  },
]);
const root = ReactDOM.createRoot(document.getElementById("root"));
root.render(
  <React.StrictMode>
    <RouterProvider router={router} />
  </React.StrictMode>
);
