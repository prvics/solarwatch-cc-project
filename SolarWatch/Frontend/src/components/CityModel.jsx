import React from "react";

const CityModel = ({ cityModel, token, handleUpdate, handleDelete }) => {
  const handleUpdateCityModel = (e) => {
    e.preventDefault();

    const updatedCityModel = {
      cityId: cityModel.cityId,
      city: e.target.city.value,
      lat: e.target.lat.value,
      lon: e.target.lon.value,
      country: e.target.country.value,
      state: e.target.state.value,
    };

    handleUpdate(
      `/api/SolarWatch/cities/${cityModel.cityId}`,
      updatedCityModel
    );
  };

  return (
    <form onSubmit={handleUpdateCityModel}>
      <div className="city-form">
        <h2>City Model</h2>

        <label>CityID:</label>
        <input type="text" name="cityId" defaultValue={cityModel.cityId} />

        <label>City: </label>
        <input type="text" name="city" defaultValue={cityModel.city} />

        <label>Country:</label>
        <input type="text" name="country" defaultValue={cityModel.country} />

        <label>Lat: </label>
        <input type="text" name="lat" defaultValue={cityModel.lat} />

        <label>Lon: </label>
        <input type="text" name="lon" defaultValue={cityModel.lon} />

        <label>State:</label>
        <input type="text" name="state" defaultValue={cityModel.state} />

        <button className="update-btn" type="submit">
          Update CityModel
        </button>
        <button
          className="delete-btn"
          type="button"
          onClick={() =>
            handleDelete(`/api/SolarWatch/cities/${cityModel.cityId}`)
          }
        >
          Delete CityModel
        </button>
      </div>
    </form>
  );
};

export default CityModel;
