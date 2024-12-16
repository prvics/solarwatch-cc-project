import { useState } from "react";
import React from "react";

import { Outlet } from "react-router-dom";

function App() {
  const [token, setToken] = useState(null);
  const [isAdmin, setIsAdmin] = useState(false);

  const login = (token, role) => {
    setToken(token);
    setIsAdmin(role);
  };

  return (
    <>
      <Outlet context={{ token, isAdmin, login }} />
    </>
  );
}

export default App;
