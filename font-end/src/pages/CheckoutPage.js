import React, { useState, useEffect } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import styled from 'styled-components';
import { format } from 'date-fns';
import { FaRegCheckCircle, FaTicketAlt, FaQrcode } from 'react-icons/fa';
import Header from '../components/common/Header';
import { useBooking } from '../context/BookingContext';
import { useAuth } from '../context/AuthContext';
import axios from 'axios';

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

const PageTitle = styled.h1`
  font-size: 2rem;
  margin-bottom: 2rem;
`;

const ErrorMessage = styled.div`
  text-align: center;
  padding: 2rem;
  color: #e94560;
  background: #16213e;
  border-radius: 8px;
  margin-bottom: 2rem;
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

const CheckoutGrid = styled.div`
  display: grid;
  grid-template-columns: 2fr 1fr;
  gap: 2rem;

  @media (max-width: 768px) {
    grid-template-columns: 1fr;
  }
`;

const Section = styled.div`
  background: #16213e;
  border-radius: 8px;
  padding: 1.5rem;
  margin-bottom: 2rem;
`;

const SectionTitle = styled.h3`
  display: flex;
  align-items: center;
  margin-bottom: 1.5rem;
  color: #e94560;
`;

const SectionIcon = styled.span`
  display: flex;
  align-items: center;
  margin-right: 0.8rem;
`;

const MovieInfo = styled.div`
  display: flex;
  margin-bottom: 1.5rem;
`;

const MoviePoster = styled.img`
  width: 100px;
  height: 150px;
  object-fit: cover;
  border-radius: 4px;
  margin-right: 1rem;
`;

const MovieDetails = styled.div`
  flex: 1;
`;

const MovieTitle = styled.h4`
  margin: 0 0 0.5rem;
  font-size: 1.2rem;
`;

const InfoRow = styled.div`
  display: flex;
  margin-bottom: 0.5rem;
  color: #a0a0a0;
  font-size: 0.9rem;
`;

const InfoLabel = styled.div`
  width: 100px;
  color: #a0a0a0;
`;

const InfoValue = styled.div`
  flex: 1;
  color: white;
`;

const BookingDetails = styled.div`
  margin-bottom: 1.5rem;
`;

const SeatsContainer = styled.div`
  display: flex;
  flex-wrap: wrap;
  gap: 0.5rem;
  margin-bottom: 1rem;
`;

const SeatBadge = styled.span`
  background: #1a1a2e;
  color: white;
  padding: 5px 10px;
  border-radius: 4px;
  font-size: 0.9rem;
`;

const PaymentMethods = styled.div`
  display: flex;
  flex-direction: column;
  gap: 1rem;
`;

const PaymentMethod = styled.div`
  display: flex;
  align-items: center;
  padding: 1rem;
  border-radius: 4px;
  border: 1px solid #e94560;
  background: rgba(233, 69, 96, 0.1);
  transition: all 0.3s;
`;

const PaymentIcon = styled.span`
  display: flex;
  align-items: center;
  margin-right: 1rem;
  font-size: 1.5rem;
  color: #e94560;
`;

const PaymentInfo = styled.div`
  flex: 1;
`;

const PaymentName = styled.div`
  font-weight: bold;
  margin-bottom: 0.2rem;
`;

const PaymentDescription = styled.div`
  font-size: 0.9rem;
  color: #a0a0a0;
`;

const OrderSummary = styled(Section)`
  position: sticky;
  top: 2rem;
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

const ButtonContainer = styled.div`
  margin-top: 2rem;
  display: flex;
  justify-content: space-between;
`;

const CompleteButton = styled(Button)`
  background: #e94560;
  color: white;
  border: none;

  &:hover:not(:disabled) {
    background: #ff6b81;
  }
`;

const SuccessModal = styled.div`
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.8);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
`;

const ModalContent = styled.div`
  background: #16213e;
  padding: 2rem;
  border-radius: 8px;
  max-width: 500px;
  width: 100%;
  text-align: center;
`;

const SuccessIcon = styled.div`
  font-size: 3rem;
  color: #4caf50;
  margin-bottom: 1rem;
`;

const ModalTitle = styled.h3`
  font-size: 1.5rem;
  margin-bottom: 1rem;
`;

const ModalMessage = styled.p`
  margin-bottom: 2rem;
  color: #a0a0a0;
`;

const ModalButton = styled(Button)`
  width: 100%;
  background: #e94560;
  color: white;
  border: none;

  &:hover {
    background: #ff6b81;
  }
`;

const CheckoutPage = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const { selectedMovie, selectedShowTime, selectedSeats, calculateTotalPrice, completeBooking, resetBooking } = useBooking();
  const { currentUser } = useAuth();

  const [isProcessing, setIsProcessing] = useState(false);
  const [showSuccessModal, setShowSuccessModal] = useState(false);
  const [bookingId, setBookingId] = useState(null);
  const [error, setError] = useState(null);

  const backendUrl = process.env.REACT_APP_API_URL || 'http://localhost:5086';

  useEffect(() => {
    console.log('Dữ liệu trong CheckoutPage:', {
      selectedMovie,
      selectedShowTime,
      selectedSeats,
    });

    if (!selectedMovie || !selectedShowTime || selectedSeats.length === 0) {
      setError('Dữ liệu đặt vé không đầy đủ. Vui lòng chọn lại suất chiếu và ghế.');
    }
  }, [selectedMovie, selectedShowTime, selectedSeats]);

  const handleBack = () => {
    navigate('/booking/seats');
  };

  const handleComplete = async () => {
    try {
      setIsProcessing(true);

      const booking = await completeBooking(currentUser.id);
      setBookingId(booking.id);

      const totalPrice = calculateTotalPrice();
      const paymentData = {
        bookingId: booking.id,
        amount: totalPrice,
        description: `Thanh toán đặt vé phim ${selectedMovie.title} - Mã đặt vé: ${booking.id}`,
        userId: currentUser.id,
        returnUrl: `${window.location.origin}/booking/payment-callback`,
      };

      const response = await axios.post(`${backendUrl}/api/payment/zalopay`, paymentData, {
        headers: { 'Content-Type': 'application/json' },
      });

      if (response.data && response.data.order_url) {

        window.location.href = response.data.order_url;
      } else {
        throw new Error('Không thể tạo giao dịch ZaloPay.');
      }
    } catch (error) {
      console.error('Lỗi khi thực hiện thanh toán:', error);
      alert('Đã có lỗi xảy ra khi thực hiện thanh toán. Vui lòng thử lại sau.');
      setIsProcessing(false);
    }
  };

  const handleCloseModal = () => {
    setShowSuccessModal(false);
    resetBooking();
    navigate('/');
  };

  const formatPrice = (price) => {
    return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(price);
  };

  if (error || !selectedMovie || !selectedShowTime || selectedSeats.length === 0) {
    return (
      <PageContainer>
        <Header />
        <ContentContainer>
          <ErrorMessage>{error || 'Dữ liệu đặt vé không đầy đủ. Vui lòng chọn lại suất chiếu và ghế.'}</ErrorMessage>
          <BackButton onClick={handleBack}>Quay lại chọn ghế</BackButton>
        </ContentContainer>
      </PageContainer>
    );
  }

  return (
    <PageContainer>
      <Header />
      <ContentContainer>
        <PageTitle>Thanh Toán</PageTitle>
        <CheckoutGrid>
          <div>
            <Section>
              <SectionTitle>
                <SectionIcon><FaTicketAlt /></SectionIcon>
                Thông Tin Vé
              </SectionTitle>
              <MovieInfo>
                <MoviePoster
                  src={selectedMovie.imageURL || 'https://via.placeholder.com/100x150?text=No+Image'}
                  alt={selectedMovie.title}
                />
                <MovieDetails>
                  <MovieTitle>{selectedMovie.title}</MovieTitle>
                  <InfoRow>
                    <InfoLabel>Phòng chiếu:</InfoLabel>
                    <InfoValue>{selectedShowTime.room?.roomNumber || 'Không xác định'}</InfoValue>
                  </InfoRow>
                  <InfoRow>
                    <InfoLabel>Ngày chiếu:</InfoLabel>
                    <InfoValue>{format(new Date(selectedShowTime.startTime), 'dd/MM/yyyy')}</InfoValue>
                  </InfoRow>
                  <InfoRow>
                    <InfoLabel>Suất chiếu:</InfoLabel>
                    <InfoValue>
                      {format(new Date(selectedShowTime.startTime), 'HH:mm')} -{' '}
                      {format(
                        new Date(selectedShowTime.endTime || new Date(selectedShowTime.startTime).setHours(new Date(selectedShowTime.startTime).getHours() + 2)),
                        'HH:mm'
                      )}
                    </InfoValue>
                  </InfoRow>
                </MovieDetails>
              </MovieInfo>
              <BookingDetails>
                <InfoRow>
                  <InfoLabel>Ghế:</InfoLabel>
                  <SeatsContainer>
                    {selectedSeats.map(seat => (
                      <SeatBadge key={seat.id}>
                        {seat.row}{seat.number} ({seat.type === 'vip' ? 'VIP' : 'Thường'})
                      </SeatBadge>
                    ))}
                  </SeatsContainer>
                </InfoRow>
              </BookingDetails>
            </Section>
            <Section>
              <SectionTitle>
                <SectionIcon><FaQrcode /></SectionIcon>
                Phương Thức Thanh Toán
              </SectionTitle>
              <PaymentMethods>
                <PaymentMethod>
                  <PaymentIcon>
                    <FaQrcode />
                  </PaymentIcon>
                  <PaymentInfo>
                    <PaymentName>ZaloPay</PaymentName>
                    <PaymentDescription>Thanh toán qua ví điện tử ZaloPay</PaymentDescription>
                  </PaymentInfo>
                </PaymentMethod>
              </PaymentMethods>
            </Section>
          </div>
          <OrderSummary>
            <SectionTitle>
              <SectionIcon><FaRegCheckCircle /></SectionIcon>
              Tóm Tắt Đơn Hàng
            </SectionTitle>
            <SummaryRow>
              <div>Phim:</div>
              <div>{selectedMovie.title}</div>
            </SummaryRow>
            <SummaryRow>
              <div>Suất chiếu:</div>
              <div>{format(new Date(selectedShowTime.startTime), 'HH:mm')}</div>
            </SummaryRow>
            <SummaryRow>
              <div>Số ghế:</div>
              <div>{selectedSeats.length}</div>
            </SummaryRow>
            <SummaryRow>
              <div>Giá vé:</div>
              <div>{formatPrice(selectedShowTime.price)}</div>
            </SummaryRow>
            {selectedSeats.some(seat => seat.type === 'vip') && (
              <SummaryRow>
                <div>Phụ thu VIP:</div>
                <div>{formatPrice(selectedSeats.filter(seat => seat.type === 'vip').length * (selectedShowTime.price * 0.2))}</div>
              </SummaryRow>
            )}
            <SummaryRow>
              <div>Tổng tiền:</div>
              <div>{formatPrice(calculateTotalPrice())}</div>
            </SummaryRow>
            <ButtonContainer>
              <BackButton onClick={handleBack}>Quay Lại</BackButton>
              <CompleteButton onClick={handleComplete} disabled={isProcessing}>
                {isProcessing ? 'Đang Xử Lý...' : 'Hoàn Tất Đặt Vé'}
              </CompleteButton>
            </ButtonContainer>
          </OrderSummary>
        </CheckoutGrid>
      </ContentContainer>
      {showSuccessModal && (
        <SuccessModal>
          <ModalContent>
            <SuccessIcon>
              <FaRegCheckCircle />
            </SuccessIcon>
            <ModalTitle>Đặt Vé Thành Công!</ModalTitle>
            <ModalMessage>
              Cảm ơn bạn đã đặt vé tại Cinema Booking. <br />
              Mã đặt vé của bạn là: <strong>#{bookingId}</strong> <br />
              Thông tin chi tiết đã được gửi đến email của bạn.
            </ModalMessage>
            <ModalButton onClick={handleCloseModal}>
              Quay Lại Trang Chủ
            </ModalButton>
          </ModalContent>
        </SuccessModal>
      )}
    </PageContainer>
  );
};

export default CheckoutPage;