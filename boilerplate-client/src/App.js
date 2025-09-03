import { BrowserRouter, Routes, Route, Link } from 'react-router-dom';
import { AuthProvider, RequireAuth } from './AuthContext';
import Login from './pages/Login';
import Signup from './pages/Signup';
import Groups from './pages/Groups';
import Permissions from './pages/Permissions';
import UserPermissions from './pages/UserPermissions';

export default function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <nav>
          <Link to="/login">Login</Link> |{' '}
          <Link to="/signup">Signup</Link> |{' '}
          <Link to="/groups">Groups</Link> |{' '}
          <Link to="/permissions">Permissions</Link> |{' '}
          <Link to="/user-permissions">User Permissions</Link>
        </nav>
        <Routes>
          <Route path="/login" element={<Login />} />
          <Route path="/signup" element={<Signup />} />
          <Route path="/groups" element={<RequireAuth><Groups /></RequireAuth>} />
          <Route path="/permissions" element={<RequireAuth><Permissions /></RequireAuth>} />
          <Route path="/user-permissions" element={<RequireAuth><UserPermissions /></RequireAuth>} />
          <Route path="*" element={<Login />} />
        </Routes>
      </BrowserRouter>
    </AuthProvider>
  );
}
