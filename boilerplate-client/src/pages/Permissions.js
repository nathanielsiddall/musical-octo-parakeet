import { useState } from 'react';
import { useAuth } from '../AuthContext';
import { apiFetch } from '../api';

export default function Permissions() {
  const { token } = useAuth();
  const [output, setOutput] = useState(null);
  const [permissionId, setPermissionId] = useState('');
  const [name, setName] = useState('');
  const [email, setEmail] = useState('');
  const [permission, setPermission] = useState('');

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
      <h2>Permissions</h2>

      <section>
        <h3>Create Permission</h3>
        <input value={name} onChange={e => setName(e.target.value)} placeholder="Name" />
        <button onClick={() => handle('POST', '/api/auth/permissions', { name })}>Create</button>
      </section>

      <section>
        <h3>Get All Permissions</h3>
        <button onClick={() => handle('GET', '/api/auth/permissions')}>Get All</button>
      </section>

      <section>
        <h3>Get Permission By Id</h3>
        <input value={permissionId} onChange={e => setPermissionId(e.target.value)} placeholder="Permission Id" />
        <button onClick={() => handle('GET', `/api/auth/permissions/${permissionId}`)}>Get</button>
      </section>

      <section>
        <h3>Update Permission</h3>
        <input value={permissionId} onChange={e => setPermissionId(e.target.value)} placeholder="Permission Id" />
        <input value={name} onChange={e => setName(e.target.value)} placeholder="New Name" />
        <button onClick={() => handle('PUT', `/api/auth/permissions/${permissionId}`, { newName: name })}>Update</button>
      </section>

      <section>
        <h3>Delete Permission</h3>
        <input value={permissionId} onChange={e => setPermissionId(e.target.value)} placeholder="Permission Id" />
        <button onClick={() => handle('DELETE', `/api/auth/permissions/${permissionId}`)}>Delete</button>
      </section>

      <section>
        <h3>Assign Permission To User</h3>
        <input value={email} onChange={e => setEmail(e.target.value)} placeholder="User Email" />
        <input value={permission} onChange={e => setPermission(e.target.value)} placeholder="Permission" />
        <button onClick={() => handle('POST', '/api/auth/permissions/assign', { email, permission })}>Assign</button>
      </section>

      <pre>{output && JSON.stringify(output, null, 2)}</pre>
    </div>
  );
}
