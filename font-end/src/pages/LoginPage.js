import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import styled from 'styled-components';
import { FaUser, FaLock, FaEnvelope, FaPhone } from 'react-icons/fa';
import Header from '../components/common/Header';
import { useAuth } from '../context/AuthContext';

// [Giữ nguyên các styled-components như trong mã gốc]

const LoginPage = () => {
  const navigate = useNavigate();
  const { login, register, error: authError } = useAuth();

  const [isLogin, setIsLogin] = useState(true);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const [formData, setFormData] = useState({
    email: '',
    password: '',
    name: '',
    phone: '',
  });

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: value,
    });
  };

  const validateForm = () => {
    const { email, password, name, phone } = formData;

    if (isLogin) {
      if (!email || !password) {
        setError('Vui lòng nhập đầy đủ thông tin');
        return false;
      }
    } else {
      if (!email || !password || !name || !phone) {
        setError('Vui lòng nhập đầy đủ thông tin');
        return false;
      }

      if (password.length < 6) {
        setError('Mật khẩu phải có ít nhất 6 ký tự');
        return false;
      }

      if (!/^\d{10}$/.test(phone)) {
        setError('Số điện thoại không hợp lệ');
        return false;
      }
    }

    setError(null);
    return true;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!validateForm()) return;

    try {
      setLoading(true);

      if (isLogin) {
        await login(formData.email, formData.password);
      } else {
        await register(formData);
      }

      navigate('/');
    } catch (err) {
      console.error('Lỗi xác thực:', err);
      setError(authError || err.message || 'Đã có lỗi xảy ra. Vui lòng thử lại.');
    } finally {
      setLoading(false);
    }
  };

  const toggleForm = () => {
    setIsLogin(!isLogin);
    setError(null);
  };

  return (
    <PageContainer>
      <Header />
      <ContentContainer>
        <FormContainer>
          <PageTitle>{isLogin ? 'Đăng nhập' : 'Đăng ký'}</PageTitle>
          <Form onSubmit={handleSubmit}>
            {!isLogin && (
              <FormGroup>
                <FormIcon><FaUser /></FormIcon>
                <Input
                  type="text"
                  name="name"
                  placeholder="Họ và tên"
                  value={formData.name}
                  onChange={handleChange}
                />
              </FormGroup>
            )}
            <FormGroup>
              <FormIcon><FaEnvelope /></FormIcon>
              <Input
                type="email"
                name="email"
                placeholder="Email"
                value={formData.email}
                onChange={handleChange}
              />
            </FormGroup>
            <FormGroup>
              <FormIcon><FaLock /></FormIcon>
              <Input
                type="password"
                name="password"
                placeholder="Mật khẩu"
                value={formData.password}
                onChange={handleChange}
              />
            </FormGroup>
            {!isLogin && (
              <FormGroup>
                <FormIcon><FaPhone /></FormIcon>
                <Input
                  type="tel"
                  name="phone"
                  placeholder="Số điện thoại"
                  value={formData.phone}
                  onChange={handleChange}
                />
              </FormGroup>
            )}
            {(error || authError) && <ErrorMessage>{error || authError}</ErrorMessage>}
            <Button type="submit" disabled={loading}>
              {loading ? 'Đang xử lý...' : isLogin ? 'Đăng nhập' : 'Đăng ký'}
            </Button>
          </Form>
          <ToggleContainer>
            <ToggleText>
              {isLogin ? 'Chưa có tài khoản?' : 'Đã có tài khoản?'}
            </ToggleText>
            <ToggleLink onClick={toggleForm}>
              {isLogin ? 'Đăng ký ngay' : 'Đăng nhập'}
            </ToggleLink>
          </ToggleContainer>
        </FormContainer>
      </ContentContainer>
    </PageContainer>
  );
};

export default LoginPage;