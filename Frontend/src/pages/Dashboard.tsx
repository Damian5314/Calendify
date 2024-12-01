import React from 'react';
import { useNavigate } from 'react-router-dom';

const Dashboard: React.FC = () => {
  const navigate = useNavigate();

  const handleLogout = () => {
    sessionStorage.removeItem('role'); // Clear session
    navigate('/login'); // Redirect to login
  };

  return (
    <div style={{ padding: '20px' }}>
      <h1>Admin Dashboard</h1>
      <p>Welcome to the admin panel!</p>
      <button onClick={handleLogout} style={{ marginTop: '10px' }}>
        Log Out
      </button>
    </div>
  );
};

export default Dashboard;
