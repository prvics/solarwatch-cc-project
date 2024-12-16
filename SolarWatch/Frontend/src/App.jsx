import { useState } from "react";
import React from "react";

import { Outlet } from "react-router-dom";

function App() {
  const [token, setToken] = useState(localStorage.getItem("token"));
  const [isAdmin, setIsAdmin] = useState(
      localStorage.getItem("isAdmin") === "true",
  );

  const login = (token, role) => {
    sessionStorage.setItem("token", token);
    sessionStorage.setItem("isAdmin", role);
    setToken(token);
    setIsAdmin(role);
  };

  const logout = () => {
    sessionStorage.removeItem("token");
    sessionStorage.removeItem("isAdmin");
    setToken(null);
    setIsAdmin(false);
  };

  return (
      <>
        <Outlet context={{ token, isAdmin, login, logout }} />
      </>
  );
}

export default App;