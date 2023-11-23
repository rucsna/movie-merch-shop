import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App.jsx'
import './index.css'


import Layout from './Pages/Layout/Layout.jsx'


const router = createBrowserRouter([
  {
    path: "/",
    element: <Layout />,
    errorElement: <ErrorPage />,
    children: [
      {
        path: "/products",
        element: <Landing />,
      },
      {
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
      },
    ],
  },
]);
ReactDOM.createRoot(document.getElementById('root')).render(
  <React.StrictMode>
    <App />
  </React.StrictMode>,
)
