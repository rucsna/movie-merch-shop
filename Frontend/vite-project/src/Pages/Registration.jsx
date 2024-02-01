import React, { useState } from "react";
import { useNavigate } from "react-router-dom";

const Registration = () => {
  const [username, setUsername] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [passwordMatch, setPasswordMatch] = useState(true);
  const [birthdate, setBirthdate] = useState("");
  const [address, setAddress] = useState("");
  const navigate = useNavigate();

  const handleRegistration = async (e) => {
    e.preventDefault();

    // if(password !== confirmPassword){
    //   console.error("Passwrods do not match");
    //   setPasswordMatch(false);
    //   return;
    // }
    // setPasswordMatch(true);

    const registrationData = {
      UserName: username,
      Email: email,
      Password: password,
      BirthDate: birthdate,
      Address: address,
    };

    try {
      const response = await fetch("/api/Auth/Register", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(registrationData),
      });

      if (!response.ok) {
        console.error("Registration failed:", response.statusText);
        return;
      }

      const data = await response.json();
      console.log("Registration successful");
      console.log("User Email:", data.email);
      console.log("User Token:", data.token);

      navigate(`/login`);
  
    } catch (error) {
      console.error("Error during registration:", error);
    }
  };

  console.log("password match", passwordMatch);
  return (
    <div className="registration-container">
      <h4>Please, enter your data for registering to MovieMerchShop</h4>
      <form onSubmit={handleRegistration}>
        <label>
          Select a username:
          <input
            type="text"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            required
          />
        </label>
        <label>
          Your email address:
          <input
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />
        </label>
        <label>
          Your password:
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
        </label>
        {/* <label>
          Your password again:
          <input
            type="password"
            value={confirmPassword}
            onChange={(e) => setConfirmPassword(e.target.value)}
            required
          />
        </label> */}
        <label>
          Date of birth:
          <input
            type="date"
            value={birthdate}
            onChange={(e) => setBirthdate(e.target.value)}
            required
          />
        </label>
        <label>
          Address:
          <input
            type="text"
            value={address}
            onChange={(e) => setAddress(e.target.value)}
            required
          />
        </label>
        <button type="submit">Register</button>
      </form>
    </div>
  );
};

export default Registration;
