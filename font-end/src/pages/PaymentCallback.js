import React, { useEffect, useState } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import styled from 'styled-components';
import axios from 'axios';
import { FaRegCheckCircle, FaTimesCircle } from 'react-icons/fa';
import Header from '../components/common/Header';

const PageContainer = styled.div`
  background-color: #0f0f1e;
  color: #fff;
  min-height: 100vh;
  display: flex;
  justify-content: center;
  align-items: center;
`;

const MessageContainer = styled.div`
  background: #16213e;
  padding: 2rem;
  border-radius: 8px;
  text-align: center;
 :max-width: 500px;
`;

const Icon = styled.div`
  font-size: 3rem;
  margin-bottom: 1rem;
  color: ${props => (props.success ? '#4caf50' : '#e94560')};
`;

const Title = styled.h2`
  font-size: 1.5rem;
  margin-bottom: 1rem;
`;

const Message = styled.p`
  color: #a0a0a0;
  margin-bottom: 2rem;
`;

const Button = styled.button`
  padding: 12px 24px;
  background: #e94560;
  color: white;
  border: none;
  border-radius: 4px;
  font-weight: bold;
  cursor: pointer;
  &:hover {
    background: #ff6b81;
  }
`;

const PaymentCallback = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const [status, setStatus] = useState(null);
  const [message, setMessage] = useState('');
  const backendUrl = process.env.REACT_APP_API_URL || 'http://localhost:5086';

  useEffect(() => {
    const query = new URLSearchParams(location.search);
    const apptransid = query.get('apptransid');
    const status = query.get('status');

    if (apptransid && status) {
      const verifyPayment = async () => {
        try {
          const response = await axios.get(`${backendUrl}/api/payment/verify`, {
            params: { apptransid },
          });
          if (response.data.success) {
            setStatus('success');
            setMessage(`Thanh toán thành công! Mã giao dịch: ${apptransid}`);
          } else {
            setStatus('error');
            setMessage('Thanh toán thất bại. Vui lòng thử lại.');
          }
        } catch (error) {
          setStatus('error');
          setMessage('Đã có lỗi xảy ra. Vui lòng liên hệ hỗ trợ.');
        }
      };
      verifyPayment();
    } else {
      setStatus('error');
      setMessage('Thông tin thanh toán không hợp lệ.');
    }
  }, [location, backendUrl]);

  const handleBack = () => {
    navigate('/');
  };

  return (
    <PageContainer>
      <Header />
      <MessageContainer>
        <Icon success={status === 'success'}>
          {status === 'success' ? <FaRegCheckCircle /> : <FaTimesCircle />}
        </Icon>
        <Title>{status === 'success' ? 'Thanh Toán Thành Công' : 'Thanh Toán Thất Bại'}</Title>
        <Message>{message}</Message>
        <Button onClick={handleBack}>Quay Lại Trang Chủ</Button>
      </MessageContainer>
    </PageContainer>
  );
};

export default PaymentCallback;