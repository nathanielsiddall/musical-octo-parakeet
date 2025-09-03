import { useState } from 'react';
import { useAuth } from '../AuthContext';
import { apiFetch } from '../api';

export default function Groups() {
  const { token } = useAuth();
  const [output, setOutput] = useState(null);
  const [groupId, setGroupId] = useState('');
  const [userId, setUserId] = useState('');
  const [permissionId, setPermissionId] = useState('');
  const [name, setName] = useState('');

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
      <h2>Groups</h2>

      <section>
        <h3>Create Group</h3>
        <input value={name} onChange={e => setName(e.target.value)} placeholder="Name" />
        <button onClick={() => handle('POST', '/api/auth/groups', { name })}>Create</button>
      </section>

      <section>
        <h3>Get All Groups</h3>
        <button onClick={() => handle('GET', '/api/auth/groups')}>Get Groups</button>
      </section>

      <section>
        <h3>Get Group By Id</h3>
        <input value={groupId} onChange={e => setGroupId(e.target.value)} placeholder="Group Id" />
        <button onClick={() => handle('GET', `/api/auth/groups/${groupId}`)}>Get Group</button>
      </section>

      <section>
        <h3>Update Group</h3>
        <input value={groupId} onChange={e => setGroupId(e.target.value)} placeholder="Group Id" />
        <input value={name} onChange={e => setName(e.target.value)} placeholder="New Name" />
        <button onClick={() => handle('PUT', `/api/auth/groups/${groupId}`, { name })}>Update</button>
      </section>

      <section>
        <h3>Delete Group</h3>
        <input value={groupId} onChange={e => setGroupId(e.target.value)} placeholder="Group Id" />
        <button onClick={() => handle('DELETE', `/api/auth/groups/${groupId}`)}>Delete</button>
      </section>

      <section>
        <h3>Add User To Group</h3>
        <input value={groupId} onChange={e => setGroupId(e.target.value)} placeholder="Group Id" />
        <input value={userId} onChange={e => setUserId(e.target.value)} placeholder="User Id" />
        <button onClick={() => handle('POST', `/api/auth/groups/${groupId}/users/${userId}`)}>Add</button>
      </section>

      <section>
        <h3>Remove User From Group</h3>
        <input value={groupId} onChange={e => setGroupId(e.target.value)} placeholder="Group Id" />
        <input value={userId} onChange={e => setUserId(e.target.value)} placeholder="User Id" />
        <button onClick={() => handle('DELETE', `/api/auth/groups/${groupId}/users/${userId}`)}>Remove</button>
      </section>

      <section>
        <h3>Assign Permission To Group</h3>
        <input value={groupId} onChange={e => setGroupId(e.target.value)} placeholder="Group Id" />
        <input value={permissionId} onChange={e => setPermissionId(e.target.value)} placeholder="Permission Id" />
        <button onClick={() => handle('POST', `/api/auth/groups/${groupId}/permissions`, { permissionId })}>Assign</button>
      </section>

      <section>
        <h3>Remove Group Permission</h3>
        <input value={groupId} onChange={e => setGroupId(e.target.value)} placeholder="Group Id" />
        <input value={permissionId} onChange={e => setPermissionId(e.target.value)} placeholder="Permission Id" />
        <button onClick={() => handle('DELETE', `/api/auth/groups/${groupId}/permissions/${permissionId}`)}>Remove</button>
      </section>

      <section>
        <h3>Get Group Permissions</h3>
        <input value={groupId} onChange={e => setGroupId(e.target.value)} placeholder="Group Id" />
        <button onClick={() => handle('GET', `/api/auth/groups/${groupId}/permissions`)}>Get Permissions</button>
      </section>

      <section>
        <h3>Get Group Users</h3>
        <input value={groupId} onChange={e => setGroupId(e.target.value)} placeholder="Group Id" />
        <button onClick={() => handle('GET', `/api/auth/groups/${groupId}/users`)}>Get Users</button>
      </section>

      <pre>{output && JSON.stringify(output, null, 2)}</pre>
    </div>
  );
}
