import React, { useEffect, useState } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import styled from 'styled-components';
import { format } from 'date-fns';
import { FaCouch, FaRegCheckCircle, FaRegTimesCircle, FaLock } from 'react-icons/fa';
import { seatService } from '../services/api';
import Header from '../components/common/Header';
import Footer from '../components/common/Footer';
import { useAuth } from '../context/AuthContext';
import { useBooking } from '../context/BookingContext';

const PageContainer = styled.div`
  background-color: #0f0f1e;
  color: #fff;
  min-height: 100vh;
`;

const ContentContainer = styled.div`
  max-width: 1000px;
  margin: 0 auto;
  padding: 2rem;
`;

const MovieInfo = styled.div`
  display: flex;
  margin-bottom: 2rem;
  background: #16213e;
  border-radius: 8px;
  overflow: hidden;
`;

const MoviePoster = styled.img`
  width: 100px;
  height: 150px;
  object-fit: cover;
`;

const MovieDetails = styled.div`
  padding: 1rem;
  flex: 1;
`;

const MovieTitle = styled.h2`
  margin: 0 0 0.5rem;
  font-size: 1.5rem;
`;

const ShowtimeInfo = styled.div`
  display: flex;
  flex-wrap: wrap;
  gap: 1rem;
  color: #a0a0a0;
  font-size: 0.9rem;
`;

const InfoItem = styled.div`
  display: flex;
  align-items: center;
`;

const InfoIcon = styled.span`
  display: flex;
  align-items: center;
  margin-right: 0.5rem;
  color: #e94560;
`;

const SeatSelectionContainer = styled.div`
  margin-bottom: 3rem;
`;

const SectionTitle = styled.h3`
  margin-bottom: 1.5rem;
  color: #e94560;
`;

const Screen = styled.div`
  height: 40px;
  background: #1a1a2e;
  margin: 0 auto 2rem;
  width: 80%;
  border-radius: 50%;
  position: relative;
  box-shadow: 0 0 10px rgba(255, 255, 255, 0.2);
  display: flex;
  justify-content: center;
  align-items: center;
  color: #a0a0a0;
  font-size: 0.9rem;
  transform: perspective(300px) rotateX(-30deg);
`;

const SeatsContainer = styled.div`
  display: flex;
  flex-direction: column;
  gap: 10px;
  align-items: center;
  margin-bottom: 2rem;
`;

const Row = styled.div`
  display: flex;
  gap: 10px;
  align-items: center;
`;

const RowLabel = styled.div`
  width: 30px;
  text-align: center;
  font-weight: bold;
`;

const Seat = styled.div`
  width: 40px;
  height: 40px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 8px;
  cursor: ${props => (props.$isAvailable ? 'pointer' : 'not-allowed')};
  background-color: ${props => {
    if (props.$isSelected) return '#e94560';
    if (!props.$isAvailable) return '#333';
    return props.$type === 'vip' ? '#ff9800' : '#1a1a2e';
  }};
  color: ${props => {
    if (props.$isSelected) return '#fff';
    if (!props.$isAvailable) return '#777';
    return props.$type === 'vip' ? '#fff' : '#fff';
  }};
  transition: all 0.2s;

  &:hover {
    transform: ${props => (props.$isAvailable ? 'scale(1.1)' : 'none')};
  }
`;

const SeatIcon = styled(FaCouch)`
  font-size: 1.5rem;
`;

const SeatLegend = styled.div`
  display: flex;
  justify-content: center;
  gap: 20px;
  margin-bottom: 2rem;
`;

const LegendItem = styled.div`
  display: flex;
  align-items: center;
  font-size: 0.9rem;
  color: #a0a0a0;
`;

const LegendColor = styled.div`
  width: 20px;
  height: 20px;
  border-radius: 4px;
  margin-right: 8px;
  background-color: ${props => props.color};
`;

const SummaryContainer = styled.div`
  background: #16213e;
  border-radius: 8px;
  padding: 1.5rem;
`;

const SummaryRow = styled.div`
  display: flex;
  justify-content: space-between;
  margin-bottom: 1rem;

  &:last-child {
    margin-bottom: 0;
    padding-top: 1rem;
    border-top: 1px solid #333;
    font-weight: bold;
    font-size: 1.2rem;
  }
`;

const SelectedSeatsList = styled.div`
  display: flex;
  flex-wrap: wrap;
  gap: 8px;
`;

const SelectedSeatBadge = styled.span`
  background: #1a1a2e;
  color: white;
  padding: 5px 10px;
  border-radius: 4px;
  font-size: 0.9rem;
`;

const ButtonContainer = styled.div`
  margin-top: 2rem;
  display: flex;
  justify-content: space-between;
`;

const Button = styled.button`
  padding: 12px 24px;
  border-radius: 4px;
  font-weight: bold;
  cursor: pointer;
  transition: all 0.3s;

  &:disabled {
    opacity: 0.5;
    cursor: not-allowed;
  }
`;

const BackButton = styled(Button)`
  background: transparent;
  color: #e94560;
  border: 2px solid #e94560;

  &:hover:not(:disabled) {
    background: #e94560;
    color: white;
  }
`;

const ContinueButton = styled(Button)`
  background: #e94560;
  color: white;
  border: none;

  &:hover:not(:disabled) {
    background: #ff6b81;
  }
`;

const SeatSelectionPage = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const { currentUser } = useAuth();
  const { setSelectedSeats, setSelectedMovie, setSelectedShowTime, selectedMovie: contextMovie, selectedShowTime: contextShowTime } = useBooking();

  // Use data from location.state, fallback to context
  const { movie = contextMovie, showTime = contextShowTime } = location.state || {};

  const [seats, setSeats] = useState([]);
  const [selectedSeats, setLocalSelectedSeats] = useState([]);
  const [seatMap, setSeatMap] = useState([]);
  const [roomInfo, setRoomInfo] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    if (!currentUser) {
      navigate('/login');
      return;
    }

    if (!movie || !showTime || !showTime.room || !showTime.room.id) {
      let errorMessage = 'Incomplete data. Please select a showtime again.';
      if (!movie) errorMessage += ' Missing movie.';
      if (!showTime) errorMessage += ' Missing showTime.';
      if (showTime && !showTime.room) errorMessage += ' Missing showTime.room.';
      if (showTime && showTime.room && !showTime.room.id) errorMessage += ' Missing showTime.room.id.';
      setError(errorMessage);
      setTimeout(() => navigate(movie ? `/movies/${movie.id}` : '/movies'), 3000);
      setLoading(false);
      return;
    }

    if (!contextMovie && movie) {
      setSelectedMovie(movie);
    }
    if (!contextShowTime && showTime) {
      setSelectedShowTime(showTime);
    }

    const fetchRoomAndSeats = async () => {
      try {
        setLoading(true);
        const roomId = showTime.room.id;

        const roomResponse = await seatService.getRoomById(roomId);
        if (!roomResponse.data) {
          throw new Error('Room information not found.');
        }
        setRoomInfo(roomResponse.data);

        const seatsResponse = await seatService.getByShowTimeId(showTime.id);
        if (!seatsResponse.data) {
          throw new Error('Seat information not found.');
        }
        setSeats(seatsResponse.data || []);
      } catch (err) {
        setError(err.message || 'Unable to load room or seat information. Please try again later.');
        setTimeout(() => navigate(movie ? `/movies/${movie.id}` : '/movies'), 3000);
      } finally {
        setLoading(false);
      }
    };

    fetchRoomAndSeats();
  }, [currentUser, navigate, movie, showTime, contextMovie, contextShowTime, setSelectedMovie, setSelectedShowTime]);

  useEffect(() => {
    if (!roomInfo || !showTime) return;

    const rowCount = parseInt(roomInfo.row) || 8;
    const seatsPerRow = parseInt(roomInfo.seatInRow) || 1;
    if (rowCount <= 0 || seatsPerRow <= 0) {
      setError('Invalid room information: row or seatInRow is incorrect.');
      setTimeout(() => navigate(movie ? `/movies/${movie.id}` : '/movies'), 3000);
      return;
    }

    const rows = Array.from({ length: rowCount }, (_, i) => String.fromCharCode(65 + i));

    const map = [];
    rows.forEach(row => {
      const rowSeats = [];
      for (let num = 1; num <= seatsPerRow; num++) {
        const seatFromApi = seats.find(
          s => (s.Row || s.row) === row && (s.SeatNumber || s.seatNumber) === num.toString()
        );
        if (seatFromApi) {
          const isAvailable =
            !seatFromApi.Tickets ||
            seatFromApi.Tickets.length === 0 ||
            !seatFromApi.Tickets.some(ticket => ticket.ProjectionId === showTime.id);
          rowSeats.push({
            id: seatFromApi.Id || seatFromApi.id,
            showTimeId: showTime.id,
            row: seatFromApi.Row || seatFromApi.row,
            number: parseInt(seatFromApi.SeatNumber || seatFromApi.seatNumber),
            type: row === 'A' || row === 'B' ? 'standard' : row === 'C' || row === 'D' ? 'vip' : 'standard',
            price: row === 'A' || row === 'B' ? showTime.price : row === 'C' || row === 'D' ? showTime.price * 1.2 : showTime.price,
            isAvailable: isAvailable,
          });
        } else {
          rowSeats.push({
            id: `missing-${row}-${num}`,
            showTimeId: showTime.id,
            row,
            number: num,
            type: row === 'A' || row === 'B' ? 'standard' : row === 'C' || row === 'D' ? 'vip' : 'standard',
            price: row === 'A' || row === 'B' ? showTime.price : row === 'C' || row === 'D' ? showTime.price * 1.2 : showTime.price,
            isAvailable: false,
          });
        }
      }
      map.push({ row, seats: rowSeats });
    });
    setSeatMap(map);
  }, [seats, showTime, roomInfo, navigate, movie]);

  const handleSeatClick = seat => {
    if (seat.isAvailable) {
      setLocalSelectedSeats(prev =>
        prev.some(s => s.id === seat.id)
          ? prev.filter(s => s.id !== seat.id)
          : [...prev, seat]
      );
    }
  };

  const calculateTotalPrice = () => {
    return selectedSeats.reduce((total, seat) => total + seat.price, 0);
  };

  const handleContinue = () => {
    if (selectedSeats.length > 0) {
      setSelectedSeats(selectedSeats);
      navigate('/booking/checkout', {
        state: {
          movie: movie,
          showTime: showTime,
          selectedSeats: selectedSeats,
        },
      });
    }
  };

  const handleBack = () => {
    navigate(`/movies/${movie?.id || ''}`);
  };

  const isSeatSelected = seat => {
    return selectedSeats.some(s => s.id === seat.id);
  };

  const formatPrice = price => {
    return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(price);
  };

  if (loading) {
    return (
      <PageContainer>
        <Header />
        <ContentContainer>Loading...</ContentContainer>
        <Footer />
      </PageContainer>
    );
  }

  if (error) {
    return (
      <PageContainer>
        <Header />
        <ContentContainer>
          {error}
          <ButtonContainer>
            <BackButton onClick={() => navigate(movie ? `/movies/${movie.id}` : '/movies')}>
              {movie ? 'Select another showtime' : 'Back to movies'}
            </BackButton>
          </ButtonContainer>
        </ContentContainer>
        <Footer />
      </PageContainer>
    );
  }

  return (
    <PageContainer>
      <Header />
      <ContentContainer>
        <MovieInfo>
          <MoviePoster
            src={movie.imageURL || 'https://via.placeholder.com/100x150?text=No+Image'}
            alt={movie.title}
          />
          <MovieDetails>
            <MovieTitle>{movie.title}</MovieTitle>
            <ShowtimeInfo>
              <InfoItem>
                <InfoIcon>
                  <FaRegCheckCircle />
                </InfoIcon>
                <div>Room: {showTime.room?.roomNumber || 'Unknown'}</div>
              </InfoItem>
              <InfoItem>
                <InfoIcon>
                  <FaRegTimesCircle />
                </InfoIcon>
                <div>{format(new Date(showTime.startTime), 'dd/MM/yyyy HH:mm')}</div>
              </InfoItem>
              <InfoItem>
                <InfoIcon>
                  <FaLock />
                </InfoIcon>
                <div>
                  Showtime:{' '}
                  <span>
                    {format(new Date(showTime.startTime), 'HH:mm')} -{' '}
                    {format(
                      new Date(showTime.endTime || new Date(showTime.startTime).setHours(new Date(showTime.startTime).getHours() + 2)),
                      'HH:mm'
                    )}
                  </span>
                </div>
              </InfoItem>
            </ShowtimeInfo>
          </MovieDetails>
        </MovieInfo>

        <SeatSelectionContainer>
          <SectionTitle>Select Seats</SectionTitle>
          <Screen>Screen</Screen>
          <SeatsContainer>
            {seatMap.length > 0 ? (
              seatMap.map(row => (
                <Row key={row.row}>
                  <RowLabel>{row.row}</RowLabel>
                  {row.seats.map(seat => (
                    <Seat
                      key={`${seat.row}-${seat.number}`}
                      $isAvailable={seat.isAvailable}
                      $isSelected={isSeatSelected(seat)}
                      $type={seat.type}
                      onClick={() => handleSeatClick(seat)}
                    >
                      <SeatIcon />
                    </Seat>
                  ))}
                  <RowLabel>{row.row}</RowLabel>
                </Row>
              ))
            ) : (
              <div>No seats available to display.</div>
            )}
          </SeatsContainer>

          <SeatLegend>
            <LegendItem>
              <LegendColor color="#1a1a2e" />
              <span>Standard Seat</span>
            </LegendItem>
            <LegendItem>
              <LegendColor color="#ff9800" />
              <span>VIP Seat</span>
            </LegendItem>
            <LegendItem>
              <LegendColor color="#e94560" />
              <span>Selected</span>
            </LegendItem>
            <LegendItem>
              <LegendColor color="#333" />
              <span>Booked or Unavailable</span>
            </LegendItem>
          </SeatLegend>

          <SummaryContainer>
            <SectionTitle>Booking Information</SectionTitle>
            <SummaryRow>
              <div>Selected Seats:</div>
              <SelectedSeatsList>
                {selectedSeats.length > 0 ? (
                  selectedSeats.map(seat => (
                    <SelectedSeatBadge key={seat.id}>
                      {seat.row}
                      {seat.number} ({seat.type === 'vip' ? 'VIP' : 'Standard'})
                    </SelectedSeatBadge>
                  ))
                ) : (
                  <span style={{ color: '#a0a0a0' }}>No seats selected</span>
                )}
              </SelectedSeatsList>
            </SummaryRow>
            <SummaryRow>
              <div>Total Seats:</div>
              <div>{selectedSeats.length}</div>
            </SummaryRow>
            <SummaryRow>
              <div>Total Price:</div>
              <div>{formatPrice(calculateTotalPrice())}</div>
            </SummaryRow>
          </SummaryContainer>

          <ButtonContainer>
            <BackButton onClick={handleBack}>Back</BackButton>
            <ContinueButton disabled={selectedSeats.length === 0} onClick={handleContinue}>
              Continue
            </ContinueButton>
          </ButtonContainer>
        </SeatSelectionContainer>
      </ContentContainer>
      <Footer />
    </PageContainer>
  );
};

export default SeatSelectionPage;