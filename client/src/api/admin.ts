import { api } from './client'

export async function assignRole(payload: any) {
  const res = await api.post('/api/admin/users/roles', payload)
  return res.data
}

export async function removeRole(payload: any) {
  const res = await api.delete('/api/admin/users/roles', { data: payload })
  return res.data
}
