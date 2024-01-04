import React, { useEffect, useState } from "react";

const Profile = () => {
  const [userInfo, setUserInfo] = useState(null);

  useEffect(() => {
    const fetchUserInfo = async () => {
      try {
        const response = await fetch("/api/Auth/GetUserByEmail", {
          method: "GET",
          headers: {
            Authorization: `Bearer ${localStorage.getItem("accessToken")}`, 
          },
        });

        if (!response.ok) {
          console.error("Failed to fetch user information:", response.statusText);
          return;
        }

        const data = await response.json();
        setUserInfo(data);
      } catch (error) {
        console.error("Error during user information retrieval:", error);
      }
    };

    fetchUserInfo();
  }, []);

  if (!userInfo) {
    return <p>Loading...</p>;
  }

  return (
    <div className="profile-container">
      <h2>Profile</h2>
      <p>User name: {userInfo.UserName}</p>
      <p>Email: {userInfo.Email}</p>
    </div>
  );
};

export default Profile;
