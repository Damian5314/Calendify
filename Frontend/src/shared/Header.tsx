import React from 'react';
import { useNavigate } from 'react-router-dom';

const Header: React.FC = () => {
  const navigate = useNavigate();
  const role = sessionStorage.getItem('role');

  const handleLogout = () => {
    sessionStorage.clear();
    navigate('/login');
  };

  return (
    <header style={{ padding: '10px', borderBottom: '1px solid #ccc' }}>
      <h1>My App</h1>
      {role && (
        <>
          <span>{`Logged in as ${role}`}</span>
          <button onClick={handleLogout} style={{ marginLeft: '10px' }}>
            Log Out
          </button>
        </>
      )}
    </header>
  );
};

export default Header;
