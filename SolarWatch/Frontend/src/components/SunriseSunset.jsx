import React from "react";

const SunriseSunset = ({ data, token, handleUpdate, handleDelete }) => {
  const handleUpdateSunriseSunset = (e) => {
    e.preventDefault();

    const updatedData = {
      id: data.id,
      city: e.target.city.value,
      sunrise: e.target.sunrise.value,
      sunset: e.target.sunset.value,
      date: e.target.date.value,
      cityId: data.cityModel.cityId,
    };

    handleUpdate(`/api/SolarWatch/sunrise-sunsets/${data.id}`, updatedData);
  };

  return (
    <form onSubmit={handleUpdateSunriseSunset}>
      <div className="sunrise-sunset-form">
        <h2>Sunrise Sunset Model:</h2>

        <label>City: </label>
        <input type="text" name="city" defaultValue={data.city} />

        <label>Sunrise:</label>
        <input type="text" name="sunrise" defaultValue={data.sunrise} />

        <label>Sunset: </label>
        <input type="text" name="sunset" defaultValue={data.sunset} />

        <label>Date: </label>
        <input type="text" name="date" defaultValue={data.date} />

        <button className="update-btn" type="submit">
          Update SunriseSunset
        </button>
        <button
          className="delete-btn"
          type="button"
          onClick={() =>
            handleDelete(`/api/SolarWatch/sunrise-sunsets/${data.id}`)
          }
        >
          Delete SunriseSunset
        </button>
      </div>
    </form>
  );
};

export default SunriseSunset;
