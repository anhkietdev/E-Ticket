import React, { useState, useEffect } from 'react';
import { useParams, Link } from 'react-router-dom';
import styled from 'styled-components';
import { format } from 'date-fns';
import { FaClock, FaCalendarAlt } from 'react-icons/fa';
import { movieService } from './api';

const Container = styled.div`
  display: flex;
  margin-bottom: 2rem;
  padding: 20px;
  max-width: 1200px;
  margin: 0 auto;
  
  @media (max-width: 768px) {
    flex-direction: column;
  }
`;

const PosterContainer = styled.div`
  width: 300px;
  flex-shrink: 0;
  margin-right: 2rem;
  
  @media (max-width: 768px) {
    width: 100%;
    margin-right: 0;
    margin-bottom: 1.5rem;
  }
`;

const Poster = styled.img`
  width: 100%;
  border-radius: 8px;
  box-shadow: 0 5px 15px rgba(0, 0, 0, 0.5);
`;

const Content = styled.div`
  flex: 1;
`;

const Title = styled.h1`
  font-size: 2.5rem;
  color: #fff;
  margin-bottom: 1rem;
  
  @media (max-width: 768px) {
    font-size: 2rem;
  }
`;

const InfoGrid = styled.div`
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(150px, 1fr));
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
  color: #fff;
  margin-bottom: 2rem;
`;

const ButtonContainer = styled.div`
  display: flex;
  gap: 1rem;
  
  @media (max-width: 480px) {
    flex-direction: column;
  }
`;

const Button = styled(Link)`
  display: inline-flex;
  align-items: center;
  justify-content: center;
  padding: 10px 20px;
  border-radius: 4px;
  font-weight: bold;
  text-decoration: none;
  cursor: pointer;
  transition: background 0.3s;

  ${props => props.variant === 'primary' && `
    background: #e94560;
    color: white;
    border: none;
    
    &:hover {
      background: #ff6b81;
    }
  `}

  ${props => props.variant === 'outline' && `
    background: transparent;
    color: #e94560;
    border: 2px solid #e94560;
    
    &:hover {
      background: #e94560;
      color: white;
    }
  `}

  ${props => props.size === 'large' && `
    padding: 12px 24px;
    font-size: 16px;
  `}
`;

const ErrorMessage = styled.div`
  color: #e94560;
  text-align: center;
  font-size: 16px;
  margin: 20px;
`;

const Loading = styled.div`
  color: #fff;
  text-align: center;
  font-size: 16px;
  margin: 20px;
`;

const BackButton = styled(Link)`
  display: inline-block;
  color: #e94560;
  text-decoration: none;
  font-size: 16px;
  margin-top: 20px;
  
  &:hover {
    text-decoration: underline;
  }
`;

const MovieDetail = () => {
  const { id } = useParams();
  const [movie, setMovie] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchMovie = async () => {
      try {
        console.log('Bắt đầu gọi API với ID:', id);
        const response = await movieService.getById(id);
        console.log('Phản hồi API gốc:', response);
        console.log('Dữ liệu phim:', response.data);
        if (!response.data) {
          throw new Error('Dữ liệu phim rỗng');
        }
        setMovie(response.data);
        setLoading(false);
      } catch (err) {
        console.error('Chi tiết lỗi:', err);
        let errorMessage = 'Không thể tải thông tin phim. Vui lòng thử lại.';
        if (err.message === 'ID phim không hợp lệ') {
          errorMessage = 'ID phim không hợp lệ. Vui lòng kiểm tra lại.';
        } else if (err.response?.status === 404) {
          errorMessage = 'Phim không tồn tại hoặc đã bị xóa.';
        } else if (err.response?.status === 401) {
          errorMessage = 'Không có quyền truy cập. Vui lòng đăng nhập lại.';
        } else if (err.response?.status === 403) {
          errorMessage = 'Bạn không có quyền truy cập tài nguyên này.';
        } else if (err.message === 'Dữ liệu phim rỗng') {
          errorMessage = 'Không tìm thấy thông tin phim.';
        } else if (err.response) {
          errorMessage = `Lỗi từ server: ${err.response.status} - ${err.response.statusText}`;
        } else if (err.request) {
          errorMessage = 'Không nhận được phản hồi từ server. Vui lòng kiểm tra kết nối.';
        }
        setError(errorMessage);
        setLoading(false);
      }
    };

    if (!id) {
      setError('ID phim không hợp lệ.');
      setLoading(false);
      return;
    }

    fetchMovie();
  }, [id]);

  const handleBookTicket = () => {
    alert('Chức năng đặt vé đang được phát triển!');
  };

  if (loading) {
    return <Loading>Đang tải...</Loading>;
  }

  if (error) {
    return <ErrorMessage>{error}</ErrorMessage>;
  }

  if (!movie) {
    return <ErrorMessage>Không tìm thấy thông tin phim.</ErrorMessage>;
  }

  // Xử lý filmGenres để hỗ trợ cả mảng string và mảng object
  const displayGenres = Array.isArray(movie.filmGenres)
    ? movie.filmGenres.map(item => {
        // Nếu item là object với cấu trúc { genre: { name: "..." } }
        if (item && typeof item === 'object' && item.genre && item.genre.name) {
          return item.genre.name;
        }
        // Nếu item là string
        return item;
      }).filter(name => name) // Loại bỏ các giá trị không hợp lệ (null, undefined)
    : [];

  const displayTitle = movie.title || 'Không có tiêu đề';
  const displayDescription = movie.description || 'Không có mô tả';
  const displayDirector = movie.director || 'Không có thông tin';
  const displayImageUrl = movie.imageURL || 'https://via.placeholder.com/300x450?text=No+Image';
  const displayDuration = movie.duration ? `${Math.floor(movie.duration / 60)}h ${movie.duration % 60}m` : 'Không có thông tin';
  const displayReleaseDate = movie.releaseDate ? format(new Date(movie.releaseDate), 'dd/MM/yyyy') : 'Không có thông tin';
  const displayTrailerUrl = movie.trailerURL || '';

  return (
    <Container>
      <PosterContainer>
        <Poster src={displayImageUrl} alt={displayTitle} />
      </PosterContainer>
      <Content>
        <Title>{displayTitle}</Title>
        <InfoGrid>
          <InfoItem>
            <InfoIcon><FaClock /></InfoIcon>
            <div>{displayDuration}</div>
          </InfoItem>
          <InfoItem>
            <InfoIcon><FaCalendarAlt /></InfoIcon>
            <div>{displayReleaseDate}</div>
          </InfoItem>
          <InfoItem>
            <InfoIcon>Đạo diễn:</InfoIcon>
            <div>{displayDirector}</div>
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
        <Description>{displayDescription}</Description>
        <ButtonContainer>
          <Button as="button" variant="primary" size="large" onClick={handleBookTicket}>
            Đặt vé ngay
          </Button>
          {displayTrailerUrl && (
            <Button
              variant="outline"
              size="large"
              href={displayTrailerUrl}
              target="_blank"
            >
              Xem trailer
            </Button>
          )}
        </ButtonContainer>
        <BackButton to="/movies">Quay lại danh sách phim</BackButton>
      </Content>
    </Container>
  );
};

export default MovieDetail;