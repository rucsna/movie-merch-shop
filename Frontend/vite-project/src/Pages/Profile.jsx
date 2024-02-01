import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import Balance from "../Components/Balance"

const Profile = () => {
  const [userInfo, setUserInfo] = useState(null);
  const [upToBalance, setUpToBalance] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchUserInfo = async () => {
      const email = localStorage.getItem("userEmail");
      try {
        const response = await fetch(`/api/Auth/GetUserByEmail/${email}`, {
          method: "GET",
          headers: {
            Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
          },
        });

        if (!response.ok) {
          console.error(
            "Failed to fetch user information:",
            response.statusText
          );
          return;
        }

        const data = await response.json();
        setUserInfo(data);
      } catch (error) {
        console.error("Error during user information retrieval:", error);
      }
      await fetchUserInfo();
    };

    fetchUserInfo();
  }, [setUpToBalance]);

  if (!userInfo) {
    return <p>Loading...</p>;
  };

  const handleBalanceClick = () => {
    setUpToBalance(!upToBalance)
  };

  const handleLogOut = () => {
    localStorage.clear();
    navigate("/");
  };

  return (
    <div className="profile-container">
      <p>Welcome, {userInfo.userName}</p>
      <p>Balance: {userInfo.balance}</p>
      <button onClick={handleBalanceClick}>Top up balance!</button>
      {upToBalance && (
        <div>
          <Balance email={userInfo.email} />
        </div>
      )}
      <p></p>
      <button onClick={handleLogOut}>Log out</button>
    </div>
  );
};

export default Profile;
