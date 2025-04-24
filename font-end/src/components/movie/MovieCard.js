import React from 'react';
import { Link } from 'react-router-dom';
import styled from 'styled-components';
import { format } from 'date-fns';
import { FaStar, FaClock, FaCalendarAlt } from 'react-icons/fa';

const Card = styled(Link)`
  background: #16213e;
  border-radius: 8px;
  overflow: hidden;
  transition: transform 0.3s, box-shadow 0.3s;
  cursor: pointer;
  text-decoration: none;
  color: inherit;
  display: block;
  
  &:hover {
    transform: translateY(-10px);
    box-shadow: 0 10px 20px rgba(0, 0, 0, 0.3);
  }
`;

const PosterContainer = styled.div`
  position: relative;
  height: 320px;
  overflow: hidden;
`;

const Poster = styled.img`
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform 0.5s;
  
  ${Card}:hover & {
    transform: scale(1.05);
  }
`;

const Badge = styled.span`
  position: absolute;
  top: 10px;
  left: 10px;
  background: #e94560;
  color: white;
  padding: 4px 8px;
  border-radius: 4px;
  font-size: 12px;
  font-weight: bold;
`;

const ContentContainer = styled.div`
  padding: 1rem;
`;

const Title = styled.h3`
  margin: 0 0 0.5rem 0;
  font-size: 1.1rem;
`;

const Content = styled.div`
  padding: 1rem;
`;

const InfoRow = styled.div`
  display: flex;
  align-items: center;
  margin-bottom: 0.5rem;
  color: #a0a0a0;
`;

const InfoIcon = styled.span`
  margin-right: 8px;
  display: flex;
  align-items: center;
`;

const GenresContainer = styled.div`
  display: flex;
  flex-wrap: wrap;
  gap: 0.5rem;
  margin-bottom: 1rem;
`;

const GenreBadge = styled.span`
  background: rgba(233, 69, 96, 0.2);
  color: #e94560;
  padding: 2px 6px;
  border-radius: 4px;
  font-size: 0.75rem;
`;

const BookButton = styled(Link)`
  display: block;
  background-color: #e94560;
  color: white;
  text-align: center;
  padding: 0.6rem;
  border-radius: 4px;
  margin-top: 0.5rem;
  transition: background-color 0.3s;
  
  &:hover {
    background-color: #ff6b81;
  }
`;

const InfoContainer = styled.div`
  display: flex;
  flex-wrap: wrap;
  margin-top: 0.5rem;
  color: #a0a0a0;
  font-size: 0.9rem;
`;

const InfoItem = styled.div`
  display: flex;
  align-items: center;
  margin-right: 1rem;
  margin-bottom: 0.5rem;
`;

const IconWrapper = styled.span`
  display: flex;
  align-items: center;
  margin-right: 0.3rem;
`;

const MovieCard = ({ movie }) => {
  // Determine if the movie is upcoming based on release date
  const isUpcoming = new Date(movie.releaseDate) > new Date();
  
  // Format duration (minutes) to hours and minutes
  const formatDuration = (minutes) => {
    const hours = Math.floor(minutes / 60);
    const mins = minutes % 60;
    return `${hours}h ${mins}m`;
  };

  // Extract genres from filmGenres array
  const getDisplayGenres = () => {
    if (!movie.filmGenres || movie.filmGenres.length === 0) {
      return [];
    }
    
    return movie.filmGenres
      .filter(fg => fg.genre && fg.genre.name)
      .map(fg => fg.genre.name)
      .slice(0, 2); // Chỉ lấy tối đa 2 thể loại
  };
  
  const displayGenres = getDisplayGenres();
  
  return (
    <Card to={`/movies/${movie.id}`}>
      <PosterContainer>
        <Poster 
          src={movie.imageURL || 'https://via.placeholder.com/300x450?text=No+Image'} 
          alt={movie.title} 
        />
        {isUpcoming && <Badge>Sắp chiếu</Badge>}
      </PosterContainer>
      
      <Content>
        <Title>{movie.title || 'Không có tiêu đề'}</Title>
        
        <InfoRow>
          <InfoIcon><FaClock /></InfoIcon>
          {movie.duration ? formatDuration(movie.duration) : 'N/A'}
        </InfoRow>
        
        <GenresContainer>
          {displayGenres.length > 0 ? (
            displayGenres.map((genre, index) => (
              <GenreBadge key={index}>{genre}</GenreBadge>
            ))
          ) : (
            <GenreBadge>Không có thể loại</GenreBadge>
          )}
        </GenresContainer>
        
        <BookButton to={`/movies/${movie.id}`}>
          Chi tiết & Đặt vé
        </BookButton>
      </Content>
    </Card>
  );
};

export default MovieCard;