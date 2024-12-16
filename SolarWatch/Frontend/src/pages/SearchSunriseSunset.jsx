import React, { useState } from "react";
import { useNavigate, useOutletContext } from "react-router-dom";
import Admin from "../components/Admin";
import CityModel from "../components/CityModel";
import SunriseSunset from "../components/SunriseSunset";
import SearchForm from "../components/SearchForm";

const SearchSunriseSunset = () => {
  const navigate = useNavigate();
  const { token, isAdmin } = useOutletContext();
  const [data, setData] = useState("");

  const handleSubmit = (e) => {
    e.preventDefault();
    setData("");

    const city = e.target.cityName.value;
    const date = e.target.date.value;
    let dateTime;

    if (!city) {
      alert("City is required");
      return;
    }

    if (date) {
      dateTime = new Date(date).toISOString();
    }

    const link = dateTime
      ? `/api/SolarWatch?city=${city}&dateTime=${dateTime}`
      : `/api/SolarWatch?city=${city}`;

    const getData = async () => {
      try {
        const response = await fetch(link, {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });
        if (!response.ok) throw new Error("Network response was not ok");
        const result = await response.json();
        setData(result);
      } catch (e) {
        alert(e.message);
      }
    };

    getData();
  };

  const handleUpdate = async (url, updatedData) => {
    const request = {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
      body: JSON.stringify(updatedData),
    };

    try {
      const response = await fetch(url, request);
      if (!response.ok) throw new Error("Network response was not ok");
      alert("Update successful!");
    } catch (error) {
      alert(error.message);
    }
  };

  const handleDelete = async (url) => {
    try {
      const response = await fetch(url, {
        method: "DELETE",
        headers: { Authorization: `Bearer ${token}` },
      });
      if (!response.ok) throw new Error("Failed to delete data");
      setData("");
      alert("Data deleted successfully");
    } catch (error) {
      alert(error.message);
    }
  };

  return (
    <div className="page-container">
      <div className="header">
        {isAdmin ? (
          <h2>Admin mode activated, welcome back!</h2>
        ) : (
          <h2>Welcome to SolarWatch!</h2>
        )}
        {isAdmin && <Admin />}
        <button onClick={() => navigate("/")} className="logout-btn">
          Logout
        </button>
        <SearchForm onSubmit={handleSubmit} />
      </div>
      {data && (
        <div className="search-container">
          {isAdmin ? (
            <div className="admin-result">
              <SunriseSunset
                data={data}
                token={token}
                handleUpdate={handleUpdate}
                handleDelete={handleDelete}
              />
              <CityModel
                cityModel={data.cityModel}
                token={token}
                handleUpdate={handleUpdate}
                handleDelete={handleDelete}
              />
            </div>
          ) : (
            <div className="user-results">
              <h2>City: {data.city}</h2>
              <p>Sunrise: {data.sunrise}</p>
              <p>Sunset: {data.sunset}</p>
              <p>On {data.date}</p>
            </div>
          )}
        </div>
      )}
    </div>
  );
};

export default SearchSunriseSunset;
