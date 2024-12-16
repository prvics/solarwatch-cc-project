import React from "react";

const SearchForm = ({ onSubmit }) => {
  return (
    <form className="search-form-container" onSubmit={onSubmit}>
      <label>City name:</label>
      <input
        className="input-field"
        type="text"
        name="cityName"
        placeholder="Enter city name"
      />
      <label>Date:</label>
      <input
        className="input-field"
        type="date"
        name="date"
        defaultValue={new Date().toISOString().split("T")[0]}
      />
      <button className="submit-btn" type="submit">
        Submit
      </button>
    </form>
  );
};

export default SearchForm;
