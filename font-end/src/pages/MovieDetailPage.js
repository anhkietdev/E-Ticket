import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import styled from 'styled-components';
import { format } from 'date-fns';
import { vi } from 'date-fns/locale';
import { FaStar, FaClock, FaCalendarAlt, FaTicketAlt } from 'react-icons/fa';
import { movieService, roomService, showTimeService } from '../services/api';
import Header from '../components/common/Header';
import Footer from '../components/common/Footer';
import { useBooking } from '../context/BookingContext';
import { useAuth } from '../context/AuthContext';

const PageContainer = styled.div`
  background-color: #0f0f1e;
  color: #fff;
  min-height: 100vh;
`;

const BackdropContainer = styled.div`
  position: relative;
  height: 60vh;
  background-image: linear-gradient(to bottom, rgba(15, 15, 30, 0.5), #0f0f1e), 
    url(${props => props.backdrop});
  background-size: cover;
  background-position: center;
  display: flex;
  align-items: center;
`;

const ContentContainer = styled.div`
  padding: 0 2rem;
  max-width: 1200px;
  margin: 0 auto;
  display: flex;
  position: relative;
`;

const PosterContainer = styled.div`
  width: 270px;
  margin-right: 2rem;
  margin-top: -250px;
  position: relative;
  z-index: 10;
`;

const Poster = styled.img`
  width: 100%;
  border-radius: 8px;
  box-shadow: 0 5px 15px rgba(0, 0, 0, 0.5);
`;

const MovieInfo = styled.div`
  flex: 1;
`;

const Title = styled.h1`
  font-size: 2.5rem;
  margin-bottom: 1rem;
`;

const InfoGrid = styled.div`
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(180px, 1fr));
  gap: 1rem;
  margin-bottom: 1.5rem;
`;

const InfoItem = styled.div`
  display: flex;
  align-items: center;
  color: #a0a0a0;
`;

const InfoIcon = styled.span`
  margin-right: 10px;
  color: #e94560;
  display: flex;
  align-items: center;
`;

const GenresContainer = styled.div`
  display: flex;
  flex-wrap: wrap;
  margin-bottom: 1.5rem;
  gap: 8px;
`;

const GenreBadge = styled.span`
  background: #e94560;
  color: white;
  padding: 5px 12px;
  border-radius: 20px;
  font-size: 14px;
`;

const Description = styled.div`
  font-size: 16px;
  line-height: 1.6;
  margin-bottom: 2rem;
`;

const BookingSection = styled.div`
  background: #16213e;
  padding: 2rem;
  border-radius: 8px;
  margin-top: 3rem;
`;

const SectionTitle = styled.h2`
  color: #e94560;
  margin-bottom: 1.5rem;
`;

const DateSelector = styled.div`
  display: flex;
  gap: 10px;
  margin-bottom: 2rem;
  overflow-x: auto;
  padding-bottom: 10px;
`;

const DateButton = styled.button`
  padding: 10px 16px;
  background: ${props => props.selected ? '#e94560' : '#1a1a2e'};
  color: ${props => props.selected ? 'white' : '#a0a0a0'};
  border: 1px solid ${props => props.selected ? '#e94560' : '#333'};
  border-radius: 4px;
  cursor: pointer;
  min-width: 80px;
  transition: all 0.3s;
  
  &:hover {
    background: ${props => props.selected ? '#ff6b81' : 'rgba(233, 69, 96, 0.1)'};
    color: ${props => props.selected ? 'white' : '#e94560'};
  }
`;

const CinemaContainer = styled.div`
  margin-bottom: 2rem;
`;

const CinemaHeader = styled.div`
  display: flex;
  align-items: center;
  padding: 15px;
  background: #1a1a2e;
  border-radius: 8px;
  margin-bottom: 1rem;
`;

const CinemaInfo = styled.div`
  flex: 1;
`;

const CinemaName = styled.h3`
  margin: 0 0 5px 0;
  font-size: 18px;
`;

const CinemaAddress = styled.p`
  margin: 0;
  color: #a0a0a0;
  font-size: 14px;
`;

const ShowtimesContainer = styled.div`
  display: flex;
  flex-wrap: wrap;
  gap: 10px;
  padding-left: 15px;
`;

const ShowtimeButton = styled.button`
  padding: 8px 16px;
  background: #1a1a2e;
  color: white;
  border: 1px solid #333;
  border-radius: 4px;
  cursor: pointer;
  transition: all 0.3s;
  
  &:hover {
    background: rgba(233, 69, 96, 0.1);
    color: #e94560;
    border-color: #e94560;
  }
  
  &:disabled {
    opacity: 0.5;
    cursor: not-allowed;
    &:hover {
      background: #1a1a2e;
      color: white;
      border-color: #333;
    }
  }
`;

const NoShowtimes = styled.p`
  color: #a0a0a0;
  padding-left: 15px;
`;

const BookingMessage = styled.div`
  text-align: center;
  padding: 2rem;
  color: #a0a0a0;
`;

const getDateOptions = () => {
  const dates = [];
  const today = new Date();
  
  for (let i = 0; i < 7; i++) {
    const date = new Date();
    date.setDate(today.getDate() + i);
    dates.push(date);
  }
  
  return dates;
};

const MovieDetailPage = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const { selectMovie, selectRoom, selectProjection } = useBooking();
  const { currentUser } = useAuth();
  
  const [movie, setMovie] = useState(null);
  const [genres, setGenres] = useState([]);
  const [rooms, setRooms] = useState([]);
  const [projections, setProjections] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [selectedDate, setSelectedDate] = useState(new Date());
  const [dateOptions] = useState(getDateOptions());
  
  useEffect(() => {
    const fetchMovieDetails = async () => {
      try {
        console.log("Đang lấy dữ liệu phim với ID:", id);
        setLoading(true);
        
        // Fetch thông tin phim
        const movieResponse = await movieService.getById(id);
        console.log("Dữ liệu phim:", movieResponse.data);
        setMovie(movieResponse.data);
        
        // Lấy thể loại từ dữ liệu phim
        if (movieResponse.data && movieResponse.data.filmGenres) {
          // Chỉ cần lấy tên thể loại từ filmGenres
          const extractedGenres = movieResponse.data.filmGenres.map(fg => ({
            name: fg.genre.name
          }));
          setGenres(extractedGenres);
        }
        
        // Fetch danh sách phòng chiếu (rạp)
        const roomsResponse = await roomService.getAll();
        console.log("Dữ liệu phòng chiếu:", roomsResponse.data);
        setRooms(roomsResponse.data);
        
        // Fetch lịch chiếu của phim
        const projectionsResponse = await showTimeService.getByFilmId(id);
        console.log("Dữ liệu lịch chiếu:", projectionsResponse.data);
        setProjections(projectionsResponse.data);
        
      } catch (err) {
        console.error('Error fetching movie details:', err);
        setError('Không thể tải thông tin phim. Vui lòng thử lại sau.');
      } finally {
        setLoading(false);
      }
    };
    
    fetchMovieDetails();
  }, [id]);
  
  const handleDateSelect = (date) => {
    setSelectedDate(date);
  };
  
  const formatShowtimeDate = (date) => {
    return format(new Date(date), 'yyyy-MM-dd');
  };
  
  const formatShortDate = (date) => {
    return format(date, 'EEE, dd/MM', { locale: vi });
  };
  
  const isToday = (date) => {
    const today = new Date();
    return date.getDate() === today.getDate() && 
           date.getMonth() === today.getMonth() && 
           date.getFullYear() === today.getFullYear();
  };
  
  const handleShowtimeSelect = async (room, projection) => {
    if (!currentUser) {
      if (window.confirm('Vui lòng đăng nhập để đặt vé. Bạn có muốn đăng nhập ngay?')) {
        navigate('/login');
      }
      return;
    }
    
    // Lưu thông tin đặt vé vào context
    selectMovie(movie);
    selectRoom(room);
    selectProjection(projection);
    
    console.log("Đang chuyển đến trang chọn ghế", {
      movie,
      room,
      projection
    });
    
    // Chuyển đến trang chọn ghế
    navigate('/booking/seats');
  };
  
  // Lọc lịch chiếu theo ngày đã chọn
  const filteredProjections = projections.filter(projection => {
    const projectionDate = new Date(projection.startTime);
    return formatShowtimeDate(projectionDate) === formatShowtimeDate(selectedDate);
  });
  
  // Nhóm lịch chiếu theo phòng
  const groupedProjections = rooms.map(room => {
    const roomProjections = filteredProjections.filter(projection => 
      projection.roomId === room.id
    );
    
    return {
      room,
      projections: roomProjections
    };
  });

  if (loading) return (
    <PageContainer>
      <Header />
      <div style={{ padding: '2rem' }}>Đang tải...</div>
      <Footer />
    </PageContainer>
  );
  
  if (error) return (
    <PageContainer>
      <Header />
      <div style={{ padding: '2rem' }}>{error}</div>
      <Footer />
    </PageContainer>
  );
  
  if (!movie) return (
    <PageContainer>
      <Header />
      <div style={{ padding: '2rem' }}>Không tìm thấy phim</div>
      <Footer />
    </PageContainer>
  );

  return (
    <PageContainer>
      <Header />
      
      <BackdropContainer backdrop={movie.imageURL || '/images/default-poster.jpg'}>
        <ContentContainer>
          <PosterContainer>
            <Poster src={movie.imageURL || '/images/default-poster.jpg'} alt={movie.title} />
          </PosterContainer>
          
          <MovieInfo>
            <Title>{movie.title}</Title>
            
            <InfoGrid>
              <InfoItem>
                <InfoIcon><FaStar /></InfoIcon>
                <div>{movie.rating || "N/A"}/10</div>
              </InfoItem>
              
              <InfoItem>
                <InfoIcon><FaClock /></InfoIcon>
                <div>{Math.floor(movie.duration / 60)}h {movie.duration % 60}m</div>
              </InfoItem>
              
              <InfoItem>
                <InfoIcon><FaCalendarAlt /></InfoIcon>
                <div>{format(new Date(movie.releaseDate), 'dd/MM/yyyy')}</div>
              </InfoItem>
            </InfoGrid>
            
            <GenresContainer>
              {genres.map((genre, index) => (
                <GenreBadge key={index}>{genre.name}</GenreBadge>
              ))}
            </GenresContainer>
            
            <Description>{movie.description}</Description>
          </MovieInfo>
        </ContentContainer>
      </BackdropContainer>
      
      <ContentContainer>
        <BookingSection>
          <SectionTitle>
            <FaTicketAlt style={{ marginRight: '10px' }} />
            Đặt vé xem phim
          </SectionTitle>
          
          <DateSelector>
            {dateOptions.map((date, index) => (
              <DateButton
                key={index}
                selected={formatShowtimeDate(date) === formatShowtimeDate(selectedDate)}
                onClick={() => handleDateSelect(date)}
              >
                {isToday(date) ? 'Hôm nay' : formatShortDate(date)}
              </DateButton>
            ))}
          </DateSelector>
          
          {groupedProjections.filter(group => group.projections.length > 0).length > 0 ? (
            groupedProjections.map(({ room, projections }) => (
              projections.length > 0 && (
                <CinemaContainer key={room.id}>
                  <CinemaHeader>
                    <CinemaInfo>
                      <CinemaName>{room.roomNumber}</CinemaName>
                      <CinemaAddress>
                        {room.type === 0 ? 'Phòng chiếu thường' : 'Phòng chiếu VIP'} • 
                        Sức chứa: {room.capacity} ghế
                      </CinemaAddress>
                    </CinemaInfo>
                  </CinemaHeader>
                  
                  <ShowtimesContainer>
                    {projections.map(projection => (
                      <ShowtimeButton
                        key={projection.id}
                        onClick={() => handleShowtimeSelect(room, projection)}
                      >
                        {format(new Date(projection.startTime), 'HH:mm')}
                      </ShowtimeButton>
                    ))}
                  </ShowtimesContainer>
                </CinemaContainer>
              )
            ))
          ) : (
            <BookingMessage>
              Không có lịch chiếu cho ngày {format(selectedDate, 'dd/MM/yyyy')}
            </BookingMessage>
          )}
          
          {/* Nút test cho việc debug */}
          <div style={{ marginTop: '2rem', textAlign: 'center' }}>
            <button 
              style={{ 
                backgroundColor: '#e94560', 
                color: 'white', 
                padding: '10px 20px', 
                borderRadius: '4px', 
                border: 'none',
                cursor: 'pointer'
              }}
              onClick={() => {
                if (!rooms.length || !projections.length) {
                  alert('Không có đủ dữ liệu để test');
                  return;
                }
                
                // Dữ liệu mẫu để test
                selectMovie(movie);
                selectRoom(rooms[0]);
                selectProjection({
                  id: "test-projection",
                  startTime: new Date().toISOString(),
                  endTime: new Date(Date.now() + 2 * 60 * 60 * 1000).toISOString(),
                  price: 90000,
                  filmId: movie.id,
                  roomId: rooms[0].id
                });
                
                // Chuyển hướng đến trang chọn ghế
                navigate('/booking/seats');
              }}
            >
              Test: Đến trang chọn ghế
            </button>
          </div>
        </BookingSection>
      </ContentContainer>
      
      <Footer />
    </PageContainer>
  );
};

export default MovieDetailPage;