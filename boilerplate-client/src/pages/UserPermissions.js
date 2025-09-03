import { useState } from 'react';
import { useAuth } from '../AuthContext';
import { apiFetch } from '../api';

export default function UserPermissions() {
  const { token } = useAuth();
  const [output, setOutput] = useState(null);
  const [userId, setUserId] = useState('');
  const [permissionId, setPermissionId] = useState('');
  const [oldPermissionId, setOldPermissionId] = useState('');
  const [newPermissionId, setNewPermissionId] = useState('');

  const handle = async (method, url, body) => {
    const res = await apiFetch(token, url, {
      method,
      body: body ? JSON.stringify(body) : undefined
    });
    const data = await res.json().catch(() => null);
    setOutput(data);
  };

  return (
    <div>
      <h2>User Permissions</h2>

      <section>
        <h3>Assign Permission To User</h3>
        <input value={userId} onChange={e => setUserId(e.target.value)} placeholder="User Id" />
        <input value={permissionId} onChange={e => setPermissionId(e.target.value)} placeholder="Permission Id" />
        <button
          onClick={() =>
            handle('POST', '/api/user-permissions/assign', { userId, permissionId })
          }
        >
          Assign
        </button>
      </section>

      <section>
        <h3>Get User Permissions</h3>
        <input value={userId} onChange={e => setUserId(e.target.value)} placeholder="User Id" />
        <button onClick={() => handle('GET', `/api/users/${userId}/permissions`)}>
          Get Permissions
        </button>
      </section>

      <section>
        <h3>Get User Effective Permissions</h3>
        <input value={userId} onChange={e => setUserId(e.target.value)} placeholder="User Id" />
        <button onClick={() => handle('GET', `/api/auth/users/${userId}/effective-permissions`)}>
          Get Effective
        </button>
      </section>

      <section>
        <h3>Remove User Permission</h3>
        <input value={userId} onChange={e => setUserId(e.target.value)} placeholder="User Id" />
        <input value={permissionId} onChange={e => setPermissionId(e.target.value)} placeholder="Permission Id" />
        <button
          onClick={() =>
            handle('DELETE', `/api/auth/users/${userId}/permissions/${permissionId}`)
          }
        >
          Remove
        </button>
      </section>

      <section>
        <h3>Update User Permission</h3>
        <input value={userId} onChange={e => setUserId(e.target.value)} placeholder="User Id" />
        <input
          value={oldPermissionId}
          onChange={e => setOldPermissionId(e.target.value)}
          placeholder="Old Permission Id"
        />
        <input
          value={newPermissionId}
          onChange={e => setNewPermissionId(e.target.value)}
          placeholder="New Permission Id"
        />
        <button
          onClick={() =>
            handle('PUT', `/api/auth/users/${userId}/permissions`, {
              oldPermissionId,
              newPermissionId
            })
          }
        >
          Update
        </button>
      </section>

      <pre>{output && JSON.stringify(output, null, 2)}</pre>
    </div>
  );
}
