import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import styled from 'styled-components';
import { format } from 'date-fns';
import { FaStar, FaClock, FaCalendarAlt } from 'react-icons/fa';
import { movieService } from '../services/api';
import Header from '../components/common/Header';

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

const ButtonContainer = styled.div`
  display: flex;
  gap: 1rem;
`;

const Button = styled.a`
  display: inline-flex;
  align-items: center;
  justify-content: center;
  padding: 10px 20px;
  border-radius: 4px;
  font-weight: bold;
  text-decoration: none;
  cursor: pointer;
  transition: background 0.3s;

  ${props =>
    props.variant === 'primary' &&
    `
    background: #e94560;
    color: white;
    border: none;
    
    &:hover {
      background: #ff6b81;
    }
  `}

  ${props =>
    props.variant === 'outline' &&
    `
    background: transparent;
    color: #e94560;
    border: 2px solid #e94560;
    
    &:hover {
      background: #e94560;
      color: white;
    }
  `}

  ${props =>
    props.size === 'large' &&
    `
    padding: 12px 24px;
    font-size: 16px;
  `}
`;

const MovieDetailPage = () => {
  const { id } = useParams();
  const [movie, setMovie] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchMovieDetails = async () => {
      try {
        setLoading(true);
        const movieResponse = await movieService.getById(id);
        setMovie(movieResponse.data);
      } catch (err) {
        console.error('Error fetching movie details:', err);
        setError('Không thể tải thông tin phim. Vui lòng thử lại sau.');
      } finally {
        setLoading(false);
      }
    };

    fetchMovieDetails();
  }, [id]);

  const handleBookTicket = () => {
    alert('Chức năng đặt vé đang được phát triển!');
  };

  if (loading) return <PageContainer><Header /><div style={{ padding: '2rem' }}>Đang tải...</div></PageContainer>;
  if (error) return <PageContainer><Header /><div style={{ padding: '2rem' }}>{error}</div></PageContainer>;
  if (!movie) return <PageContainer><Header /><div style={{ padding: '2rem' }}>Không tìm thấy phim</div></PageContainer>;

  const displayPoster = movie.imageURL || 'https://via.placeholder.com/300x450?text=No+Image';
  const displayGenres = movie.filmGenres || [];
  const displayDuration = movie.duration ? `${Math.floor(movie.duration / 60)}h ${movie.duration % 60}m` : 'Không có thông tin';
  const displayReleaseDate = movie.releaseDate ? format(new Date(movie.releaseDate), 'dd/MM/yyyy') : 'Không có thông tin';

  return (
    <PageContainer>
      <Header />
      
      <BackdropContainer backdrop={displayPoster}>
        <ContentContainer>
          <PosterContainer>
            <Poster src={displayPoster} alt={movie.title} />
          </PosterContainer>
          
          <MovieInfo>
            <Title>{movie.title}</Title>
            
            <InfoGrid>
              <InfoItem>
                <InfoIcon><FaStar /></InfoIcon>
                <div>{movie.rating || 'N/A'}/10</div>
              </InfoItem>
              
              <InfoItem>
                <InfoIcon><FaClock /></InfoIcon>
                <div>{displayDuration}</div>
              </InfoItem>
              
              <InfoItem>
                <InfoIcon><FaCalendarAlt /></InfoIcon>
                <div>{displayReleaseDate}</div>
              </InfoItem>
            </InfoGrid>
            
            <GenresContainer>
              {displayGenres.length > 0 ? (
                displayGenres.map((genre, index) => (
                  <GenreBadge key={index}>{genre}</GenreBadge>
                ))
              ) : (
                <GenreBadge>Không có thể loại</GenreBadge>
              )}
            </GenresContainer>
            
            <Description>{movie.description}</Description>

            <ButtonContainer>
              <Button
                as="button"
                variant="primary"
                size="large"
                onClick={handleBookTicket}
              >
                Đặt vé xem phim
              </Button>
              {movie.trailerURL && (
                <Button
                  variant="outline"
                  size="large"
                  href={movie.trailerURL}
                  target="_blank"
                >
                  Xem trailer
                </Button>
              )}
            </ButtonContainer>
          </MovieInfo>
        </ContentContainer>
      </BackdropContainer>
    </PageContainer>
  );
};

export default MovieDetailPage;