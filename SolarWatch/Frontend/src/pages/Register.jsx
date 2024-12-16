import React from "react";
import { jwtDecode } from "jwt-decode";
import { useNavigate, useOutletContext } from "react-router-dom";

const Register = () => {
  const navigate = useNavigate();
  const { login } = useOutletContext();
  const roleCheck = (token) => {
    if (!token) return false;

    const decodedToken = jwtDecode(token);

    const userRole =
      decodedToken[
        "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
      ];

    return userRole === "Admin";
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    const email = e.target.email.value;
    const username = e.target.username.value;
    const password = e.target.password.value;
    const cPassword = e.target.cPassword.value;

    if (password !== cPassword) {
      alert("Passwords do not match!");
      return;
    }

    const requestOptions = {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ email, username, password }),
    };

    const requestLog = {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ email, password }),
    };

    try {
      const regResponse = await fetch("/api/Auth/Register", requestOptions);
      if (!regResponse.ok) {
        const regError = await regResponse.json();
        throw new Error(regError.message || "Registration failed");
      }

      const logResponse = await fetch("/api/Auth/Login", requestLog);
      if (!logResponse.ok) {
        const logError = await logResponse.json();
        throw new Error(logError.message || "Login failed");
      }

      const data = await logResponse.json();
      login(data.token, roleCheck(data.token));
      navigate("/solarwatch");
    } catch (error) {
      alert(error.message);
    }
  };

  return (
    <>
      <div className="register-form-container">
        <h1>Register</h1>
        <form onSubmit={handleSubmit}>
          <div>
            <label htmlFor="email">Email address:</label>
            <input type="email" name="email" placeholder="Enter your email" />
          </div>
          <div>
            <label htmlFor="username">Username:</label>
            <input
              type="username"
              name="username"
              placeholder="Enter your username"
            />
          </div>
          <div>
            <label htmlFor="password">Password:</label>
            <input
              type="password"
              name="password"
              placeholder="Enter your password"
            />
          </div>
          <div>
            <label htmlFor="cPassword">Confirm Password:</label>
            <input
              type="password"
              name="cPassword"
              placeholder="Confirm your password"
            />
          </div>
          <div>
            <button className="reg-btn">Register</button>
          </div>
        </form>
      </div>
    </>
  );
};

export default Register;
