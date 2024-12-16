import React from "react";
import { useNavigate } from "react-router-dom";

const MainPage = () => {
  const navigate = useNavigate();

  return (
    <div className="main-container">
      <h2 className="welcome">Welcome on SolarWatch!</h2>
      <p className="p-login-reg">Please login or register!</p>
      <button onClick={() => navigate("/login")} className="login-btn">
        Login
      </button>
      <button onClick={() => navigate("/register")} className="reg-btn">
        Register
      </button>
    </div>
  );
};

export default MainPage;
