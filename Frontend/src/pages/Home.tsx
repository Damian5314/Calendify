import React from 'react';
import { useNavigate } from 'react-router-dom';

const Home: React.FC = () => {
  const navigate = useNavigate();

  const handleLogout = () => {
    sessionStorage.removeItem('role'); // Clear session
    navigate('/login'); // Redirect to login
  };

  return (
    <div style={{ padding: '20px' }}>
      <h1>Welcome Home</h1>
      <p>You are logged in as a user.</p>
      <button onClick={handleLogout} style={{ marginTop: '10px' }}>
        Log Out
      </button>
    </div>
  );
};

export default Home;
