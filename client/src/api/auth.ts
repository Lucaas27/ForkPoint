import { api, setAuthToken } from './client'

export async function register(payload: any) {
  const res = await api.post('/api/auth/register', payload)
  return res.data
}

export async function login(payload: any) {
  const res = await api.post('/api/auth/login', payload)
  const token = res.data?.token
  if (token) setAuthToken(token)
  return res.data
}

export async function logout() {
  const res = await api.post('/api/auth/logout')
  setAuthToken(undefined)
  return res.data
}

export async function refreshToken(payload: any) {
  const res = await api.post('/api/auth/refresh-token', payload)
  const token = res.data?.token
  if (token) setAuthToken(token)
  return res.data
}
