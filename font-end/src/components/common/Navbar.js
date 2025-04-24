/*import React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import styled from 'styled-components';
import { FaUser, FaSignOutAlt, FaFilm, FaTicketAlt, FaCog } from 'react-icons/fa';
import { useAuth } from '../context/AuthContext';

const NavbarContainer = styled.nav`
  background-color: #16213e;
  color: white;
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0.5rem 2rem;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
`;

const Logo = styled(Link)`
  font-size: 1.5rem;
  font-weight: bold;
  color: white;
  text-decoration: none;
  display: flex;
  align-items: center;
  
  &:hover {
    color: #e94560;
  }
`;

const NavLinks = styled.div`
  display: flex;
  align-items: center;
  gap: 1.5rem;
  
  @media (max-width: 768px) {
    gap: 1rem;
  }
`;

const NavLink = styled(Link)`
  color: white;
  text-decoration: none;
  font-weight: 500;
  display: flex;
  align-items: center;
  gap: 0.5rem;
  
  &:hover {
    color: #e94560;
  }
  
  @media (max-width: 768px) {
    font-size: 0.9rem;
  }
`;

const AdminLink = styled(NavLink)`
  background-color: #e94560;
  padding: 0.4rem 0.8rem;
  border-radius: 4px;
  
  &:hover {
    background-color: #ff6b81;
    color: white;
  }
`;

const Button = styled.button`
  background: transparent;
  border: none;
  color: white;
  cursor: pointer;
  font-size: 1rem;
  font-weight: 500;
  display: flex;
  align-items: center;
  gap: 0.5rem;
  
  &:hover {
    color: #e94560;
  }
  
  @media (max-width: 768px) {
    font-size: 0.9rem;
  }
`;

const Navbar = () => {
  const { currentUser, userRole, logout } = useAuth();
  const navigate = useNavigate();
  
  const handleLogout = () => {
    logout();
    navigate('/');
  };
  
  return (
    <NavbarContainer>
      <Logo to="/">
        <FaFilm style={{ marginRight: '0.5rem' }} />
        Cinema Booking
      </Logo>
      
      <NavLinks>
        <NavLink to="/movies">
          <FaTicketAlt /> Phim
        </NavLink>
        
        {currentUser ? (
          // User is logged in
          <>
            {userRole === 'staff' ? (
              // Staff-specific links
              <AdminLink to="/staff">
                <FaCog /> Quản lý
              </AdminLink>
            ) : (
              // Regular user links
              <NavLink to="/profile">
                <FaUser /> {currentUser.name || currentUser.username || 'Tài khoản'}
              </NavLink>
            )}
            
            <Button onClick={handleLogout}>
              <FaSignOutAlt /> Đăng xuất
            </Button>
          </>
        ) : (
          // User is not logged in
          <>
            <NavLink to="/login">
              <FaUser /> Đăng nhập
            </NavLink>
          </>
        )}
      </NavLinks>
    </NavbarContainer>
  );
};

export default Navbar;*/


///skiplogin and author 
import React from 'react';
import { Link } from 'react-router-dom';
import styled from 'styled-components';
import { FaFilm, FaTicketAlt, FaCog } from 'react-icons/fa';

const NavbarContainer = styled.nav`
  background-color: #16213e;
  color: white;
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0.5rem 2rem;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
`;

const Logo = styled(Link)`
  font-size: 1.5rem;
  font-weight: bold;
  color: white;
  text-decoration: none;
  display: flex;
  align-items: center;
  
  &:hover {
    color: #e94560;
  }
`;

const NavLinks = styled.div`
  display: flex;
  align-items: center;
  gap: 1.5rem;
  
  @media (max-width: 768px) {
    gap: 1rem;
  }
`;

const NavLink = styled(Link)`
  color: white;
  text-decoration: none;
  font-weight: 500;
  display: flex;
  align-items: center;
  gap: 0.5rem;
  
  &:hover {
    color: #e94560;
  }
  
  @media (max-width: 768px) {
    font-size: 0.9rem;
  }
`;

const AdminLink = styled(NavLink)`
  background-color: #e94560;
  padding: 0.4rem 0.8rem;
  border-radius: 4px;
  
  &:hover {
    background-color: #ff6b81;
    color: white;
  }
`;

const Navbar = () => {
  return (
    <NavbarContainer>
      <Logo to="/">
        <FaFilm style={{ marginRight: '0.5rem' }} />
        Cinema Booking
      </Logo>
      
      <NavLinks>
        <NavLink to="/movies">
          <FaTicketAlt /> Phim
        </NavLink>
        <AdminLink to="/staff">
          <FaCog /> Quản lý
        </AdminLink>
      </NavLinks>
    </NavbarContainer>
  );
};

export default Navbar;