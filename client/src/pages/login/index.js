import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import "./styles.css";
import api from "../../services/api";

import logoImage from "../../assets/logo.svg";
import padlock from "../../assets/padlock.png";

export default function Login() {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");

  const navigate = useNavigate();

  async function Login(e) {
    //avoid page refreash and keep SPA behavior
    e.preventDefault();

    const data = {
      username: username,
      password: password,
    };

    //const data = ["username", "password"];

    try {
      let result = await api.post("api/auth/v1/signin", data).catch((error) => {
        if (error.response) {
          // The server responded with a status code outside the 2xx range
          console.log("Error response:", error.response);
        } else if (error.request) {
          // The request was made but no response was received
          console.log("Error request:", error.request);
        } else {
          // Something happened in setting up the request that triggered an error
          console.log("Error message:", error.message);
        }
      });

      localStorage.setItem("username", username);
      localStorage.setItem("accessToken", result.data.accessToken);
      localStorage.setItem("refreshToken", result.data.refreshToken);

      if (result.data.authenticated) {
        navigate("/books");
      } else {
        alert("error during login");
      }
    } catch (error) {
      alert(error);
    }
  }

  return (
    <div className="login-container">
      <section className="form">
        <img src={logoImage} alt="Erudio Logo" />
        <form onSubmit={Login}>
          <h1>Access your Account</h1>

          <input
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            placeholder="Username"
          />
          <input
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            type="password"
            placeholder="Password"
          />

          <button className="button" type="submit">
            Login
          </button>
        </form>
      </section>

      <img className="padlock" src={padlock} alt="Login" />
    </div>
  );
}
