import React, { createContext, useContext, useState } from 'react';

const BookingContext = createContext();

export const BookingProvider = ({ children }) => {
  const [selectedMovie, setSelectedMovie] = useState(null);
  const [selectedCinema, setSelectedCinema] = useState(null);
  const [selectedShowTime, setSelectedShowTime] = useState(null);
  const [selectedSeats, setSelectedSeats] = useState([]);

  const calculateTotalPrice = () => {
    if (!selectedShowTime || !selectedSeats.length) return 0;
    const basePrice = selectedShowTime.price * selectedSeats.length;
    const vipSurcharge = selectedSeats.filter((seat) => seat.type === 'vip').length * (selectedShowTime.price * 0.2);
    return basePrice + vipSurcharge;
  };

  const completeBooking = async (userId) => {
    return new Promise((resolve) => {
      setTimeout(() => {
        resolve({ id: Math.floor(Math.random() * 1000000) });
      }, 1000);
    });
  };

  const resetBooking = () => {
    setSelectedMovie(null);
    setSelectedCinema(null);
    setSelectedShowTime(null);
    setSelectedSeats([]);
  };

  return (
    <BookingContext.Provider
      value={{
        selectedMovie,
        setSelectedMovie,
        selectedCinema,
        setSelectedCinema,
        selectedShowTime,
        setSelectedShowTime,
        selectedSeats,
        setSelectedSeats,
        calculateTotalPrice,
        completeBooking,
        resetBooking,
      }}
    >
      {children}
    </BookingContext.Provider>
  );
};

export const useBooking = () => useContext(BookingContext);