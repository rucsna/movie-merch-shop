import React from 'react'
import ReactDOM from 'react-dom/client'
import { createBrowserRouter, RouterProvider } from "react-router-dom";

import './index.css'


import Layout from './Pages/Layout'


const router = createBrowserRouter([
  {
    path: "/",
    element: <Layout />,
    // errorElement: <ErrorPage />,
    children: [
     /*  {
        path: "/products",
        element: <Layout />,
      }, */
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
      {
        path: "/registration",
        element: <Registration />,
      }, */
    ],
  },
]);
const root = ReactDOM.createRoot(document.getElementById("root"));
root.render(
  <React.StrictMode>
    <RouterProvider router={router} />
  </React.StrictMode>
);
