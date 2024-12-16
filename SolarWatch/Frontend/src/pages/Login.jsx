import React from "react";
import { jwtDecode } from "jwt-decode";
import { useNavigate, useOutletContext } from "react-router-dom";

const Login = () => {
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

  const handleSubmit = (e) => {
    e.preventDefault();
    const email = e.target.email.value;
    const password = e.target.password.value;

    const requestOptions = {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ email, password }),
    };

    const loginUser = async () => {
      const response = await fetch("/api/Auth/Login", requestOptions);
      if (!response.ok) {
        alert("Wrong e-mail or password");
        return;
      }
      const data = await response.json();
      login(data.token, roleCheck(data.token));
      navigate("/solarwatch");
    };
    loginUser();
  };

  return (
    <>
      <div className="login-form-container">
        <h1>Login</h1>
        <form onSubmit={handleSubmit}>
          <div>
            <label htmlFor="email">Email address:</label>
            <input type="email" name="email" placeholder="Enter your email" />
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
            <button className="login-btn">Login</button>
          </div>
        </form>
      </div>
    </>
  );
};

export default Login;
