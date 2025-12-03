import { api } from './client'

export async function updateMe(payload: any) {
  const res = await api.patch('/api/account/update', payload)
  return res.data
}

export async function adminUpdateUser(userId: number, payload: any) {
  const res = await api.patch(`/api/account/update/${userId}`, payload)
  return res.data
}

export async function forgotPassword(payload: any) {
  const res = await api.post('/api/account/forgot-password', payload)
  return res.data
}

export async function resetPassword(payload: any) {
  const res = await api.post('/api/account/reset-password', payload)
  return res.data
}

export async function confirmEmail(params: any) {
  const res = await api.get('/api/account/verify', { params })
  return res.data
}

export async function resendEmailConfirmation(payload: any) {
  const res = await api.post('/api/account/resend-email-confirmation', payload)
  return res.data
}

export async function myRestaurants() {
  const res = await api.get('/api/auth/restaurants')
  return res.data
}
