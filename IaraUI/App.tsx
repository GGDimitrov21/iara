import React from 'react';
import { HashRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { Dashboard } from './pages/Dashboard';
import { Permits } from './pages/Permits';
import { Vessels } from './pages/Vessels';
import { Operations } from './pages/Operations';
import { Inspections } from './pages/Inspections';
import { Reports } from './pages/Reports';
import { Quotas } from './pages/Quotas';
import { Login } from './pages/Login';
import { Register } from './pages/Register';

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<Navigate to="/login" replace />} />
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
        <Route path="/dashboard" element={<Dashboard />} />
        <Route path="/permits" element={<Permits />} />
        <Route path="/vessels" element={<Vessels />} />
        <Route path="/operations" element={<Operations />} />
        <Route path="/inspections" element={<Inspections />} />
        <Route path="/reports" element={<Reports />} />
        <Route path="/quotas" element={<Quotas />} />
        <Route path="/settings" element={<Navigate to="/dashboard" replace />} />
      </Routes>
    </Router>
  );
}

export default App;